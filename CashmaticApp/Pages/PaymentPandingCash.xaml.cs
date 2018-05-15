using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace CashmaticApp.Pages
{
    /// <summary>
    /// Interaction logic for PaymentPanding.xaml
    /// </summary>
    public partial class PaymentPandingCash : Page
    {
        private RootObject _ob;
        private FileSystemWatcher _dirWatcher = null;
        private FileSystemWatcher _saldatoWatcher = null;
        private FileSystemWatcher _pagatoWatcher = null;

        public PaymentPandingCash(RootObject ob)
        {
            InitializeComponent();
            _ob = ob;
            InitPayment();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new RefoundPending(_ob);
        }

        private void InitPayment()
        {
            int amountLeft = _ob.payment.paymentSummary.total;

            if(_ob.payment.paymentSummary.OnPayment)
            {
                int saldato = CashmaticCommands.ReadSaldato();
                amountLeft = _ob.payment.paymentSummary.total - saldato;
            }
            else
            {
                CashmaticCommands.WriteSubtotale(_ob.payment.paymentSummary.total);
                _ob.payment.paymentSummary.OnPayment = true;
            }
            
            tblPrice.Text = String.Format("{0:0.00}€", amountLeft /(double) 100);

            SetSaldatoChangeListener();
            SetPagatoChangeListener();
        }

        private void OnChangedDir(object source, FileSystemEventArgs e)
        {
            if (File.Exists(Helper.base_path + "saldato.txt") && _saldatoWatcher == null)
            {
                SetSaldatoChangeListener();
            }
        }

        private void OnChangeSaldato(object source, FileSystemEventArgs e)
        {
            int saldato = CashmaticCommands.ReadSaldato();
            saldato = _ob.payment.paymentSummary.total - saldato;
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background, new Action(() => tblPrice.Text = String.Format("{0:0.00}€", saldato /(double) 100)));
        }

        private void OnChangedPagato(object source, FileSystemEventArgs e)
        {
            int pagato = CashmaticCommands.ReadPagato();
            int saldato = CashmaticCommands.ReadPagato();
            int erogato = CashmaticCommands.ReadErogato();
            int nonerogat = CashmaticCommands.ReadNonerogato();

            if(pagato==_ob.payment.paymentSummary.total && saldato==_ob.payment.paymentSummary.total)
            {
                
                Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background, new Action(() => Application.Current.MainWindow.Content = new ThankYouCash()));
            }
          
        }

        private void SetSaldatoChangeListener()
        {
            _saldatoWatcher = new FileSystemWatcher();
            _saldatoWatcher.Path = Helper.base_path;
            _saldatoWatcher.Filter ="saldato.txt";
            _saldatoWatcher.EnableRaisingEvents = true;
            _saldatoWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _saldatoWatcher.Changed += new FileSystemEventHandler(OnChangeSaldato);
            _saldatoWatcher.EnableRaisingEvents = true;
        }

        private void SetPagatoChangeListener()
        {
            _pagatoWatcher = new FileSystemWatcher();
            _pagatoWatcher.Path = Helper.base_path;
            _pagatoWatcher.Filter = "pagato.txt";
            _pagatoWatcher.EnableRaisingEvents = true;
            _pagatoWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _pagatoWatcher.Created += new FileSystemEventHandler(OnChangedPagato);
            _pagatoWatcher.Changed += new FileSystemEventHandler(OnChangedPagato);
            _pagatoWatcher.EnableRaisingEvents = true;
        }
    }
}
