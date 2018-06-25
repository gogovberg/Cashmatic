using hgi.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CashmaticApp.Pages
{
    /// <summary>
    /// Interaction logic for ThankYou.xaml
    /// </summary>
    public partial class ThankYouCash : Page
    {
        private System.Timers.Timer _thankyouPrint;
        private object _thankyouObject = new object();
        private double _thankyouTimer;

        public ThankYouCash(RootObject ob)
        {
            Debug.Log("CashmaticApp", "Initializing thank you cash");
            InitializeComponent();

            TransactionLogic.RequestBill(ob);
            TransactionLogic.ExternalCheckout(ob);

            _thankyouTimer = Global.thankYouTimer;

            _thankyouPrint = new System.Timers.Timer();
            _thankyouPrint.Elapsed += new ElapsedEventHandler(RedirectToTicketScan);
            _thankyouPrint.Interval = _thankyouTimer; // 1000 ms => 1 second
            _thankyouPrint.Enabled = true;

            
        }
        private void RedirectToTicketScan(object source, ElapsedEventArgs e)
        {
            _thankyouPrint.Enabled = false;
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(() => Application.Current.MainWindow.Content = new TicketScanPage()));
            
        }
    }
}
