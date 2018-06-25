using CashmaticApp.Controls;
using hgi.Environment;
using RestSharp;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace CashmaticApp.Pages
{
    /// <summary>
    /// Interaction logic for TicketScanPage.xaml
    /// </summary>
    public partial class TicketScanPage : Page
    {
        private string _barCode = string.Empty;
        private string _language = null;

        public TicketScanPage()
        {
            Debug.Log("CashmaticApp", "Initializing ticket scan page");
            InitializeComponent();
            en.IsSelected = true;
            _language = en.Name;
            tbBarCode.Focus();
            Keyboard.Focus(tbBarCode);
            _barCode = "";

        }
        private void CheckSyntaxAndReport()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                _barCode = tbBarCode.Text.Trim();
            }
            ));
            if (!string.IsNullOrEmpty(_barCode) && _barCode.Length == 36)
            {
                Debug.Log("CashmaticApp", "CheckSyntaxAndReport");
              
                RootObject ob = TransactionLogic.RequestParkingDetails(_barCode);
                if (ob != null && !ob.isError)
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(() => Application.Current.MainWindow.Content = new PaymentSummaryPage(ob)));
                }
                else
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                          new Action(() => Application.Current.MainWindow.Content = new TicketScanPage()));
                }

            }
            _barCode = "";
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
            if (!si.Name.Equals(cbName))
            {
                si.IsSelected = false;
                
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
            cb.IsSelected = true;
            _language = cb.Name;
            MutualyExclusiveCheckboxes(cb.Name);
         
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CheckSyntaxAndReport();
            }
            base.OnKeyDown(e);
        }

    }
}
