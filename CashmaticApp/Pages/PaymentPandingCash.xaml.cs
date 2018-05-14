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
        private FileSystemWatcher _fileWatcher = null;
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
 
            CashmaticCommands.WriteSubtotale(_ob.payment.paymentSummary.total);
            tblPrice.Text = String.Format("{0:0.00}€", _ob.payment.paymentSummary.total / 100);

            if(!File.Exists(Helper.base_path + "saldato.txt"))
            {
                _dirWatcher = new FileSystemWatcher();
                _dirWatcher.Path = Helper.base_path;
                _dirWatcher.Filter = "*.txt*";
                _dirWatcher.Changed += new FileSystemEventHandler(OnChangedDir);
       
            }
            else
            {
                SetSaldatoChangeListener();
            }
        }
        private void OnChangedDir(object source, FileSystemEventArgs e)
        {
            if (File.Exists(Helper.base_path + "pagato.txt") && _fileWatcher==null)
            {
                SetSaldatoChangeListener();
            }
        }
        private void OnChange(object source, FileSystemEventArgs e)
        {
            int saldato = CashmaticCommands.ReadSaldato();
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background, new Action(() => String.Format("{0:0.00}€", saldato / 100)));
        }
        private void SetSaldatoChangeListener()
        {
            _fileWatcher = new FileSystemWatcher();
            _fileWatcher.Path = Helper.base_path;
            _fileWatcher.Filter = Helper.base_path + "saldato.txt";
            _fileWatcher.EnableRaisingEvents = true;
            _fileWatcher.NotifyFilter = NotifyFilters.Attributes |
                                        NotifyFilters.CreationTime |
                                        NotifyFilters.FileName |
                                        NotifyFilters.LastAccess |
                                        NotifyFilters.LastWrite |
                                        NotifyFilters.Size |
                                        NotifyFilters.Security;
            _fileWatcher.Changed += new FileSystemEventHandler(OnChange);
        }
    }
}
