using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashmaticApp
{
    public class Global
    {

        public static string ready2orderAuthorization = "";
        public static string pandaParkenAuthorization = "";
        public static string printer = "";
        public static string parameters = "";
        public static string ready2orderUri = "";
        public static string pandaParkenExternalCheckoutUri = "";
        public static string pandaParkenExternalGetBillDataUri = "";
        public static string terminalId = "";
        public static string terminalLog = "";
        public static bool isCashPayment = false;
        public static bool isCardPayment = false;
        public static string cashmaticKey = "";
        public static string cashmaticInitializationVector = "";
        public static string cashmaticBasePath = "";
        public static double thankYouTimer = 0;
        public static int sixPrintReceiptWidth = 0;
        public static string merchantReceipt = "";
        public static string cardholderReceipt = "";
        public static int subtotale = 0;
        public static int pagato = 0;
        public static string request_bill_id;
        public static string ready2order_hash;
        public static string pandaParkenDetailsJson = "";
        public static string ready2orderJson = "";
        public static RootObject bill_data = null;
        public static TerminalCommands terminalCommands = null;
        public static int BalanceTimeFrom = 2;
        public static int BalanceTimeTo = 3;
        public static double BalanceTimer = 1500000;
    }
}
