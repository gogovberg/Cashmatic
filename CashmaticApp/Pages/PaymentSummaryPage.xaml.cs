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
    /// Interaction logic for PaymentSummaryPage.xaml
    /// </summary>
    public partial class PaymentSummaryPage : Page
    {
        RootObject _ob;

        public PaymentSummaryPage(RootObject ob)
        {
            _ob = ob;
            InitializeComponent();
            try
            {
                dtSummary.ItemsSource = LoadSummaryItems(_ob.ready2order.items);
                dtSummary.AutoGenerateColumns = false;
                tblTotalValue.Text = string.Format("{0:0.00}€", _ob.panda.total_price);
                tblBasePriceValue.Text = string.Format("{0:0.00}€", _ob.panda.base_price);
                tblVatOneValue.Text = string.Format("{0:0.00}€", _ob.panda.vat_rate_one);
                tblVatTwoValue.Text = string.Format("{0:0.00}€", _ob.panda.vat_rate_two);
                tblDateTimeInValue.Text = _ob.panda.checkindate;
                tblDateTimeOutValue.Text = _ob.panda.checkoutdate;

                SetLanguageCheckbox(_ob.panda.language);
            }
            catch(Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }
  
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

        private void Language_checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            SetLanguageCheckbox(cb.Name);
            MutualyExclusiveCheckboxes(cb.Name);
            _ob.panda.language = cb.Name;
        }

        private void SetLanguageCheckbox(string language)
        {
            try
            {
                language = string.IsNullOrEmpty(language) ? "en" : language;
                enImg.Source = new BitmapImage(new Uri("/Images/" + (language.Equals("en") ? "gb.png" : "gb_bw.png"), UriKind.Relative));
                deImg.Source = new BitmapImage(new Uri("/Images/" + (language.Equals("de") ? "de.png" : "de_bw.png"), UriKind.Relative));
                siImg.Source = new BitmapImage(new Uri("/Images/" + (language.Equals("si") ? "si.png" : "si_bw.png"), UriKind.Relative));
                huImg.Source = new BitmapImage(new Uri("/Images/" + (language.Equals("hu") ? "hu.png" : "hu_bw.png"), UriKind.Relative));
                czImg.Source = new BitmapImage(new Uri("/Images/" + (language.Equals("cz") ? "cz.png" : "cz_bw.png"), UriKind.Relative));
                skImg.Source = new BitmapImage(new Uri("/Images/" + (language.Equals("sk") ? "sk.png" : "sk_bw.png"), UriKind.Relative));
            }
            catch(Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }
       

        }

        private void MutualyExclusiveCheckboxes(string cbName)
        {
            if (!en.Name.Equals(cbName))
            {
                en.IsChecked = false;

            }
            if (!de.Name.Equals(cbName))
            {
                de.IsChecked = false;

            }
            if (!si.Name.Equals(cbName))
            {
                si.IsChecked = false;

            }
            if (!hu.Name.Equals(cbName))
            {
                hu.IsChecked = false;

            }
            if (!cz.Name.Equals(cbName))
            {
                cz.IsChecked = false;

            }
            if (!sk.Name.Equals(cbName))
            {
                sk.IsChecked = false;

            }
        }

        private List<SummaryItem> LoadSummaryItems(List<Item> itm)
        {
            List<SummaryItem> items = new List<SummaryItem>();
            int i = 0;
            foreach (var item in itm)
            {
                items.Add(new SummaryItem() { ItemID = i, ItemName = item.item_name, ItemPrice = string.Format("{0}€", item.item_price), ItemQty = item.item_quantity, ItemTotal = string.Format("{0}€", item.item_price*item.item_quantity) });
                i++;
            }

            return items;
        }
    }
}
