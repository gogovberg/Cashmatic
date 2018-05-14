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
                Debug.Log("Cashmatic", e.ToString());
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
                Debug.Log("Cashmatic", e.ToString());
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
                Debug.Log("Cashmatic", ex.ToString());
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
                
                Debug.Log("Cashmatic", ex.ToString());
            }

            return obj;
        }
    }
}
