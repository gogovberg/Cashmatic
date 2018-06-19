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
        static int VALIDATION_DELAY = 350;
        System.Threading.Timer timer = null;
        private string _language = null;

        public TicketScanPage()
        {
            Debug.Log("CashmaticApp", "Initializing ticket scan page");
            InitializeComponent();
            en.IsSelected = true;
            _language = en.Name;
            tbBarCode.Focus();
            Keyboard.Focus(tbBarCode);
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
                Debug.Log("CashmaticApp", "CheckSyntaxAndReport");
                DisposeTimer();
                RootObject ob = TransactionLogic.RequestParkingDetails(_barCode);
                if (ob != null && !ob.isError)
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(() => Application.Current.MainWindow.Content = new PaymentSummaryPage(ob)));
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

        
    }
}
