﻿using hgi.Environment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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
    /// Interaction logic for RefundingProcessxaml.xaml
    /// </summary>
    public partial class RefundingProces : Page
    {
        private FileSystemWatcher _fileWatcher = null;
        private RootObject _ob;

        public RefundingProces(RootObject ob, bool CannotDispense)
        {
            InitializeComponent();
            _ob = ob;
            if(!CannotDispense)
            {
                _fileWatcher = new FileSystemWatcher();
                _fileWatcher.Path = Global.base_path;
                _fileWatcher.Filter = "annualla.txt";
                _fileWatcher.Deleted += new FileSystemEventHandler(OnDeletedAnnuala);
                _fileWatcher.EnableRaisingEvents = true;
                CashmaticCommands.WriteAnnulla();
            }
            else
            {

                _fileWatcher = new FileSystemWatcher();
                _fileWatcher.Path = Global.base_path;
                _fileWatcher.Filter = "erogato.txt";
                _fileWatcher.Created += new FileSystemEventHandler(OnCreatedErogato);
                _fileWatcher.EnableRaisingEvents = true;
                
                CashmaticCommands.WriteSubtotale(Global.pagato*(-1));
            }
        }
        private void OnDeletedAnnuala(object source, FileSystemEventArgs e)
        {

            FileInfo file = new FileInfo(e.FullPath);
            while (Helper.isFileLocked(file))
            {
                Thread.Sleep(50);
            }
            try
            {
                Application.Current.Dispatcher.BeginInvoke(
                     DispatcherPriority.Background, new Action(() => Application.Current.MainWindow.Content = new TicketScanPage()));
            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }
        }
        private void OnCreatedErogato(object source, FileSystemEventArgs e)
        {
            FileInfo file = new FileInfo(e.FullPath);
            while (Helper.isFileLocked(file))
            {
                Thread.Sleep(50);
            }
            try
            {
                int erogato = CashmaticCommands.ReadErogato();
                if (erogato==Global.pagato)
                {
                    file.Delete();
                    Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background, new Action(() => Application.Current.MainWindow.Content = new TicketScanPage()));
                }
            }
            catch (Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }

        }
    }
}
