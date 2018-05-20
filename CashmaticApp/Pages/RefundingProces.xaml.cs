using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for RefundingProcessxaml.xaml
    /// </summary>
    public partial class RefundingProces : Page
    {
        private FileSystemWatcher _fileWatcher = null;
        private RootObject _ob;
        public RefundingProces(RootObject ob, bool CannotDispense)
        {
            InitializeComponent();
            _ob = ob;
            _fileWatcher = new FileSystemWatcher();
            _fileWatcher.Path = Helper.base_path;

            _fileWatcher.Filter = !CannotDispense ? "annulla.txt" : "subtotale.txt";

            _fileWatcher.Deleted += new FileSystemEventHandler(OnDeleted);
            _fileWatcher.EnableRaisingEvents = true;
            if(!CannotDispense)
            {
                CashmaticCommands.WriteAnnulla();
            }
            else
            {
                int pagato = CashmaticCommands.ReadPagato();
                CashmaticCommands.WriteSubtotale(pagato*(-1));
            }

        }
        private void OnDeleted(object source, FileSystemEventArgs e)
        {
   
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background, new Action(() => Application.Current.MainWindow.Content = new TicketScanPage()));
        }
    }
}
