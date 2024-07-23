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

                        //ProcessNetsNetwork(ApplicationStateContext.PaymentMethodObject.PaymentMode, ApplicationStateContext.NetAmountToPay);
                        PaymentHelper.ProcessNetsNetwork(ApplicationStateContext.PaymentMethodObject.PaymentMode, ApplicationStateContext.NetAmountToPay);

                        if (PaymentHelper.mbTransactionSuccess)
                        {
                            DoTransaction();

                            if (!string.IsNullOrEmpty(transactionNo))
                                PrintHelper.Print();
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
                catch(Exception ex)
                {
                   
                }
            };

            this.Unloaded += (sender, e) =>
            {
                timer?.Stop();
                timer = null;
            };
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
