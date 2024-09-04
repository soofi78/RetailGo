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
using HashGo.Core.Contracts.Views;
using HashGo.Wpf.App.Helpers;
using Microsoft.Extensions.Hosting;
using HashGo.Core.Contracts.Services;

namespace HashGo.Wpf.App.BestTech.Views
{
    /// <summary>
    /// Interaction logic for QRPaymentPage.xaml
    /// </summary>
    public partial class QRPaymentPage : Page
    {
        private DispatcherTimer timer;
        INavigationService navigationService;
        private TimeSpan time = TimeSpan.FromMinutes(2);
        NetsQRHelper netsQR;

        public QRPaymentPage(QRPaymentPageViewModel qRPaymentPageViewModel, 
                            INavigationService navigationService,
                            IPopupService popupService, 
                            ILoggingService logger) //: base(popupService)
        {
            InitializeComponent();

            this.navigationService = navigationService;
            netsQR = new NetsQRHelper(logger);

            this.Loaded += (sender, e) =>
            {
                qrIamge.Source = Base64StringToBitmapImage(ApplicationStateContext.NETQRImageBase64String); 
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

        #region Properties

        

        #endregion
    }
}
