using System;
using System.Collections.Generic;
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
}
