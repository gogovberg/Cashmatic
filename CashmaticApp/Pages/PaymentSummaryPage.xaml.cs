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
    /// Interaction logic for PaymentSummaryPage.xaml
    /// </summary>
    public partial class PaymentSummaryPage : Page
    {
        RootObject _ob;
        public PaymentSummaryPage(RootObject ob)
        {
            _ob = ob;
            InitializeComponent();
            dtSummary.ItemsSource = LoadSummaryItems(_ob.payment.item);
            dtSummary.AutoGenerateColumns = false;
            tblTotalValue.Text = string.Format("{0:0.00}€", ob.payment.paymentSummary.total / (double)100);
            tblBasePriceValue.Text = string.Format("{0:0.00}€", ob.payment.paymentSummary.basePrice / (double)100);
            tblVatOneValue.Text = string.Format("{0:0.00}€", ob.payment.paymentSummary.vat1 / (double)100);
            tblVatTwoValue.Text = string.Format("{0:0.00}€", ob.payment.paymentSummary.vat2 / (double)100);
            tblDateTimeInValue.Text = ob.payment.paymentSummary.checkin;
            tblDateTimeOutValue.Text = ob.payment.paymentSummary.checkout;
        }
        private List<SummaryItem> LoadSummaryItems(List<Item> itm)
        {
            List<SummaryItem> items = new List<SummaryItem>();
            int i = 0;
            foreach(var item in itm)
            {
                items.Add(new SummaryItem() { ItemID = i, ItemName = item.name, ItemPrice = item.price, ItemQty = item.qty, ItemTotal = item.total });
                i++;
            }

            return items;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new TicketScanPage();
        }

        private void btnPayCash_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new PaymentPandingCash(_ob);
        }

        private void btnPayCard_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new PaymentPandingCard(_ob);
        }
    }
}
