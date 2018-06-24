using hgi.Environment;
using SIX.TimApi;
using System;
using System.Collections.Generic;
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
using static SIX.TimApi.Terminal;

namespace CashmaticApp.Pages
{
    /// <summary>
    /// Interaction logic for PaymentPandingCard.xaml
    /// </summary>
    public partial class PaymentPandingCard : Page
    {
        private RootObject _ob;

        private SIX.TimApi.Constants.TransactionStatus _transactionStatus;
        private SIX.TimApi.Constants.CardReaderStatus _cardReaderStatus;
        private SIX.TimApi.Constants.ConnectionStatus _connectionStatus;

        private bool _isTransactionComplete;
    
        public PaymentPandingCard(RootObject ob)
        {
            Debug.Log("CashmaticApp", "Initializing payment paning card");
            InitializeComponent();
            _ob = ob;
            tblReminder.Text = string.Format("Amount to pay {0}", string.Format("{0:0.00}€", _ob.panda.total_price));


            if (Global.terminalCommands==null)
            {
                Global.terminalCommands = new TerminalCommands();

            }
            Global.terminalCommands.TransactionError += terminal_transactionError;
            Global.terminalCommands.StatusChanged += terminal_statusChanged;
            Global.terminalCommands.TransactionCompleted += terminal_transactionCompleted;

            Global.terminalCommands.onPurchase(ob.panda.total_price);


            _isTransactionComplete = false;
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Debug.Log("CashmaticApp", "Button cancel click");
            _isTransactionComplete = false;
            Global.terminalCommands.onCancel();
            Global.terminalCommands.TransactionError -= terminal_transactionError;
            Global.terminalCommands.StatusChanged -= terminal_statusChanged;
            Global.terminalCommands.TransactionCompleted -= terminal_transactionCompleted;
            System.Windows.Application.Current.MainWindow.Content = new PayingProblemCard(_ob);
          
        }
        private void terminal_transactionError(object sender, TimException e)
        {
            System.Windows.Application.Current.MainWindow.Content = new PayingProblemCard(_ob);
        }
        private void terminal_transactionCompleted(object sender, TransactionCompletedEventArgs EventArgs)
        {
            _isTransactionComplete = true;
            System.Windows.Application.Current.MainWindow.Content = new ThankYouCard(_ob);
           
        }
        private void terminal_statusChanged(object sender, TerminalStatus e)
        {
            _transactionStatus = e.TransactionStatus;
            _cardReaderStatus = e.CardReaderStatus;
            _connectionStatus = e.ConnectionStatus;
            if (!_isTransactionComplete)
            {
                if (e.TransactionStatus == SIX.TimApi.Constants.TransactionStatus.Processing ||
                    e.TransactionStatus == SIX.TimApi.Constants.TransactionStatus.ReadingCard ||
                    e.TransactionStatus == SIX.TimApi.Constants.TransactionStatus.PinEntry)
                {
                    System.Windows.Application.Current.MainWindow.Content = new TransactionWait(_ob);
                }
            }
            else
            {
                if (_cardReaderStatus == SIX.TimApi.Constants.CardReaderStatus.CardReaderEmpty &&
                        _transactionStatus == SIX.TimApi.Constants.TransactionStatus.Idle)
                {
                    Global.terminalCommands.TransactionError -= terminal_transactionError;
                    Global.terminalCommands.StatusChanged -= terminal_statusChanged;
                    Global.terminalCommands.TransactionCompleted -= terminal_transactionCompleted;
                    System.Windows.Application.Current.MainWindow.Content = new TicketScanPage();
                }

            }
          

        }

    }
}
