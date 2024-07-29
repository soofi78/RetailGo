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
        private readonly ILoggingService _logger;
        public ProcessingPaymentPage(INavigationService navigationService, IRetailConnectService retailConnectService, ILoggingService logger)
        {
            InitializeComponent();

            this.navigationService = navigationService;
            this.retailConnectService = retailConnectService;
            _logger = logger;
            this.Loaded += (sender, e) =>
            {
                //performOperation();

                try
                {
                    if (ApplicationStateContext.PaymentMethodObject != null && ApplicationStateContext.SalesOrderRequestObject != null)
                    {
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                        if (ApplicationStateContext.PaymentMethodObject.PaymentMode == "NETSQR")
                        {
                            string hostId = HashGoAppSettings.NETSQRHOSTID; // "37066801";   
                            string hostMId = HashGoAppSettings.NETSQRHOSTMID; //"11137066800";  
                            string invoiceRef = DateTime.Now.ToString("MMddHHmmss");
                            string gatewayToken = HashGoAppSettings.NETSQRGATEWAYTOKEN; //"gXKYoXJisXLE6krTTNebWqzWMnZ4UF9lgLGWMuvl";    
                            NetsQRHelper netsQR = new NetsQRHelper(_logger);

                            // if host id, host mid and gateway token is empty then dont proceed this payment 
                            if(string.IsNullOrEmpty(hostId) && string.IsNullOrEmpty(hostMId) && string.IsNullOrEmpty(gatewayToken))
                            {
                                // throw error
                                MessageBoxResult res = System.Windows.MessageBox.Show("Please make sure that QR Host ID, QR Host MID, QR Gateway Token, QR Timer values are configured");

                                if(res == MessageBoxResult.OK)
                                    navigationService.NavigateToAsync(Pages.PaymentMethod.ToString());

                                return;
                            }
                            else
                            {
                                PaymentResponseDto netsResponse = netsQR.ProcessPayment(hostId, hostMId, ApplicationStateContext.NetAmountToPay, invoiceRef, gatewayToken);

                                // TODO - set the timer in descending order once u convert the responst to QR
                                // timeout field in settings
                                // if it failed stay until the time is out
                                // when timer is running PaymentStatus (next line) should fire and wait for a response

                                if(netsResponse != null && !string.IsNullOrEmpty(netsResponse.NetsQrCode))
                                {
                                    ApplicationStateContext.NETQRImageBase64String = netsResponse.NetsQrCode;
                                    navigationService.NavigateToAsync(Pages.QRPayment.ToString());

                                    PaymentResponseDto netsStatus = netsQR.PaymentStatus(hostId, hostMId, netsResponse.NetQRPaymentResponse.data.InstitutionCode, netsResponse.NetQRPaymentResponse.data.TxnIdentifier, netsResponse.NetQRPaymentResponse.data.InvoiceRef, gatewayToken);
                                    if (netsStatus.IsSuccess)
                                    {
                                        performOperation();
                                    }

                                    return;
                                }
                                else 
                                    return;
                            } 
                        }
                        else
                        {
                            if(string.IsNullOrEmpty(HashGoAppSettings.NETSIP))
                            {
                                System.Windows.MessageBox.Show("Please make sure that NETSIP is configured");
                                return;
                            }
                            PaymentHelper.ProcessNetsNetwork(ApplicationStateContext.PaymentMethodObject.PaymentMode, ApplicationStateContext.NetAmountToPay);

                            if (PaymentHelper.mbTransactionSuccess)
                            {
                                performOperation();
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
                finally
                {
                    Mouse.OverrideCursor = null;
                }
            };

            this.Unloaded += (sender, e) =>
            {
                timer?.Stop();
                timer = null;
            };
        }

        void performOperation()
        {
            DoTransaction();

            if (!string.IsNullOrEmpty(transactionNo))
            {
                GetLocationDetails();
                GetSalesOrder();
                PrintHelper.Print();
            }
        }

        async void GetLocationDetails()
        {
            ApplicationStateContext.LocationDetailsObj = await retailConnectService.GetLocationDetails();
        }

        async void GetSalesOrder()
        {
            ApplicationStateContext.SalesOrderWrapperobj = await retailConnectService.GetSalesOrderForEdit();
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
