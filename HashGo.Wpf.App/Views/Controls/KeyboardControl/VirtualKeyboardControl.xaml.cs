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
using System.Windows.Shapes;

namespace HashGo.Wpf.App.Views.Controls.KeyboardControl
{
    /// <summary>
    /// Interaction logic for VirtualKeyboardControl.xaml
    /// </summary>
    public partial class VirtualKeyboardControl : Window
    {
        public VirtualKeyboardControl()
        {
            InitializeComponent();
            Height = Application.Current.MainWindow.Height / 4;
            Width = Application.Current.MainWindow.Width;
            Top = Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - Height;
            Left = Application.Current.MainWindow.Left;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
