using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
using HashGo.Core.Models;
using HashGo.Core.Models.BestTech;
using HashGo.Domain.Helper;
using HashGo.Domain.Services;
using HashGo.Infrastructure;
using HashGo.Infrastructure.DataContext;
using HashGo.Wpf.App.Helpers;
using HashGo.Wpf.App.Models.BestTech;
using PrinterUtility;
using PrinterUtility.EscPosEpsonCommands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Windows.Networking.Connectivity;

namespace HashGo.Wpf.App.BestTech.Views
{
    /// <summary>
    /// Interaction logic for ProcessingPaymentPage.xaml
    /// </summary>
    public partial class ProcessingPaymentPage : Page
    {
        DispatcherTimer timer;
        INavigationService navigationService;
        IRetailConnectService retailConnectService;
        public ProcessingPaymentPage(INavigationService navigationService, IRetailConnectService retailConnectService)
        {
            InitializeComponent();

            this.navigationService = navigationService;
            this.retailConnectService = retailConnectService;

            this.Loaded += (sender, e) =>
            {

                try
                {
                    if (ApplicationStateContext.PaymentMethodObject != null && ApplicationStateContext.SalesOrderRequestObject != null)
                    { 
                        if (HashGoAppSettings.NETSIP == null || HashGoAppSettings.NETSIP.Length == 0)
                        {
                            string hostId = "37066801";  // move this to settings
                            string hostMId = "11137066800"; // move this to settings
                            string invoiceRef = DateTime.Now.ToString("MMddHHmmss");
                            string gatewayToken = "gXKYoXJisXLE6krTTNebWqzWMnZ4UF9lgLGWMuvl"; // move this to settings
                            NetsQRHelper netsQR = new NetsQRHelper();
                            PaymentResponseDto netsResponse = netsQR.ProcessPayment(hostId, hostMId, ApplicationStateContext.NetAmountToPay, invoiceRef, gatewayToken);

                            // TODO - set the timer in descending order once u convert the responst to QR
                            // timeout field in settings
                            // if it failed stay until the time is out
                            // when timer is running PaymentStatus (next line) should fire and wait for a response

                            PaymentResponseDto netsStatus = netsQR.PaymentStatus(hostId, hostMId, netsResponse.NetQRPaymentResponse.data.InstitutionCode,netsResponse.NetQRPaymentResponse.data.TxnIdentifier,netsResponse.NetQRPaymentResponse.data.InvoiceRef,
                                                                            gatewayToken);
                            if (netsStatus.IsSuccess)
                            {
                                DoTransaction();

                                if (!string.IsNullOrEmpty(transactionNo))
                                {
                                    GetLocationDetails();
                                    PrintHelper.Print();
                                }
                            } 
                        }
                        else
                        {
                            //ProcessNetsNetwork(ApplicationStateContext.PaymentMethodObject.PaymentMode, ApplicationStateContext.NetAmountToPay);
                            PaymentHelper.ProcessNetsNetwork(ApplicationStateContext.PaymentMethodObject.PaymentMode, ApplicationStateContext.NetAmountToPay);

                            if (PaymentHelper.mbTransactionSuccess)
                            {
                                DoTransaction();

                                if (!string.IsNullOrEmpty(transactionNo))
                                {
                                    GetLocationDetails();
                                    PrintHelper.Print();
                                }
                            }
                        }

                        ApplicationStateContext.TransactionNo = transactionNo;

                        timer = new DispatcherTimer()
                        {
                            Interval = TimeSpan.FromSeconds(4),
                        };

                        timer.Tick += (sender, e) =>
                        {
                            if (!string.IsNullOrEmpty(transactionNo) && PaymentHelper.mbTransactionSuccess)
                            {
                                navigationService.NavigateToAsync(Pages.PurchaseSucceded.ToString());
                            }
                            else
                            {
                                navigationService.NavigateToAsync(Pages.PurchaseFailed.ToString());
                            }
                        };

                        timer.Start();
                    }
                }
                catch (Exception ex)
                {

                }
            };

            this.Unloaded += (sender, e) =>
            {
                timer?.Stop();
                timer = null;
            };
        }

        async void GetLocationDetails()
        {
            ApplicationStateContext.LocationDetailsObj = await retailConnectService.GetLocationDetails();
        }

        #region Region Payment Transaction

        async void DoTransaction()
        {
            CreateTransaction(ApplicationStateContext.SalesOrderRequestObject);
        }

        private string transactionNo;

        private async Task CreateTransaction(SalesOrderRequest salesOrderRequest)
        {
            TransactionDetails transactionDetails = await retailConnectService.CreateSalesOrderWithPayment(salesOrderRequest);
          
            if (transactionDetails != null)
            {
                transactionNo = transactionDetails.transactionNo;
                ApplicationStateContext.TransactionId = transactionDetails.id;
            }
        }

        #endregion
    }
}
