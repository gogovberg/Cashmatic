﻿using hgi.Environment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for PaymentPanding.xaml
    /// </summary>
    public partial class PaymentPandingCash : Page
    {
        private RootObject _ob;

        private FileSystemWatcher _saldatoWatcher = null;
        private FileSystemWatcher _pagatoWatcher = null;
        private FileSystemWatcher _erogatoWatcher = null;

        public PaymentPandingCash(RootObject ob)
        {
            Debug.Log("CashmaticApp", "Initializing payment panding cash");
            InitializeComponent();
            _ob = ob;
            InitPayment();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Debug.Log("CashmaticApp", "CancelPayment");
            Application.Current.MainWindow.Content = new RefoundPending(_ob);
        }

        private void InitPayment()
        {
            Debug.Log("CashmaticApp", "InitPayment");
            int amountLeft = Global.subtotale;

            if (_ob.panda.OnPayment)
            {
                int saldato = CashmaticCommands.ReadSaldato();
                amountLeft = Global.subtotale - saldato;
            }
            else
            {
                CashmaticCommands.WriteSubtotale(Global.subtotale);
                _ob.panda.OnPayment = true;
            }

            tblPrice.Text = String.Format("{0:0.00}€", amountLeft / (double)100);

            SetSaldatoChangeListener();
            SetPagatoChangeListener();
            SetErogatoChangeListener();
            SetNonerogatoChangeListener();
        }

        private void OnChangedDir(object source, FileSystemEventArgs e)
        {
            Debug.Log("CashmaticApp", "OnChangedDir");
            if (File.Exists(Global.base_path + "saldato.txt") && _saldatoWatcher == null)
            {
                SetSaldatoChangeListener();
            }
        }

        private void OnChangeSaldato(object source, FileSystemEventArgs e)
        {
            Debug.Log("CashmaticApp", "OnChangeSaldato");
            FileInfo file = new FileInfo(e.FullPath);
            while (Helper.isFileLocked(file))
            {
                Thread.Sleep(50);
            }
            try
            {
                int saldato = CashmaticCommands.ReadSaldato();
                Global.pagato = saldato;
                saldato = Global.subtotale - saldato;

                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background, new Action(() => tblPrice.Text = String.Format("{0:0.00}€", saldato / (double)100)));
            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }

        }

        private void OnCreatedPagato(object source, FileSystemEventArgs e)
        {
            Debug.Log("CashmaticApp", "OnCreatedPagato");
            FileInfo file = new FileInfo(e.FullPath);

            Thread.Sleep(3000);
            while (Helper.isFileLocked(file))
            {
                Thread.Sleep(50);
            }
            try
            {
                int pagato = CashmaticCommands.ReadPagato();
                int saldato = CashmaticCommands.ReadPagato();
                int erogato = CashmaticCommands.ReadErogato();
                int nonerogat = CashmaticCommands.ReadNonerogato();
                Global.pagato = pagato - erogato;

                if (pagato >= Global.subtotale)
                {
                    file.Delete();
                    Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background, new Action(() => Application.Current.MainWindow.Content = new ThankYouCash(_ob)));
                }
            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }


        }

        private void OnCreatedErogato(object source, FileSystemEventArgs e)
        {
            Debug.Log("CashmaticApp", "OnCreatedErogato");
            FileInfo file = new FileInfo(e.FullPath);
            while (Helper.isFileLocked(file))
            {
                Thread.Sleep(50);
            }
            try
            {

                int erogato = CashmaticCommands.ReadErogato();
                int resto = Global.pagato - Global.subtotale;

                if ((resto - erogato) > 0)
                {
                    file.Delete();
                    Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background, new Action(() => Application.Current.MainWindow.Content = new RefundingProces(_ob, true)));
                }
            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }

        }
        private void OnCreatedNonerogato(object source, FileSystemEventArgs e)
        {
            Debug.Log("CashmaticApp", "OnCreatedNonerogato");
            FileInfo file = new FileInfo(e.FullPath);
            while (Helper.isFileLocked(file))
            {
                Thread.Sleep(50);
            }
            try
            {

                int erogato = CashmaticCommands.ReadErogato();
                int resto = Global.pagato - Global.subtotale;

                if ((resto - erogato) > 0)
                {
                    file.Delete();
                    Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background, new Action(() => Application.Current.MainWindow.Content = new RefundingProces(_ob, true)));
                }
            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }

        }

        private void SetSaldatoChangeListener()
        {
            Debug.Log("CashmaticApp", "SetSaldatoChangeListener");
            _saldatoWatcher = new FileSystemWatcher();
            _saldatoWatcher.Path = Global.base_path;
            _saldatoWatcher.Filter = "saldato.txt";
            _saldatoWatcher.Changed += new FileSystemEventHandler(OnChangeSaldato);
            _saldatoWatcher.EnableRaisingEvents = true;
        }

        private void SetPagatoChangeListener()
        {
            Debug.Log("CashmaticApp", "SetPagatoChangeListener");
            _pagatoWatcher = new FileSystemWatcher();
            _pagatoWatcher.Path = Global.base_path;
            _pagatoWatcher.Filter = "pagato.txt";
            _pagatoWatcher.Created += new FileSystemEventHandler(OnCreatedPagato);
            _pagatoWatcher.EnableRaisingEvents = true;
        }

        private void SetErogatoChangeListener()
        {
            Debug.Log("CashmaticApp", "SetErogatoChangeListener");
            _erogatoWatcher = new FileSystemWatcher();
            _erogatoWatcher.Path = Global.base_path;
            _erogatoWatcher.Filter = "erogato.txt";
            _erogatoWatcher.Created += new FileSystemEventHandler(OnCreatedErogato);
            _erogatoWatcher.EnableRaisingEvents = true;
        }
        private void SetNonerogatoChangeListener()
        {
            Debug.Log("CashmaticApp", "SetNonerogatoChangeListener");
            _erogatoWatcher = new FileSystemWatcher();
            _erogatoWatcher.Path = Global.base_path;
            _erogatoWatcher.Filter = "nonerogato.txt";
            _erogatoWatcher.Created += new FileSystemEventHandler(OnCreatedNonerogato);
            _erogatoWatcher.EnableRaisingEvents = true;
        }
    }
}
