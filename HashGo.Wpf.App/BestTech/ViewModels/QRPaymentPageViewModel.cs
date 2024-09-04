using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
using HashGo.Domain.ViewModels.Base;
using HashGo.Infrastructure;
using HashGo.Infrastructure.DataContext;
using HashGo.Wpf.App.Helpers;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace HashGo.Wpf.App.BestTech.ViewModels
{
    public class QRPaymentPageViewModel :BaseViewModel
    {
        INavigationService navigationService;
        DispatcherTimer timer = null;
        TimeSpan time = TimeSpan.FromMinutes(2);
        NetsQRHelper netsQR;
        IPaymentService paymentService;
        public static readonly object lockobj = new object();

        public QRPaymentPageViewModel(INavigationService navigationService, 
                                      ILoggingService logger,
                                      IPaymentService paymentService)
        {
            this.navigationService = navigationService;
            netsQR = new NetsQRHelper(logger);
            this.paymentService  = paymentService;

            NavigateToPreviousScreenCommand = new RelayCommand(OnNavigateToPreviousScreen);
            ViewLoadedCommand = new RelayCommand(OnViewLoaded);
            ViewUnLoadedCommand = new RelayCommand(OnViewUnloaded);
        }

        private void OnViewUnloaded()
        {
            timer.Stop();
            timer = null;
        }

        private void OnViewLoaded()
        {
            int tmpTime = Convert.ToInt32(HashGoAppSettings.NETSQRTIMER);

            if (tmpTime != 0)
                time = TimeSpan.FromMinutes(tmpTime);

            timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(2)
            };

            timer.Tick += (sender, e) =>
            {
                TimerText = time.ToString(@"m\:ss");
                time = time.Add(TimeSpan.FromSeconds(-1));
                if (time == TimeSpan.FromSeconds(0))
                {
                    timer.Stop();
                    //Call reverse api
                    PaymentResponseDto reverseStatus = netsQR.ReverseTransaction(HashGoAppSettings.NETSQRHOSTID, HashGoAppSettings.NETSQRHOSTMID, ApplicationStateContext.NETQRStanId,
                                                                    ApplicationStateContext.Deposit.Value, NetsResponse.NetQRPaymentResponse.data.InvoiceRef,
                                                                    NetsResponse.NetQRPaymentResponse.data.TxnIdentifier,
                                                                    HashGoAppSettings.NETSQRGATEWAYTOKEN);
                    navigationService.NavigateToAsync(Pages.PaymentMethod.ToString());
                    return;
                }
                checkPaymentStatusCallBack();
            };

            timer.Start();
        }

        void checkPaymentStatusCallBack()
        {
            lock(lockobj)
            {
                PaymentResponseDto netsStatus = netsQR.PaymentStatus(HashGoAppSettings.NETSQRHOSTID,
                                                                HashGoAppSettings.NETSQRHOSTMID,        
                                                                ApplicationStateContext.NETQRStanId,
                                                                ApplicationStateContext.Deposit.Value,
                                                                NetsResponse.NetQRPaymentResponse.data.InstitutionCode,
                                                                 NetsResponse.NetQRPaymentResponse.data.TxnIdentifier, NetsResponse.NetQRPaymentResponse.data.InvoiceRef,
                                                                 HashGoAppSettings.NETSQRGATEWAYTOKEN);

                if (netsStatus.IsSuccess)   //then break the timer and show success message.
                {
                    paymentService.PerformPayment();
                    timer.Stop();
                    navigationService.NavigateToAsync(Pages.PurchaseSucceded.ToString());
                    return;
                }
            }
        }

        private void OnNavigateToPreviousScreen()
        {
            //Call reverse api
            PaymentResponseDto reverseStatus = netsQR.ReverseTransaction(HashGoAppSettings.NETSQRHOSTID, HashGoAppSettings.NETSQRHOSTMID, ApplicationStateContext.NETQRStanId,
                                                            ApplicationStateContext.Deposit.Value, NetsResponse.NetQRPaymentResponse.data.InvoiceRef,
                                                            NetsResponse.NetQRPaymentResponse.data.TxnIdentifier,
                                                            HashGoAppSettings.NETSQRGATEWAYTOKEN);
            navigationService.NavigateToAsync(Pages.PaymentMethod.ToString());
        }

        #region Command

        public ICommand NavigateToPreviousScreenCommand { get; private set; }
        public ICommand ViewLoadedCommand { get; private set; }
        public ICommand ViewUnLoadedCommand { get; private set; }

        #endregion

        public override void ViewUnloaded()
        {
            timer.Stop();
            timer = null;
        }

        #region Properties

        public PaymentResponseDto NetsResponse { get; set; }
        

        string timerText;
        public string TimerText 
        {
            get => timerText;
            set
            {
                timerText = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
