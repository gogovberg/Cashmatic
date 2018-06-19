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
    /// Interaction logic for PaymentPandingCard.xaml
    /// </summary>
    public partial class PaymentPandingCard : Page
    {
        private RootObject _ob;
        public PaymentPandingCard(RootObject ob)
        {
            Debug.Log("CashmaticApp", "Initializing payment paning card");
            InitializeComponent();
            _ob = ob;
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Debug.Log("CashmaticApp", "Button cancel click");
            Application.Current.MainWindow.Content = new RefoundPending(_ob);
        }
    }
}
