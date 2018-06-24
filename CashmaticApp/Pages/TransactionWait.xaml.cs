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
        private RootObject _ob;

        public TransactionWait(RootObject ob)
        {
            Debug.Log("CashmaticApp", "Initialize transaction wait");

            _ob = ob;
            InitializeComponent();
            tbAmount.Text = string.Format("Paying amount of {0}", string.Format("{0:0.00}€", _ob.panda.total_price));
        }
    }
}
