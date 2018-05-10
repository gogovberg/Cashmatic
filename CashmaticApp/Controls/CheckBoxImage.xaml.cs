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

namespace CashmaticApp.Controls
{
    /// <summary>
    /// Interaction logic for CheckBoxImage.xaml
    /// </summary>
    public partial class CheckBoxImage : UserControl
    {
        private DateTime downTime;
        private object downSender;
        public event EventHandler Control_Click;
        public ImageSource FlagSource
        {
            get { return imgFlag.Source; }
            set { imgFlag.Source = value; }
        }
        public string LanguageBox
        {
            get { return tblLanguage.Text; }
            set { tblLanguage.Text = value; }
        }
        public bool IsSelected
        {
            get { return (bool)this.GetValue(IsSelectedProperty); }
            set { this.SetValue(IsSelectedProperty, value); }
        }
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.RegisterAttached("IsSelected", typeof(bool), typeof(CheckBoxImage), new PropertyMetadata(false));
        public CheckBoxImage()
        {
            InitializeComponent();
        }

        protected void EventBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.downSender = sender;
                this.downTime = DateTime.Now;
            }
        }

        protected void EventBorder_MouseUp(object sender, MouseButtonEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Released && sender == this.downSender)
            {
                TimeSpan timeSinceDown = DateTime.Now - this.downTime;
                if (timeSinceDown.TotalMilliseconds < 500)
                {

                    if (this.Control_Click != null)
                    {
                        this.Control_Click(this, e);
                    }

                }
            }



        }
    }
}
