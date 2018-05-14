using System;
using System.Collections.Generic;
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

namespace CashmaticApp.Pages
{
    /// <summary>
    /// Interaction logic for RefundingProcessxaml.xaml
    /// </summary>
    public partial class RefundingProces : Page
    {
  
        public RefundingProces()
        {
            InitializeComponent();
            CashmaticCommands.WriteAnnulla();
            Application.Current.MainWindow.Content = new ThankYouCash();
        }

    }
}
