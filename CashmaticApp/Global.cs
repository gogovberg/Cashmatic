using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashmaticApp
{
    public class Global
    {
        public static string base_path = "C:/Cashmatic/";
        public static int subtotale = 0;
        public static int pagato = 0;
        public static string request_bill_id;
        public static string ready2order_hash;
        public static string pandaParkenDetailsJson = "";
        public static string ready2orderJson = "";
        public static RootObject bill_data = null;
    }
}
