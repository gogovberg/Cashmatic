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
    /// Interaction logic for PayingProblemCash.xaml
    /// </summary>
    public partial class PayingProblemCash : Page
    {
        RootObject _ob;
        public PayingProblemCash(RootObject ob)
        {
            Debug.Log("CashmaticApp", "Initializing paying problem cash");
            InitializeComponent();
            _ob = ob;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Debug.Log("CashmaticApp", "Button back click");
            Application.Current.MainWindow.Content = new PaymentSummaryPage(_ob);
        }

        private void btnRetry_Click(object sender, RoutedEventArgs e)
        {
            if(CashmaticCommands.IsConnected())
            {
                Debug.Log("CashmaticApp", "Button retry click");
                _ob.ready2order.paymentMethod_id = _ob.panda.paymentMethodCASH;
                Application.Current.MainWindow.Content = new PaymentPandingCash(_ob);
            }
        
        }
    }
}
