using CashmaticApp.Pages;
using hgi.Environment;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CashmaticApp.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Debug.Log("CashmaticApp", "Initializing main window");
            InitializeComponent();

            
            string baseDir = System.AppDomain.CurrentDomain.BaseDirectory;
            string pathTodirectoryBills = baseDir + "Bills";
            string pathTodirectoryDebugLog = baseDir + "DebugLog";
            string pathTodirectoryReceipts = baseDir + "Receipts";
            string pathTodirectoryBalances = baseDir + "Balances";

            Debug.Log("CashmaticApp", "Basedir: "+baseDir);
            Debug.Log("CashmaticApp", "pathTodirectoryBills: " + pathTodirectoryBills);
            Debug.Log("CashmaticApp", "pathTodirectoryDebugLog: " + pathTodirectoryDebugLog);
            Debug.Log("CashmaticApp", "pathTodirectoryReceipts: " + pathTodirectoryReceipts);
            Debug.Log("CashmaticApp", "pathTodirectoryBalances: " + pathTodirectoryBalances);


            try
            {
                System.IO.Directory.CreateDirectory(pathTodirectoryBills);
                System.IO.Directory.CreateDirectory(pathTodirectoryDebugLog);
                System.IO.Directory.CreateDirectory(pathTodirectoryReceipts);
                System.IO.Directory.CreateDirectory(pathTodirectoryBalances);

                Global.ready2orderAuthorization = ConfigurationManager.AppSettings["ready2orderAuthorization"];
                Global.pandaParkenAuthorization = ConfigurationManager.AppSettings["pandaParkenAuthorization"];
                Global.printer = ConfigurationManager.AppSettings["printer"];
                Global.parameters = ConfigurationManager.AppSettings["parameters"];
                Global.ready2orderUri = ConfigurationManager.AppSettings["ready2orderUri"];
                Global.pandaParkenExternalGetBillDataUri = ConfigurationManager.AppSettings["pandaParkenExternalGetBillDataUri"];
                Global.pandaParkenExternalCheckoutUri = ConfigurationManager.AppSettings["pandaParkenExternalCheckoutUri"];
                Global.terminalId = ConfigurationManager.AppSettings["terminalId"];
                Global.terminalLog = ConfigurationManager.AppSettings["terminalLog"];
                Global.isCashPayment = bool.Parse(ConfigurationManager.AppSettings["isCashPayment"]);
                Global.isCardPayment = bool.Parse(ConfigurationManager.AppSettings["isCardPayment"]);
                Global.cashmaticKey = ConfigurationManager.AppSettings["cashmaticKey"];
                Global.cashmaticInitializationVector = ConfigurationManager.AppSettings["cashmaticInitializationVector"];
                Global.cashmaticBasePath = ConfigurationManager.AppSettings["cashmaticBasePath"];
                Global.thankYouTimer = double.Parse(ConfigurationManager.AppSettings["thankYouTimer"]);
                Global.sixPrintReceiptWidth = int.Parse(ConfigurationManager.AppSettings["sixPrintReceiptWidth"]);
                if (ConfigurationManager.AppSettings["CardPaymentPrintPageSizeAddHeight"] != null) { Global.CardPaymentPrintPageSizeAddHeight = int.Parse(ConfigurationManager.AppSettings["CardPaymentPrintPageSizeAddHeight"]); };
                if (ConfigurationManager.AppSettings["CardPaymentPrintStartAt"] != null) { Global.CardPaymentPrintStartAt = int.Parse(ConfigurationManager.AppSettings["CardPaymentPrintStartAt"]); };
                if (ConfigurationManager.AppSettings["CardPaymentPrintLineHeight"] != null) { Global.CardPaymentPrintLineHeight = int.Parse(ConfigurationManager.AppSettings["CardPaymentPrintLineHeight"]); };
                if (ConfigurationManager.AppSettings["CardPaymentPrintLeft"] != null) { Global.CardPaymentPrintLeft = int.Parse(ConfigurationManager.AppSettings["CardPaymentPrintLeft"]); };
   


        string balanceTimeFrom = ConfigurationManager.AppSettings["balanceTimeFrom"];
                string balanceTimeTo = ConfigurationManager.AppSettings["balanceTimeTo"];

                int timeFrom = 0;
                int timeTo = 0;

                //if number, must be between 1 and 24
                bool isNumeric = int.TryParse(balanceTimeFrom, out timeFrom);
                if(isNumeric && timeFrom >=1 && timeFrom<=24)
                {
                    Global.BalanceTimeFrom = timeFrom;
                }
                //if number, must be between 1 and 24
                isNumeric = int.TryParse(balanceTimeTo, out timeTo);
                if (isNumeric && timeTo >= 1 && timeTo <= 24)
                {
                    Global.BalanceTimeTo= timeTo;
                }
                //if time to is smaller than time from, reverse the time order (from becomes to and vice versa)
                if(timeTo<=timeFrom)
                {
                    Global.BalanceTimeFrom = timeTo;
                    Global.BalanceTimeTo = timeFrom;
                }

                Global.BalanceTimer = double.Parse( ConfigurationManager.AppSettings["balanceTimer"]);

                if(File.Exists(ConfigurationManager.AppSettings["CashmaticAppPath"]))
                {
                    if (Global.isCashPayment)
                    {
                        File.Delete(Global.cashmaticBasePath + "connected");
                        System.Diagnostics.Process[] pname = System.Diagnostics.Process.GetProcessesByName("Cashmatic");
                        if (pname.Length == 0)
                        {
                            System.Diagnostics.ProcessStartInfo PSI = new System.Diagnostics.ProcessStartInfo();
                            PSI.FileName = System.IO.Path.GetFileName(ConfigurationManager.AppSettings["CashmaticAppPath"]);
                            PSI.WorkingDirectory = System.IO.Path.GetDirectoryName(ConfigurationManager.AppSettings["CashmaticAppPath"]);
                            PSI.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                            PSI.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized;
                            System.Diagnostics.Process.Start(PSI);
                        }
                    }
                }
                else
                {
                    Debug.Log("CashmaticApp", string.Format("File path {0} does not exists", ConfigurationManager.AppSettings["CashmaticAppPath"]));
                    Global.isCashPayment = false; 
                }
               

                this.Content = new TicketScanPage();

            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
                Helper.ShowResponseMessage("ERROR_MAIN", "SYSTEM_ERROR");
            }
          
        
        
        
          
        }
    }
}
