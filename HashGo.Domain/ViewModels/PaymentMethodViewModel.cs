using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.View;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
using HashGo.Core.Models;
using HashGo.Core.Models.BestTech;
using HashGo.Domain.Helper;
using HashGo.Domain.Models.Base;
using HashGo.Infrastructure;
using HashGo.Infrastructure.DataContext;
using HashGo.Infrastructure.Events;
using Prism.Events;
using System.Net.Sockets;
using static System.Net.Mime.MediaTypeNames;

namespace HashGo.Domain.ViewModels
{
    public partial class PaymentMethodViewModel : BaseResturantViewModel
    {
        [ObservableProperty]
        IEnumerable<PaymentMethod> paymentMethods;

        [ObservableProperty]
        Cart cart;


        private INetworkService _NetworkService;
        IRetailConnectService retailConnectService;
        IEventAggregator eventAggregator;
        IPopupService popupService;
        SharedDataService sharedDataService;


        public PaymentMethodViewModel(ILoggingService loggingService,
            IRestaurantBrandService brandService,
                                      INavigationService navigationService,
                                      IOrderService orderService, INetworkService networkService,
                                      IRetailConnectService retailConnectService,
                                      IEventAggregator eventAggregator,
                                      IPopupService popupService, SharedDataService sharedDataService)
            : base(loggingService, brandService, navigationService, orderService)
        {
            this.popupService = popupService;
            this.eventAggregator = eventAggregator;
            _NetworkService = networkService;
            this.retailConnectService = retailConnectService;
            this.sharedDataService = sharedDataService;
        }
        protected override async Task InitializeDataAsync()
        {
            this.Logger.Trace($"{nameof(PaymentMethodViewModel)} : {nameof(InitializeDataAsync)}() Started.");

            await this.LoadDataAsync();

            this.Logger.Trace($"{nameof(PaymentMethodViewModel)} : {nameof(InitializeDataAsync)}() Completed.");

        }

        protected override async Task LoadDataAsync()
        {
            this.Logger.Trace($"{nameof(PaymentMethodViewModel)} : {nameof(LoadDataAsync)}() Started.");

            this.PaymentMethods = await this.OrderService.ReadPaymentMethod();

            this.Logger.Trace($"{nameof(PaymentMethodViewModel)} : {nameof(LoadDataAsync)}() Completed.");

        }

        private bool CanProcessPaymentMethod(PaymentMethod paymentMethod) { return !this.IsLoading && paymentMethod != null; }

        [RelayCommand]
        void NavigateToPaymentPageScreen()
        {
            //var parameters = new Dictionary<string, object>
            //    {
            //        { "IsServiceDepartment", sharedDataService.DepartmentName.ToUpper() == "SERVICING" },
            //    };
            //this.NavigationService.NavigateToAsync(Pages.DineDateSelect.ToString(), parameters);


            this.NavigationService.NavigateToAsync(Pages.CustomerDetails.ToString());
        }

