using hgi.Environment;
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

        public static void RequestBill(string itemId)
        {
            try
            {
                Debug.Log("Cashmatic", string.Format("Bill request for {0}", itemId));

                if (ConfigurationManager.AppSettings["ready2orderAuthorization"] != null)
                {
                    string ready2orderAuthorization = ConfigurationManager.AppSettings["ready2orderAuthorization"];
                    string printer = ConfigurationManager.AppSettings["printer"];
                    string parameters = ConfigurationManager.AppSettings["parameters"];
                    string restUri = ConfigurationManager.AppSettings["ready2orderUri"];

                    restUri = restUri.Replace("[ITEMID]", itemId);

                    var restClient = new RestClient(restUri);
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("content-type", "multipart/form-data;");
                    request.AddHeader("authorization", ready2orderAuthorization);
                    IRestResponse response = restClient.Execute(request);
                    RootObject ob = SimpleJson.DeserializeObject<RootObject>(response.Content);

                    if (!ob.ready2order.error)
                    {
                        Debug.Log("Cashmatic", "Request succsessfull.");
                        using (var webClient = new WebClient())
                        {
                            webClient.DownloadFile(ob.ready2order.uri, "Bills\\"+itemId+".pdf");
                        }
                        PDFPrinterX Prn = new PDFPrinterX();
                        Prn.LogFile = "PDFPrinter.log";
                        Prn.Print("Bills\\" + itemId + ".pdf", printer, parameters);
                        if (Prn.ErrorMessage != null)
                        {
                            Debug.Log("Cashmatic", Prn.ErrorMessage);
                        }
                        else
                        {
                            Debug.Log("Cashmatic", "Print succsessfull.");
                        }
                    }
                    else
                    {
                        Debug.Log("Cashmatic", "Server error.");
                    }
                }
                else
                {
                    Debug.Log("Cashmatic",string.Format("Can not make request for item[{0}]. ready2orderAuthorization key not found in application configuration file. ", itemId));
                }  
            }
            catch (Exception ex)
            {
                Debug.Log("Cashmatic", ex.ToString());
            }

        }

        public static RootObject RequestParkingDetails(string itemId)
        {
            RootObject ob = new RootObject();
            try
            {
                Debug.Log("Cashmatic", string.Format("Bill request for {0}", itemId));

                if (ConfigurationManager.AppSettings["pandaParkenAuthorization"] != null)
                {
                    string pandaParkenAuthorization = ConfigurationManager.AppSettings["pandaParkenAuthorization"];
                    string printer = ConfigurationManager.AppSettings["printer"];
                    string parameters = ConfigurationManager.AppSettings["parameters"];
                    string restUri = ConfigurationManager.AppSettings["pandaParkeUri"];

                    restUri = restUri.Replace("[ITEMID]", itemId);

                    var restClient = new RestClient(restUri);
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("content-type", "multipart/form-data;");
                    request.AddParameter("authorization", pandaParkenAuthorization);
                    request.AddParameter("itemId", itemId);
                    IRestResponse response = restClient.Execute(request);
                  
                    ob = SimpleJson.DeserializeObject<RootObject>(response.Content);

                }
                else
                {
                    Debug.Log("Cashmatic", string.Format("Can not make request for item[{0}]. pandaParkenAuthorization key not found in application configuration file. ", itemId));
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Cashmatic", ex.ToString());
            }
            return ob;
        }
    }
}
