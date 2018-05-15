using CashmaticApp.Controls;
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
using System.Windows.Threading;

namespace CashmaticApp.Pages
{
    /// <summary>
    /// Interaction logic for TicketScanPage.xaml
    /// </summary>
    public partial class TicketScanPage : Page
    {
        private string _barCode = string.Empty;
        static int VALIDATION_DELAY = 350;
        System.Threading.Timer timer = null;
        public TicketScanPage()
        {
            InitializeComponent();
            en.IsSelected = true;
        }

        private void tbBarCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox origin = sender as TextBox;
            if (!origin.IsFocused)
            {
                return;
            }
            DisposeTimer();
            timer = new System.Threading.Timer(TimerElapsed, null, VALIDATION_DELAY, VALIDATION_DELAY);

        }

        private void TimerElapsed(Object obj)
        {
            CheckSyntaxAndReport();
            DisposeTimer();
        }

        private void DisposeTimer()
        {
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }
        }

        private void CheckSyntaxAndReport()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                _barCode = tbBarCode.Text.Trim();
            }
            ));
            if(!string.IsNullOrEmpty(_barCode) && _barCode.Length==36)
            {
                DisposeTimer();

                var json = "{ 	'payment': {	'paymentSummary': {	'checkin': '3.2.2018 14:30 ',	'checkout': '8.2.2018 01:16',	'basePrice': '1',	'vat1': '2',	'vat2': '3',	'total': '300'	},	'item': [{	'name': 'item1',	'qty': '3',	'price': '5'	},	{	'name': 'item2',	'qty': '4',	'price': '2'	},	{	'name': 'item3',	'qty': '2',	'price': '4'	}] 	} }";

                RootObject ob = Helper.JSONToObject<RootObject>(json);
                if (ob != null)
                {

                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(() => Application.Current.MainWindow.Content = new PaymentSummaryPage(ob)));
                }

                //TODO: send hash to server 
                //TODO: go to next page

            }
        }

        private void tbBarCode_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox origin = sender as TextBox;
            string text = origin.Text;
            if(!string.IsNullOrEmpty(text))
            {
                if (char.IsWhiteSpace(text[0]))
                {
                    var b = 9;
                }
            }
          
        }

        private void MutualyExclusiveCheckboxes(string cbName)
        {
            if(!en.Name.Equals(cbName))
            {
                en.IsSelected = false;
            }
            if (!de.Name.Equals(cbName))
            {
                de.IsSelected = false;
            }
            if (!slo.Name.Equals(cbName))
            {
                slo.IsSelected = false;
            }
            if (!hu.Name.Equals(cbName))
            {
                hu.IsSelected = false;
            }
            if (!cz.Name.Equals(cbName))
            {
                cz.IsSelected = false;
            }
            if (!sk.Name.Equals(cbName))
            {
                sk.IsSelected = false;
            }
    
            
        }

        private void Control_Click(object sender, EventArgs e)
        {
            CheckBoxImage cb = sender as CheckBoxImage;
            MutualyExclusiveCheckboxes(cb.Name);
        }

      
    }
}
