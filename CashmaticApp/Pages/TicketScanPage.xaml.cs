using CashmaticApp.Controls;
using hgi.Environment;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private App _currentApp = ((App)Application.Current);
        public TicketScanPage()
        {
            Debug.Log("CashmaticApp", "Initializing ticket scan page");
            InitializeComponent();
            en.IsSelected = true;
            _language = en.Name;
            tbBarCode.Focus();
            Keyboard.Focus(tbBarCode);
            _barCode = "";
            Global.request_bill_id = "";

            ChangeLanguageDictionary(_language);

            imgScanLogo.Source = new Uri(@"Images/QRscan.mp4", UriKind.Relative);
            imgScanLogo.Play();

        }
        private void CheckSyntaxAndReport()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                _barCode = tbBarCode.Text.Trim();
            }
            ));
            if(!string.IsNullOrEmpty(_barCode) && _barCode.Length>=36)
            {
                _barCode = _barCode.Substring(_barCode.Length - 36, 36);

                Debug.Log("CashmaticApp", "CheckSyntaxAndReport");

                RootObject ob = TransactionLogic.RequestParkingDetails(_barCode);
                if (ob != null && !ob.isError)
                {
                    ob.panda.language = _language;
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(() => Application.Current.MainWindow.Content = new PaymentSummaryPage(ob)));
                }
            }
            else
            {
                Debug.Log("CashmaticApp", "incorrect hash"+_barCode);
            }
           
            this.Dispatcher.Invoke(new Action(() =>
            {
                tbBarCode.Text = "";
            }
              ));
            _barCode = "";
        }

        private void MutualyExclusiveCheckboxes(string cbName)
        {
           
            if (!en.Name.Equals(cbName))
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

            for(int i=0; i< _currentApp.Resources.MergedDictionaries.Count; i++)
            {
                if(_currentApp.Resources.MergedDictionaries[i].Source.OriginalString.Contains("Language"))
                {
                    index = i;
                }
            }
           
            if( index > 0 )
            {
                _currentApp.Resources.MergedDictionaries.RemoveAt(index);
            }

            _currentApp.Resources.MergedDictionaries.Add(dict);

        }

        private void Control_Click(object sender, EventArgs e)
        {
            CheckBoxImage cb = sender as CheckBoxImage;
            cb.IsSelected = true;
            _language = cb.Name;

            MutualyExclusiveCheckboxes(cb.Name);
            ChangeLanguageDictionary(cb.Name);


        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CheckSyntaxAndReport();
                
              
            }
            base.OnKeyDown(e);
        }

        private void imgScanLogo_MediaEnded(object sender, RoutedEventArgs e)
        {
            imgScanLogo.Position = TimeSpan.Zero;
            imgScanLogo.Play();
        }
    }
}
