//using hgi.Environment;
using hgi.Environment;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace CashmaticApp
{
    public static class Helper
    {

        private static Newtonsoft.Json.Formatting indented = Newtonsoft.Json.Formatting.Indented;
        private static JsonSerializerSettings settings = new JsonSerializerSettings() {TypeNameHandling = TypeNameHandling.All };

        public static string ObjectToXml(object o)
        {
            StringWriter sw = new Utf8StringWriter();
            XmlTextWriter tw = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(o.GetType());
                tw = new XmlTextWriter(sw);
                serializer.Serialize(tw, o);
            }
            catch (Exception e)
            {
                //Debug.Log("Cashmatic", e.ToString());
            }
            finally
            {
                sw.Close();
                if (tw != null)
                {
                    tw.Close();
                }
            }
            return sw.ToString();
        }
        public static Object XmlToObject(string xml, Type objectType)
        {
            StringReader strReader = null;
            XmlSerializer serializer = null;
            XmlTextReader xmlReader = null;
            Object obj = null;
            try
            {
                strReader = new StringReader(xml);
                serializer = new XmlSerializer(objectType);
                xmlReader = new XmlTextReader(strReader);
                obj = serializer.Deserialize(xmlReader);
            }
            catch (Exception e)
            {
                //Debug.Log("Cashmatic", e.ToString());
            }
            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
                if (strReader != null)
                {
                    strReader.Close();
                }
            }
            return obj;
        }
        public static string ObjectToJSON(object o)
        {
  
            string result = null;
            try
            {
                result = JsonConvert.SerializeObject(o, indented, settings);
            }
            catch(Exception ex)
            {
                //Debug.Log("Cashmatic", ex.ToString());
            }
         
            return result;
        }
        public static T JSONToObject<T>(string json) where T : new()
        {


            T obj = new T();
            try
            {
              obj = JsonConvert.DeserializeObject<T>(json, settings);
            }
            catch(Exception ex)
            {
                
                //Debug.Log("Cashmatic", ex.ToString());
            }

            return obj;
        }
        public static bool isFileLocked(FileInfo file)
        {
            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }

        public static void ShowResponseMessage(string status, string message)
        {
            Windows.MessageBox mb = null;

            if (!string.IsNullOrWhiteSpace(status) && !string.IsNullOrWhiteSpace(message))
            {
                object resource = Application.Current.Resources[message];
                if (resource != null)
                {
                    mb = new Windows.MessageBox(resource.ToString());
                    if(!status.Equals("ERROR_MAIN"))
                    {
                        mb.Owner = ((App)Application.Current).MainWindow;
                    }
                    mb.ShowDialog();
                }
            }
         
        }
        public static void SaveReceiptsData(string transactionId,string reciever,string content)
        {
            try
            {
                string todaysDate = DateTime.Now.ToString("yyyyMMdd");
                string pathTodirectory = string.Format("Receipts\\{0}", todaysDate);
                System.IO.Directory.CreateDirectory(pathTodirectory);
                string pathToFile = string.Format("{0}\\{1}_{2}.txt", pathTodirectory, transactionId, reciever);
                System.IO.File.WriteAllText(pathToFile, content);
            }
            catch(Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }
        }
        private static string FormatToCurrency(int unformattedNumber)
        {
            float f = unformattedNumber * 0.01f;
            return f.ToString("0.00");
        }
        private static int parseInt(string amount)
        {
            // Split string by decimal point
            string[] s = amount.Split(',');
            string final = "";
            int payoutAmount = 0;

            // If there was a decimal point
            if (s.Length > 1)
            {
                // Add a trailing zero if necessary
                if (s[1].Length == 1)
                    s[1] += "0";
                // If more than 2 decimal places, cull end
                else if (s[1].Length > 2)
                    s[1] = s[1].Substring(0, 2);

                final += s[0] + s[1]; // Add to final result string
            }
            else
                final += s[0] + "00"; // Add two zeros if there is no decimal point entered

            try
            {
                // Parse it to a number
                return payoutAmount = Int32.Parse(final);
            }
            catch
            {
                //MessageBox.Show("Error in splitting amount", "Cashmatic - Plus");
                return 0;
            }
        }

    }
}
