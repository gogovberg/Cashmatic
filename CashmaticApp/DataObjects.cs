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
        public int ItemPrice { set; get; }
        public int ItemTotal { set; get; }
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
        public string vat1 { get; set; }
        public string vat2 { get; set; }
        public int total { get; set; }
        public bool OnPayment { set; get; }
    }

    public class Item
    {
        public string name { get; set; }
        public int qty { get; set; }
        public int price { get; set; }
        public int total { set; get; }
    }
    public class Payment
    {
        public PaymentSummary paymentSummary { get; set; }
        public List<Item> item { get; set; }
    }
    public class RootObject
    {
        public Payment payment { get; set; }
    }
}
