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
        IPrintTemplateParserService printTemplateParserService;
        public PaymentService(IRetailConnectService retailConnectService, IPrintTemplateParserService printTemplateParserService)
        {
            this.retailConnectService = retailConnectService;
            this.printTemplateParserService = printTemplateParserService;
        }
        public async Task<bool> PerformPayment()
        {
            DoTransaction();

            if (!string.IsNullOrEmpty(ApplicationStateContext.TransactionNo))
            {
                GetLocationDetails();
                GetSalesOrder();
                GetPrintTemplateReceipt();

                if (!string.IsNullOrEmpty(ApplicationStateContext.Template))
                {
                    await printTemplateParserService.GetReceipt(ApplicationStateContext.Template);
                }
                else return false;

                //PrintHelper.Print();

                return true;
            }

            return false;
        }

        async void GetPrintTemplateReceipt()
        {
            var templateReceipt = await retailConnectService.GetTemplateReceiptResponse();

            if(templateReceipt != null) 
            {
                ApplicationStateContext.Template = templateReceipt.result.First().template;
            }
        }

        async void DoTransaction()
        {
            CreateTransaction(ApplicationStateContext.SalesOrderRequestObject);
        }

        private async Task CreateTransaction(SalesOrderRequest salesOrderRequest)
        {
            //salesOrderRequest.salesOrder.netTotal = salesOrderRequest.salesOrder.subTotal;
            salesOrderRequest.salesOrder.subTotal = salesOrderRequest.salesOrder.netTotal - salesOrderRequest.salesOrder.tax;
            salesOrderRequest.salesOrder.paidAmount = ApplicationStateContext.Deposit ?? 0;

            foreach (var item in salesOrderRequest.salesOrderDetail)
            {
                item.netTotal = item.subTotal;
                //item.subTotal = item.netTotal - item.tax;
            }
            
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