        [RelayCommand(CanExecute = nameof(CanProcessPaymentMethod))]
        private async Task ProcessPaymentMethod(PaymentMethod paymentMethod)
        {
            try
            {
                IsBusy = true;
                this.Logger.Trace($"{nameof(PaymentMethodViewModel)} : {nameof(ProcessPaymentMethod)}() Started.");

                SalesOrderRequest salesOrderRequest = GetSalesOrderRequest();
                string paymentType = GetPaymentType(paymentMethod.PaymentMode);

                IsBusy = false;

                #region Payment   //TODO

                ApplicationStateContext.SalesOrderRequestObject = salesOrderRequest;
                ApplicationStateContext.PaymentMethodObject = paymentMethod;


                if (paymentMethod.Name.ToUpper() == "NETS")
                {
                    //this.NavigationService.NavigateToAsync(Pages.QRPayment.ToString());
                    this.NavigationService.NavigateToAsync(Pages.ProcessingPayment.ToString());
                }
                if (paymentMethod.Name.ToUpper() == "NETS QR")
                {
                    //this.NavigationService.NavigateToAsync(Pages.QRPayment.ToString());
                    this.NavigationService.NavigateToAsync(Pages.ProcessingPayment.ToString());
                }
                else if (paymentMethod.Name.ToUpper() == "VISA")
                {
                    this.NavigationService.NavigateToAsync(Pages.ProcessingPayment.ToString());
                }
                else if (paymentMethod.Name.ToUpper() == "CASH")
                {
                    this.NavigationService.NavigateToAsync(Pages.ProcessingPayment.ToString());
                }
                else if (paymentMethod.Name.ToUpper() == "PAY AT COUNTER")
                {
                    ApplicationStateContext.Deposit = 0;
                    ApplicationStateContext.PaymentMethodObject.PaymentMode = "CASH";
                    paymentMethod.Name = "CASH";
                    paymentType = "CASH";
                    this.NavigationService.NavigateToAsync(Pages.ProcessingPayment.ToString());
                }
                #endregion

                //ApplicationStateContext.ClearData();

                //if(transactionDetails != null && 
                //   !string.IsNullOrEmpty(transactionDetails.transactionNo))
                //{

                //    #region SelectPaymentCommand

                //    if(paymentMethod.Name.Equals("PlutusCard"))
                //    {

                //    }

                //    #endregion

                //    this.OrderQueueNumber = _NetworkService.GetQueueNumber();

                //    var parameters = new Dictionary<string, object>
                //    {
                //        { nameof(this.OrderId), this.OrderId },
                //        { nameof(this.SelectedRestaurant), this.SelectedRestaurant },
                //        { nameof(this.OrderQueueNumber), this.OrderQueueNumber },
                //        { "SelectedPaymentMethod", paymentMethod},
                //        { "TransactionNumber", transactionDetails.transactionNo},
                //    };

                //    await this.NavigateToPage(Pages.OrderConfirmation, parameters);
                //}
                //else
                //{
                //    this.NavigateToPage(Pages.RestaurantStartup, null);
                //}

                this.Logger.Trace($"{nameof(PaymentMethodViewModel)} : {nameof(ProcessPaymentMethod)}() Completed.");
            }

            catch (Exception e)
            {
                IsBusy = false;
            }
        }

        public decimal Deposit
        {
            get
            {
                //decimal amount = Convert.ToDecimal(0.20 * SelectedUnits.Sum(ee => ((ee.UnitPrice + ee.AddOnsPrice) * ee.UnitCount)));
                //if (ApplicationStateContext.NetAmountToPay == 0)
                //{
                //    ApplicationStateContext.NetAmountToPay = amount;
                //}
                //return amount;
                return ApplicationStateContext.Deposit.Value;
            }
        }

