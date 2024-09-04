using HashGo.Domain.Models.Base;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace HashGo.Wpf.App.BestTech.Controls
{
    /// <summary>
    /// Interaction logic for VirtualKeyboard.xaml
    /// </summary>
    public partial class VirtualKeyboard : Window
    {
        SharedDataService sharedDataService;
        int oskProcessId = -1;
        public VirtualKeyboard(SharedDataService sharedDataService)
        {
            InitializeComponent();
            this.sharedDataService = sharedDataService;
            tBoxInput.Focus();

            this.Loaded += (sender, e) =>
            {
                tBoxInput.Text = null;

                if(!string.IsNullOrEmpty(sharedDataService.RefferalCode))
                {
                    tBoxInput.Text = sharedDataService.RefferalCode;
                    tBoxInput.SelectionStart = sharedDataService.RefferalCode.Length;
                }

            };

            this.MinHeight = SystemParameters.PrimaryScreenHeight/4;
            this.Width = SystemParameters.PrimaryScreenWidth - 10;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                sharedDataService.RefferalCode = tBoxInput.Text;
                this.DialogResult = true;
                return;
            }

            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                 (e.Key >= Key.A && e.Key <= Key.Z) || e.Key == Key.CapsLock ||
                 e.Key == Key.Delete ||
                 e.Key == Key.Back))
            {
                e.Handled = true;
                return;
            }

        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(VirtualKeyboard), new PropertyMetadata(null));



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                string key = btn.Content.ToString();
                SendKey(key);
            }
        }

        void SendKey(string key)
        {
            if (key == "Space Bar" || key == "Space")
            {
                key = " ";
            }
            else if (key == "Delete")
            {
                tBoxInput.Text = "";
                tBoxInput.Focus();
                tBoxInput.SelectionStart = 0;
                return;
            }
            else if (key == "Backspace")
            {
                tBoxInput.Text = tBoxInput.Text.Substring(0, tBoxInput.Text.Length - 1);
                tBoxInput.Focus();
                tBoxInput.SelectionStart = tBoxInput.Text.Length;
                return;
            }

            int selectionStart = tBoxInput.SelectionStart;
            tBoxInput.Text = tBoxInput.Text.Insert(selectionStart, key);
            tBoxInput.SelectionStart = selectionStart + key.Length;
            Text = tBoxInput.Text;
        }

        private void Border_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            sharedDataService.RefferalCode = tBoxInput.Text;
            //this.Close();
            this.DialogResult = true;
        }

        private void Path_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void tBoxInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

            }
        }
    }
}
