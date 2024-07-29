using HashGo.Core.Contracts.Services;
using HashGo.Core.Models.BestTech;
using HashGo.Core.Models;
using HashGo.Domain.Services;
using HashGo.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashGo.Wpf.App.Helpers;

namespace HashGo.Wpf.App.Services
{
    public class PaymentService : IPaymentService
    {
        IRetailConnectService retailConnectService;
        public PaymentService(IRetailConnectService retailConnectService)
        {
            this.retailConnectService = retailConnectService;
        }
        public async Task<bool> PerformPayment()
        {
            DoTransaction();

            if (!string.IsNullOrEmpty(ApplicationStateContext.TransactionNo))
            {
                GetLocationDetails();
                GetSalesOrder();
                PrintHelper.Print();

                return true;
            }

            return false;
        }

        async void DoTransaction()
        {
            CreateTransaction(ApplicationStateContext.SalesOrderRequestObject);
        }

        private async Task CreateTransaction(SalesOrderRequest salesOrderRequest)
        {
            TransactionDetails transactionDetails = await retailConnectService.CreateSalesOrderWithPayment(salesOrderRequest);

            if (transactionDetails != null)
            {
                ApplicationStateContext.TransactionNo = transactionDetails.transactionNo;
                ApplicationStateContext.TransactionId = transactionDetails.id;
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
    }
}
