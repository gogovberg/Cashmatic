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
        public RefoundPending()
        {
            InitializeComponent();
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new PaymentPandingCash();
        }

        private void btnRefund_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
