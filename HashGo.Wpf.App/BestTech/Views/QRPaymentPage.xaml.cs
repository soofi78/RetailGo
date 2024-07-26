using QRCoder.Xaml;
using QRCoder;
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
using HashGo.Wpf.App.BestTech.ViewModels;
using HashGo.Wpf.App.BestTech.Controls;
using HashGo.Core.Contracts.View;
using System.IO;
using HashGo.Infrastructure.DataContext;
using System.Windows.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using HashGo.Infrastructure;
using HashGo.Core.Enum;
using HashGo.Wpf.App.Services;

namespace HashGo.Wpf.App.BestTech.Views
{
    /// <summary>
    /// Interaction logic for QRPaymentPage.xaml
    /// </summary>
    public partial class QRPaymentPage : Page
    {
        private DispatcherTimer timer;
        int time = 120;

        public QRPaymentPage(QRPaymentPageViewModel qRPaymentPageViewModel, IPopupService popupService) //: base(popupService)
        {
            InitializeComponent();

            this.Loaded += (sender, e) =>
            {
                qrIamge.Source = Base64StringToBitmapImage(ApplicationStateContext.NETQRImageBase64String);
                int tmpTime  = Convert.ToInt32(HashGoAppSettings.NETSQRTIMER);

                if (tmpTime != 0)
                    time = tmpTime;
                timer = new DispatcherTimer()
                {
                    Interval = TimeSpan.FromSeconds(1),
                };

                timer.Tick += (sender, e) =>
                {
                    tBlockTimer.Text = time.ToString();
                    time--;
                    if (time == 0)
                        timer.Stop();
                };

                timer.Start();
            };
            this.Unloaded += (sender, e) =>
            {
                timer?.Stop();
                timer = null;
            };

            this.DataContext = qRPaymentPageViewModel;
        }

        public BitmapImage Base64StringToBitmapImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(imageBytes))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
    }
}
