using hgi.Environment;
using SIX.TimApi;
using System.Windows;
using System.Windows.Controls;
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
        private App _currentApp = ((App)System.Windows.Application.Current);

        private bool _isTransactionComplete;
        private bool _isTransactionError;
    
        public PaymentPandingCard(RootObject ob)
        {
            Debug.Log("CashmaticApp", "Initializing payment paning card");
            InitializeComponent();
            _ob = ob;
            tblReminder.Text = string.Format("{0} {1}", _currentApp.Resources["titleAmounToPayCard"].ToString(), string.Format("{0:0.00}€", _ob.panda.total_price));


            if (Global.terminalCommands==null)
            {
                Global.terminalCommands = new TerminalCommands();

            }
            Global.terminalCommands.TransactionError += terminal_transactionError;
            Global.terminalCommands.StatusChanged += terminal_statusChanged;
            Global.terminalCommands.TransactionCompleted += terminal_transactionCompleted;
            Global.terminalCommands.ActivateCompleted += terminal_activateCompleted;
            Global.terminalCommands.DeactivateCompleted += terminal_deactivateCompleted;

            if (Global.terminalCommands.isActivated)
            {
                Global.terminalCommands.onPurchase(_ob.panda.total_price);
            }
            else
            {
                Global.terminalCommands.onActivate();
            }

            _isTransactionComplete = false;
            _isTransactionError = false;
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Debug.Log("CashmaticApp", "Button cancel click");
            _isTransactionComplete = false;
            Global.terminalCommands.onCancel();
            TerminalListenersClear();
            System.Windows.Application.Current.MainWindow.Content = new PayingProblemCard(_ob);
          
        }
        private void terminal_activateCompleted(object sender, ActivateCompletedEventArgs EventArgs)
        {
            Global.terminalCommands.onPurchase(_ob.panda.total_price);
        }
        private void terminal_deactivateCompleted(object sender, DeactivateCompletedEventArgs EventArgs)
        {
            if(EventArgs.DeactivateResponse.PrintData!=null)
            {

            }
        }
        private void terminal_transactionError(object sender, TimException e)
        {
            _isTransactionError = true;
            TerminalListenersClear();
            System.Windows.Application.Current.MainWindow.Content = new PayingProblemCard(_ob);
            
        }
        private void terminal_transactionCompleted(object sender, TransactionCompletedEventArgs EventArgs)
        {
            _isTransactionComplete = true;
            string tranId = EventArgs.TransactionResponse.TransactionInformation.TransRef;
            Global.merchantReceipt = EventArgs.TransactionResponse.PrintData.MerchantReceipt;
            Global.cardholderReceipt = EventArgs.TransactionResponse.PrintData.CardholderReceipt;
            Helper.SaveReceiptsData(tranId, "MerchantReceipt", Global.merchantReceipt);
            Helper.SaveReceiptsData(tranId, "CardholderReceipt", Global.cardholderReceipt);
            TerminalListenersClear();
            System.Windows.Application.Current.MainWindow.Content = new ThankYouCard(_ob);
           
        }
        private void terminal_statusChanged(object sender, TerminalStatus e)
        {
            _transactionStatus = e.TransactionStatus;
            _cardReaderStatus = e.CardReaderStatus;
            _connectionStatus = e.ConnectionStatus;
            if(!_isTransactionError)
            {
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
                        TerminalListenersClear();
                        System.Windows.Application.Current.MainWindow.Content = new TicketScanPage();
                    }
                }
            }
        }
        private void TerminalListenersClear()
        {
            Global.terminalCommands.TransactionError -= terminal_transactionError;
            Global.terminalCommands.StatusChanged -= terminal_statusChanged;
            Global.terminalCommands.TransactionCompleted -= terminal_transactionCompleted;
            Global.terminalCommands.ActivateCompleted -= terminal_activateCompleted;
            Global.terminalCommands.DeactivateCompleted -= terminal_deactivateCompleted;


        }
    }
}