        private SalesOrderRequest GetSalesOrderRequest()
        {
            decimal total = 0, tax = 0;
            List<SalesOrderDetail> lstSaleOrderDetails = new List<SalesOrderDetail>();

            foreach (var selectedUnit in SelectedUnits)
            {
                SalesOrderDetail detail = new SalesOrderDetail();
                detail.unitId = selectedUnit.UnitId;
                detail.productId = selectedUnit.Id;
                detail.price = Convert.ToDecimal(selectedUnit.UnitPrice);
                detail.qty = selectedUnit.UnitCount;
                detail.subTotal = detail.price * detail.qty;
                //detail.subTotal = detail.price;
                detail.productName = selectedUnit.Name;
                detail.taxId = selectedUnit.TaxId;
                detail.tax = selectedUnit.TaxAmount;

                total += detail.price;
                tax += detail.tax;
                lstSaleOrderDetails.Add(detail);

                #region AddOns

                if (selectedUnit.LstSelectedUnitInstallationTypes?.Count > 0)
                {
                    foreach (var addon in selectedUnit.LstSelectedUnitInstallationTypes)
                    {
                        SalesOrderDetail addOnDetail = new SalesOrderDetail();
                        addOnDetail.unitId = addon.UnitId;
                        addOnDetail.productId = addon.InstallationTypeId;
                        addOnDetail.price = Convert.ToDecimal(addon.AddOnPrice);
                        addOnDetail.qty = addon.InstallationTypeCount;
                        addOnDetail.subTotal = Convert.ToDecimal(addon.AddOnPrice * addOnDetail.qty);
                        addOnDetail.productName = addon.InstallationType;
                        addOnDetail.tax = addon.TaxAmount;
                        addOnDetail.taxId = addon.TaxId;
                        tax += addOnDetail.tax;
                        //total += Convert.ToDecimal(addon.AddOnPrice * addOnDetail.qty);
                        lstSaleOrderDetails.Add(addOnDetail);
                    }
                }

                #endregion
            }
            ApplicationStateContext.Tax = tax;

            SalesOrderRequest salesOrderRequest = new SalesOrderRequest();
            salesOrderRequest.salesOrderDetail = lstSaleOrderDetails;
            salesOrderRequest.salesOrder = new SalesOrder()
            {
                date = ApplicationStateContext.CustomerDate,
                name = ApplicationStateContext.CustomerDetailsObj?.Name,
                referralCode = this.ReferralCode,
                contactNo = ApplicationStateContext.CustomerDetailsObj?.ContactNumber,
                address = ApplicationStateContext.CustomerDetailsObj?.UnitNo,
                postalCode = Convert.ToString(ApplicationStateContext.CustomerDetailsObj?.PostalCode),
                unitName = ApplicationStateContext.CustomerDetailsObj?.UnitNo,
                floorNumber = ApplicationStateContext.CustomerDetailsObj?.FloorNo,
                locationId = Convert.ToInt32(HashGoAppSettings.LocationId),
                email = ApplicationStateContext.CustomerDetailsObj?.Email,
                tax = ApplicationStateContext.Tax ?? 0,
                netTotal = ApplicationStateContext.NetAmountToPay
                //netTotal = total
            };
            return salesOrderRequest;
        }

        private string GetPaymentType(string sNetsPaymode)
        {
            string sPaymentType = "NETS";
            //picNets.Image = Image.FromFile(Application.StartupPath + @"\images\Nets\Nets.gif");
            switch (sNetsPaymode)
            {
                case "NETSFP":
                    sPaymentType = "FLASHPAY";
                    //picNets.Image = Image.FromFile(Application.StartupPath + @"\images\Nets\NetsFlashPay.gif");
                    break;
                case "NETSQR":
                    sPaymentType = "QR Code";
                    //picNets.Image = Image.FromFile(Application.StartupPath + @"\images\Nets\NetsQR.gif");
                    break;
                case "CASH":
                    sPaymentType = "CASH";
                    break;
                case "CREDITCARD":
                    sPaymentType = "CREDITCARD";
                    //picNets.Image = Image.FromFile(Application.StartupPath + @"\images\Nets\CreditCard.gif");
                    break;
                default:
                    sPaymentType = sNetsPaymode;
                    break;
            }
            //lblPayBy.Text = sPaymentType;

            return sPaymentType;
        }



        bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }

        [RelayCommand]
        void NavigateToStartScreen()
        {
            eventAggregator.GetEvent<ClearAllSelectedDataEvent>().Publish(true);
            this.NavigateToPage(Pages.RestaurantStartup, null);
        }

        [RelayCommand]
        void NavigateToSelectProducts()
        {
            this.NavigateToPage(Pages.ProductSelection, null);
        }

        List<Unit> selectedUnits = new List<Unit>();

        public List<Unit> SelectedUnits
        {
            get => sharedDataService.SelectedUnits;
            set
            {
                sharedDataService.SelectedUnits = value;
                OnPropertyChanged();
            }
        }

        public string ReferralCode { get; private set; }

    }
}
