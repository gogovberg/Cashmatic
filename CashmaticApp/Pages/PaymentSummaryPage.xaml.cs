﻿using hgi.Environment;
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
        private App _currentApp = ((App)Application.Current);
        public PaymentSummaryPage(RootObject ob)
        {
            Debug.Log("CashmaticApp", "Initializing payment summary page");
            _ob = ob;
            InitializeComponent();
            try
            {

                Global.cardholderReceipt = "";
                Global.merchantReceipt = "";

                dtSummary.ItemsSource = LoadSummaryItems(_ob.ready2order.items);
                dtSummary.AutoGenerateColumns = false;

                tblTotalValue.Text = string.Format("{0:0.00}€", _ob.panda.total_price);
                tblBasePriceValue.Text = string.Format("{0:0.00}€", _ob.panda.base_price);
                tblVatOne.Text = string.Format("VAT({0}%)", _ob.panda.vat_rate_low);
                tblVatOneValue.Text = string.Format("{0:0.00}€", _ob.panda.vat_rate_low_value);
                tblVatTwo.Text = string.Format("VAT({0}%)", _ob.panda.vat_rate_high);
                tblVatTwoValue.Text = string.Format("{0:0.00}€", _ob.panda.vat_rate_high_value);

                tblDateTimeInValue.Text = _ob.panda.checkindate;
                tblDateTimeOutValue.Text = _ob.panda.checkoutdate;

                SetLanguageCheckbox(_ob.panda.language);

                Global.subtotale = (int)(ob.panda.total_price * 100);

                if(!Global.isCardPayment)
                {
                    btnPayCard.Visibility = Visibility.Collapsed;
                }
                if (!Global.isCashPayment)
                {
                    btnPayCash.Visibility = Visibility.Collapsed;
                }

            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Debug.Log("CashmaticApp", "Button back click");
            //Global.terminalCommands.onDeactivate();
            Application.Current.MainWindow.Content = new TicketScanPage();
        }

        private void btnPayCash_Click(object sender, RoutedEventArgs e)
        {
            Debug.Log("CashmaticApp", "Button pay cash click");
            if(CashmaticCommands.IsConnected())
            {
                _ob.ready2order.paymentMethod_id = _ob.panda.paymentMethodCASH;
                Application.Current.MainWindow.Content = new PaymentPandingCash(_ob);
            }
            else
            {
                Application.Current.MainWindow.Content = new PayingProblemCash(_ob);
            }
        
        }

        private void btnPayCard_Click(object sender, RoutedEventArgs e)
        {
            Debug.Log("CashmaticApp", "Button pay card click");

            _ob.ready2order.paymentMethod_id = _ob.panda.paymentMethodCARD;
            Application.Current.MainWindow.Content = new PaymentPandingCard(_ob);
        }

        private void Language_checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            SetLanguageCheckbox(cb.Name);
            MutualyExclusiveCheckboxes(cb.Name);
            _ob.panda.language = cb.Name;
            ChangeLanguageDictionary(cb.Name);
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
            catch (Exception ex)
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
        private void ChangeLanguageDictionary(string cbName)
        {
            ResourceDictionary dict = new ResourceDictionary();

            switch (cbName)
            {
                case "en":
                    dict.Source = new Uri("..\\Languages\\English.xaml", UriKind.Relative);
                    break;
                case "de":
                    dict.Source = new Uri("..\\Languages\\German.xaml", UriKind.Relative);
                    break;
                case "si":
                    dict.Source = new Uri("..\\Languages\\Slovene.xaml", UriKind.Relative);
                    break;
                case "hu":
                    dict.Source = new Uri("..\\Languages\\Hungary.xaml", UriKind.Relative);
                    break;
                case "cz":
                    dict.Source = new Uri("..\\Languages\\Czech.xaml", UriKind.Relative);
                    break;
                case "sk":
                    dict.Source = new Uri("..\\Languages\\Slovak.xaml", UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("..\\Languages\\English.xaml", UriKind.Relative);
                    break;
            }

            int index = -1;

            for (int i = 0; i < _currentApp.Resources.MergedDictionaries.Count; i++)
            {
                if (_currentApp.Resources.MergedDictionaries[i].Source.OriginalString.Contains("Language"))
                {
                    index = i;
                }
            }

            if (index > 0)
            {
                _currentApp.Resources.MergedDictionaries.RemoveAt(index);
            }

            _currentApp.Resources.MergedDictionaries.Add(dict);

        }
        private List<SummaryItem> LoadSummaryItems(List<Item> itm)
        {
            List<SummaryItem> items = new List<SummaryItem>();
            int i = 0;
            foreach (var item in itm)
            {
                items.Add(new SummaryItem() { ItemID = i, ItemName = item.item_name, ItemPrice = string.Format("{0}€", item.item_price), ItemQty = item.item_quantity, ItemTotal = string.Format("{0}€", item.item_price * item.item_quantity) });
                i++;
            }

            return items;
        }
    }
}
