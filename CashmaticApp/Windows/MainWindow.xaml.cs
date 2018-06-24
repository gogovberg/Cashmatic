using CashmaticApp.Pages;
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

namespace CashmaticApp.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            string pathTodirectoryBills = "/Bills";
            //string pathTodirectoryDebugLog = "/DebugLog";

            System.IO.Directory.CreateDirectory(pathTodirectoryBills);
            //System.IO.Directory.CreateDirectory(pathTodirectoryDebugLog);

            Debug.Log("CashmaticApp", "Initializing main window");
            InitializeComponent();
          
            this.Content = new TicketScanPage();
        }
    }
}
