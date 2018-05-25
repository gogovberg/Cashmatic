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
      
        private FileSystemWatcher _saldatoWatcher = null;
        private FileSystemWatcher _pagatoWatcher = null;
        private FileSystemWatcher _erogatoWatcher = null;

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
            SetErogatoChangeListener();
        }

        private void OnChangedDir(object source, FileSystemEventArgs e)
        {
            if (File.Exists(Global.base_path + "saldato.txt") && _saldatoWatcher == null)
            {
                SetSaldatoChangeListener();
            }
        }

        private void OnChangeSaldato(object source, FileSystemEventArgs e)
        {
            FileInfo file = new FileInfo(e.FullPath);
            while (Helper.isFileLocked(file))
            {
                Thread.Sleep(50);
            }
            try
            {
                int saldato = CashmaticCommands.ReadSaldato();
                Global.pagato = saldato;
                saldato = _ob.payment.paymentSummary.total - saldato;
            
                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background, new Action(() => tblPrice.Text = String.Format("{0:0.00}€", saldato / (double)100)));
            }
            catch (Exception ex)
            {

            }
         
        }

        private void OnCreatedPagato(object source, FileSystemEventArgs e)
        {

            FileInfo file = new FileInfo(e.FullPath);

            while (Helper.isFileLocked(file))
            {
                Thread.Sleep(50);
            }
            try
            {
                int pagato = CashmaticCommands.ReadPagato();
                int saldato = CashmaticCommands.ReadPagato();
                int erogato = CashmaticCommands.ReadErogato();
                int nonerogat = CashmaticCommands.ReadNonerogato();
                Global.pagato = pagato;

                if (pagato == _ob.payment.paymentSummary.total && saldato == _ob.payment.paymentSummary.total)
                {
                    file.Delete();
                    Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background, new Action(() => Application.Current.MainWindow.Content = new ThankYouCash()));
                }
            }
            catch(Exception ex)
            {
                //TODO:Handle exception
            }

           
        }
        private void OnCreatedErogato(object source, FileSystemEventArgs e)
        {
            FileInfo file = new FileInfo(e.FullPath);
            while (Helper.isFileLocked(file))
            {
                Thread.Sleep(50);
            }
            try
            {
              
                int erogato = CashmaticCommands.ReadErogato();
                int resto = Global.pagato - Global.subtotale;

                if ((resto - erogato) > 0)
                {
                    file.Delete();
                    Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background, new Action(() => Application.Current.MainWindow.Content = new RefundingProces(_ob,true)));
                }
            }
            catch(Exception ex)
            {
                //TODO: handle error
            }
         
        }
        private void SetSaldatoChangeListener()
        {
            _saldatoWatcher = new FileSystemWatcher();
            _saldatoWatcher.Path = Global.base_path;
            _saldatoWatcher.Filter ="saldato.txt";
            _saldatoWatcher.Changed += new FileSystemEventHandler(OnChangeSaldato);
            _saldatoWatcher.EnableRaisingEvents = true;
        }
        private void SetPagatoChangeListener()
        {
            _pagatoWatcher = new FileSystemWatcher();
            _pagatoWatcher.Path = Global.base_path;
            _pagatoWatcher.Filter = "pagato.txt";
            _pagatoWatcher.EnableRaisingEvents = true;
            _pagatoWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _pagatoWatcher.Created += new FileSystemEventHandler(OnCreatedPagato);
            _pagatoWatcher.EnableRaisingEvents = true;
        }
        private void SetErogatoChangeListener()
        {
            _erogatoWatcher = new FileSystemWatcher();
            _erogatoWatcher.Path = Global.base_path;
            _erogatoWatcher.Filter = "erogato.txt";
            _erogatoWatcher.Created += new FileSystemEventHandler(OnCreatedErogato);
            _erogatoWatcher.EnableRaisingEvents = true;
        }
    }
}
