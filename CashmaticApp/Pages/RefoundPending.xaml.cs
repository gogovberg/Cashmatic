using hgi.Environment;
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

namespace CashmaticApp.Pages
{
    /// <summary>
    /// Interaction logic for RefoundPending.xaml
    /// </summary>
    public partial class RefoundPending : Page
    {
        private RootObject _ob;
        public RefoundPending(RootObject ob)
        {
            Debug.Log("CashmaticApp", "Initializing refund pending");
            InitializeComponent();
            _ob = ob;
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            Debug.Log("CashmaticApp", "Button continue click");
            Application.Current.MainWindow.Content = new PaymentPandingCash(_ob);
        }

        private void btnRefund_Click(object sender, RoutedEventArgs e)
        {
            Debug.Log("CashmaticApp", "Button refund click");
            Application.Current.MainWindow.Content = new RefundingProces(_ob, false);
        }
    }
}
