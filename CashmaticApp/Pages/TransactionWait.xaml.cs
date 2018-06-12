using System.Windows;
using System.Windows.Controls;

namespace CashmaticApp.Pages
{
    /// <summary>
    /// Interaction logic for TransactionWait.xaml
    /// </summary>
    public partial class TransactionWait : Page
    {
        public TransactionWait()
        {
            InitializeComponent();
            Application.Current.MainWindow.Content = new ThankYouCash();
        }
    }
}
