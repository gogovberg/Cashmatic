using hgi.Environment;
using Newtonsoft.Json.Linq;
//using PDFPrinter;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CashmaticApp
{
    public static class TransactionLogic
    {
        public static RootObject RequestParkingDetails(string hash)
        {
            RootObject ob = new RootObject();
            ob.isError = true;
            try
            {
                Debug.Log("CashmaticApp", string.Format("Bill request for {0}", hash));
                var restClient = new RestClient(Global.pandaParkenExternalGetBillDataUri);
                var request = new RestRequest(Method.GET);
                request.AddHeader("content-type", "multipart/form-data;");
                request.AddParameter("authorization", Global.pandaParkenAuthorization);
                request.AddParameter("bid", hash);
                IRestResponse response = restClient.Execute(request);
                ob = SimpleJson.DeserializeObject<RootObject>(response.Content);
                ob.isError = true;

                if (ob != null && ob.status.Equals("OK"))
                {
                    Global.request_bill_id = hash;
                    ob.isError = false;
                    PaymentSummary(ob);

                }
                Helper.ShowResponseMessage(ob.status, ob.message);
                Debug.Log("CashmaticApp", string.Format("STATUS: {0} MESSAGE:{1}", ob.status, ob.message));
            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }
            return ob;
        }

        public static void RequestBill(RootObject globalData)
        {
            try
            {
                Debug.Log("CashmaticApp", string.Format("Bill request for {0}", Global.request_bill_id));

                string jsonOrderdata = SimpleJson.SerializeObject(globalData.ready2order);
                var restClient = new RestClient(Global.ready2orderUri);
                var request = new RestRequest(Method.PUT);
                request.AddHeader("content-type", "multipart/form-data;");
                request.AddHeader("authorization", Global.ready2orderAuthorization);
                request.AddParameter("application/json", jsonOrderdata, ParameterType.RequestBody);
                IRestResponse response = restClient.Execute(request);
                RootObject ob = SimpleJson.DeserializeObject<RootObject>(response.Content);

                string status = ob.isError ? "ERROR" : "OK";
                string message = string.IsNullOrEmpty(ob.message) ? "" : ob.message;

                Helper.ShowResponseMessage(status, message);
                Debug.Log("CashmaticApp", string.Format("STATUS: {0} MESSAGE:{1}", status, message));


                string todaysDate = DateTime.Now.ToString("yyyyMMdd");

                string pathTodirectory = string.Format("Bills\\{0}", todaysDate);

                System.IO.Directory.CreateDirectory(pathTodirectory);

                if (!string.IsNullOrEmpty(ob.invoice_pdf))
                {
                    globalData.invoice_id = ob.invoice_id;
                    globalData.invoice_numberFull = ob.invoice_numberFull;
                    globalData.invoice_pdf = ob.invoice_pdf;

                    Debug.Log("CashmaticApp", "Request succsessfull.");
                    string pathToFile = string.Format("{0}\\{1}.pdf", pathTodirectory, Global.request_bill_id);
                    using (var webClient = new WebClient())
                    {
                        webClient.DownloadFile(ob.invoice_pdf, pathToFile);
                    }
                    //PDFPrinterX Prn = new PDFPrinterX();
                    //Prn.LogFile = "PDFPrinter.log";
                    //Prn.Print(pathToFile, Global.printer, Global.parameters);
                    //if (Prn.ErrorMessage != null)
                    //{
                    //    Debug.Log("CashmaticApp", Prn.ErrorMessage);
                    //}
                    //else
                    //{
                    //    Debug.Log("CashmaticApp", "Print succsessfull.");
                    //}
                }
                else
                {
                    Debug.Log("CashmaticApp", "Server error.");
                }
            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }

        }

        public static void ExternalCheckout(RootObject ob)
        {
            try
            {
              
                Debug.Log("CashmaticApp", string.Format("External checkout for {0}", Global.request_bill_id));
                var restClient = new RestClient(Global.pandaParkenExternalCheckoutUri);
                var request = new RestRequest(Method.GET);
                request.AddHeader("content-type", "multipart/form-data;");
                request.AddParameter("authorization", Global.pandaParkenAuthorization);
                request.AddParameter("bid", Global.request_bill_id);
                request.AddParameter("paymentMethod ", ob.ready2order.paymentMethod_id);
                request.AddParameter("invoiceId ", ob.invoice_id);
                request.AddParameter("invoiceNumberFull ", ob.invoice_numberFull);
                request.AddParameter("merchantReceipt", Global.merchantReceipt);
                IRestResponse response = restClient.Execute(request);
                RootObject tempob = SimpleJson.DeserializeObject<RootObject>(response.Content);
                Helper.ShowResponseMessage(tempob.status, tempob.message);
                Debug.Log("CashmaticApp", string.Format("STATUS: {0} MESSAGE:{1}", tempob.status, tempob.message));
            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }
          
        }

        private static void PaymentSummary(RootObject ob)
        {
            try
            {
                if(ob!=null)
                {
                    ob.ready2order.total = 0;
                    foreach (Item it in ob.ready2order.items)
                    {
                        ob.ready2order.total = it.item_price * it.item_quantity;
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }
        }
    }
}
