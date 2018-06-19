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
    /// Interaction logic for PayingProblemCard.xaml
    /// </summary>
    public partial class PayingProblemCard : Page
    {
        RootObject _ob;
        public PayingProblemCard(RootObject ob)
        {
            Debug.Log("CashmaticApp", "Initializing paying problem card");
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
            Debug.Log("CashmaticApp", "Button retry click");
            Application.Current.MainWindow.Content = new RefoundPending(_ob);
        }
    }
}
