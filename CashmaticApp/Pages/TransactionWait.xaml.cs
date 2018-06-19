using hgi.Environment;
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
            Debug.Log("CashmaticApp", "Initialize transaction wait");
            InitializeComponent();
            Application.Current.MainWindow.Content = new ThankYouCash();
        }
    }
}
