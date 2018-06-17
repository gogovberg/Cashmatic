using hgi.Environment;
using Newtonsoft.Json.Linq;
using PDFPrinter;
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

        public static void RequestBill()
        {
            try
            {
                Debug.Log("CashmaticApp", string.Format("Bill request for {0}", Global.request_bill_id));

                if (ConfigurationManager.AppSettings["ready2orderAuthorization"] != null && !string.IsNullOrWhiteSpace(Global.pandaParkenDetailsJson))
                {
                    string ready2orderAuthorization = ConfigurationManager.AppSettings["ready2orderAuthorization"];
                    string printer = ConfigurationManager.AppSettings["printer"];
                    string parameters = ConfigurationManager.AppSettings["parameters"];
                    string restUri = ConfigurationManager.AppSettings["ready2orderUri"];

                    var restClient = new RestClient(restUri);
                    var request = new RestRequest(Method.PUT);
                    request.AddHeader("content-type", "multipart/form-data;");
                    request.AddHeader("authorization", ready2orderAuthorization);
                    request.AddParameter("application/json", Global.pandaParkenDetailsJson, ParameterType.RequestBody);
                    IRestResponse response = restClient.Execute(request);
                    RootObject ob = SimpleJson.DeserializeObject<RootObject>(response.Content);

                    string todaysDate = DateTime.Now.ToString("yyyyMMdd");

                    string pathTodirectory = string.Format("Bills\\{0}", todaysDate);

                    System.IO.Directory.CreateDirectory(pathTodirectory);

                    if (!string.IsNullOrEmpty(ob.invoice_pdf))
                    {
                        Debug.Log("CashmaticApp", "Request succsessfull.");
                        string pathToFile = string.Format("{0}\\{1}.pdf", pathTodirectory, Global.request_bill_id);
                        using (var webClient = new WebClient())
                        {
                            webClient.DownloadFile(ob.invoice_pdf, pathToFile);
                        }
                        PDFPrinterX Prn = new PDFPrinterX();
                        Prn.LogFile = "PDFPrinter.log";
                        Prn.Print(pathToFile, printer, parameters);
                        if (Prn.ErrorMessage != null)
                        {
                            Debug.Log("CashmaticApp", Prn.ErrorMessage);
                        }
                        else
                        {
                            Debug.Log("CashmaticApp", "Print succsessfull.");
                        }
                    }
                    else
                    {
                        Debug.Log("CashmaticApp", "Server error.");
                    }
                }
                else
                {
                    Debug.Log("CashmaticApp",string.Format("Can not make request for item[{0}]. ready2orderAuthorization key not found in application configuration file. ", Global.request_bill_id));
                }  
            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }

        }

        public static RootObject RequestParkingDetails(string hash)
        {
            RootObject ob = new RootObject();
            ob.isError = true;
            try
            {
                Debug.Log("CashmaticApp", string.Format("Bill request for {0}", hash));

                if (ConfigurationManager.AppSettings["pandaParkenAuthorization"] != null)
                {
                    string pandaParkenAuthorization = ConfigurationManager.AppSettings["pandaParkenAuthorization"];  
                    string restUri = ConfigurationManager.AppSettings["pandaParkeUri"];


                    var restClient = new RestClient(restUri);
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("content-type", "multipart/form-data;");
                    request.AddParameter("authorization", pandaParkenAuthorization);
                    request.AddParameter("bid", hash);
                    IRestResponse response = restClient.Execute(request);
                  
                    ob = SimpleJson.DeserializeObject<RootObject>(response.Content);
                   
                    if (ob!=null && ob.status.Equals("OK"))
                    {
                      
                        JObject results = JObject.Parse(response.Content);

                        JToken token = results.SelectToken("ready2order");
                        if (token != null)
                        {
                            Global.pandaParkenDetailsJson = token.ToString();
                            Global.request_bill_id = hash;

                            ob.isError = false;
                            PaymentSummary(ob);
                            
                        }
                        else
                        {
                            Debug.Log("CashmaticApp", "ready2order data is missing");
                        }
                       
                    }
                   
                    Debug.Log("CashmaticApp", string.Format("STATUS: {0} MESSAGE:{1}", ob.status, ob.message));
                }
                else
                {

                    Debug.Log("CashmaticApp", string.Format("Can not make request for item[{0}]. pandaParkenAuthorization key not found in application configuration file. ", hash));
                }
            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }
            return ob;
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
                Debug.Log("Cashmatic", ex.ToString());
            }
        }
    }
}
