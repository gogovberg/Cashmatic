using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashmaticApp
{
    public class SummaryItem
    {
        public int ItemID { set; get; }
        public string ItemName { set; get; }
        public int ItemQty { set; get; }
        public string ItemPrice { set; get; }
        public string ItemTotal { set; get; }
    }
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
    public class MoneyLevels
    {
        public Dictionary<int, int> Coins { set; get; }
        public Dictionary<int, int> Notes { set; get; }
        public Dictionary<int, int> Stacker { set; get; }
    }
    public class PaymentSummary
    {
        public string checkin { get; set; }
        public string checkout { get; set; }
        public int basePrice { get; set; }
        public int vat1 { get; set; }
        public int vat2 { get; set; }
        public int total { get; set; }
        public bool OnPayment { set; get; }
        public string Language { set; get; }
    }

   
    public class Payment
    {
        public PaymentSummary paymentSummary { get; set; }
        public List<Item> item { get; set; }
    }
  
    public class Item
    {
        public int item_quantity { get; set; }
        public string item_name { get; set; }
        public string item_comment { get; set; }
        public int item_price { get; set; }
        public int item_vatRate { get; set; }
        public int product_id { get; set; }
        public int discount_id { get; set; }
        public string discount_unit { get; set; }
        public int discount_value { get; set; }
        public string item_priceBase { get; set; }
    }

    public class Panda
    {
        public string checkindate { get; set; }
        public string checkoutdate { get; set; }
        public string language { set; get; }
        public double base_price { set; get; }
        public double vat_rate_one { set; get; }
        public double vat_rate_two { set; get; }
        public double total_price { set; get; }
        public bool OnPayment { set; get; }
    }
    public class Address
    {
        public string company { get; set; }
        public string vatId { get; set; }
        public string title { get; set; }
        public string salutation { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string country { get; set; }
    }

    public class Ready2order
    {
        public bool error { get; set; }
        public string uri { get; set; }
        public string format { get; set; }
        public bool createPDF { get; set; }
        public string pdfFormat { get; set; }
        public int invoice_inPrinterQueue { get; set; }
        public string invoice_roundToSmallestCurrencyUnit { get; set; }
        public string invoice_externalReferenceNumber { get; set; }
        public string invoice_showRecipient { get; set; }
        public string invoice_text { get; set; }
        public string invoice_priceBase { get; set; }
        public bool invoice_testMode { get; set; }
        public int paymentMethod_id { get; set; }
        public int user_id { get; set; }
        public int customer_id { get; set; }
        public List<Item> items { get; set; }
        public Address address { get; set; }
        public int total { set; get; }
        
    }
    public class RootObject
    {
        public string status { set; get; }
        public string message { set; get; }
        public Panda panda { get; set; }
        public Ready2order ready2order { get; set; }
    }
}
