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
        public PaymentSummaryPage()
        {
            InitializeComponent();
            dtSummary.ItemsSource = LoadSummaryItems();
            dtSummary.AutoGenerateColumns = false;
        }
        private List<SummaryItem> LoadSummaryItems()
        {
            List<SummaryItem> items = new List<SummaryItem>();

            items.Add(new SummaryItem() { ItemID = 1, ItemName = "Item one", ItemPrice = 10, ItemQty = 2, ItemTotal = 10 * 2 });
            items.Add(new SummaryItem() { ItemID = 2, ItemName = "Item two", ItemPrice = 5, ItemQty = 1, ItemTotal = 5 * 1 });
            items.Add(new SummaryItem() { ItemID = 3, ItemName = "Item three", ItemPrice = 2, ItemQty = 4, ItemTotal = 2 * 4 });
            items.Add(new SummaryItem() { ItemID = 4, ItemName = "Item four", ItemPrice = 9, ItemQty = 10, ItemTotal = 9 * 10 });
            items.Add(new SummaryItem() { ItemID = 5, ItemName = "Item Five", ItemPrice = 1, ItemQty = 6, ItemTotal = 1 * 6 });

            return items;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new TicketScanPage();
        }

        private void btnPayCash_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPayCard_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
