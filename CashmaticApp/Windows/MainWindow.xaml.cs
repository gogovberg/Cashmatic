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


            try
            {
                System.IO.Directory.CreateDirectory(pathTodirectoryBills);
                System.IO.Directory.CreateDirectory(pathTodirectoryDebugLog);
                System.IO.Directory.CreateDirectory(pathTodirectoryReceipts);

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

             
                File.Delete(Global.cashmaticBasePath+"connected");

                //System.Diagnostics.Process.Start(ConfigurationManager.AppSettings["CashmaticAppPath"]);

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
