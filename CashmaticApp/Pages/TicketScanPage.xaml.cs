﻿using System;
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
            //en.IsChecked = true;
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

                //TODO: send has to server 
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

        private void Language_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            MutualyExclusiveCheckboxes(cb.Name);
        }
        private void MutualyExclusiveCheckboxes(string cbName)
        {
            if(!en.Name.Equals(cbName))
            {
                en.IsChecked = false;
            }
            if (!de.Name.Equals(cbName))
            {
                de.IsChecked = false;
            }
            if (!slo.Name.Equals(cbName))
            {
                slo.IsChecked = false;
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
            Application.Current.MainWindow.Content = new PaymentSummaryPage();
        }
    }
}
