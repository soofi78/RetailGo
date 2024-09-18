using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
using HashGo.Core.Models;
using HashGo.Core.Models.BestTech;
using HashGo.Domain.DataContext;
using HashGo.Domain.Models.Base;
using HashGo.Domain.ViewModels.Base;
using HashGo.Infrastructure.DataContext;
using HashGo.Infrastructure.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HashGo.Wpf.App.BestTech.ViewModels
{
    public  class ConfirmDineDatePageViewModel : BaseViewModel
    {
        readonly ILoggingService logger;
        readonly IRetailConnectService retailConnectService;
        readonly INavigationService navigationService;
        readonly IEventAggregator eventAggregator;
        readonly SharedDataService sharedDataService;
        int deliverySlot1Id = 0;
        int deliverySlot2Id = 0;

        string deliverySlotName1;
        public string DeliverySlotName1
        {
            get => deliverySlotName1;
            set
            {
                deliverySlotName1 = value;
                OnPropertyChanged();
            }
        }

        string deliverySlotName2;
        public string DeliverySlotName2
        {
            get => deliverySlotName2;
            set
            {
                deliverySlotName2 = value;
                OnPropertyChanged();
            }
        }

        public ConfirmDineDatePageViewModel(ILoggingService loggingService,
                                            INavigationService navigationService,
                                            IEventAggregator eventAggregator,
                                            IRetailConnectService retailConnectService,
                                            SharedDataService sharedDataService)
        {
            this.sharedDataService = sharedDataService;
            this.logger = loggingService;
            this.navigationService = navigationService;
            this.eventAggregator = eventAggregator;
            this.retailConnectService = retailConnectService;
            FillDeliverySlot();

            PreviousScreenCommand = new RelayCommand(OnPreviousScreenClicked);
            NextScreenCommand = new RelayCommand(OnMoveToNextScreen);
        }

        void OnClearData(bool isClearData)
        {
             SelectedDate = DateTime.Now;
        }

        void OnMoveToNextScreen()
        {
            ApplicationStateContext.CustomerDate = SelectedDate.Date;
            ApplicationStateContext.IsMorningTime = this.isMorningSelected;
            ApplicationStateContext.IsEveningTime = this.isEveningSelected;



            int deliverySlotId = 0;
            if (IsServiceDepartment)
            {
                if (ApplicationStateContext.IsMorningTime) deliverySlotId = deliverySlot1Id;
                else if (ApplicationStateContext.IsEveningTime) deliverySlotId = deliverySlot2Id;
                else
                {
                    MessageBox.Show("Select the Slot.");
                    return;
                }
            }
            else deliverySlotId = deliverySlot1Id;

            var balanceSlot = this.retailConnectService.BalanceSlotByDeliveryTiming(ApplicationStateContext.DepartmentId, deliverySlotId);
            ApplicationStateContext.deliverySlotId = deliverySlotId;
            if (balanceSlot.Result == 0)
            {
                MessageBox.Show("Delivery Slot is not available. Please choose another time slot.");
                return;
            }

            navigationService.NavigateToAsync(Pages.CustomerDetails.ToString());
        }

        void OnPreviousScreenClicked()
        {
            navigationService.NavigateToAsync(Pages.Payment.ToString());
        }

        void FillDeliverySlot()
        {
            IReadOnlyCollection<DeliveryTimings> lstDeliveryTimings = this.retailConnectService.DeliveryTimingByDept(ApplicationStateContext.DepartmentId).Result;

            int nCnt = 0;
            foreach (var item in lstDeliveryTimings)
            {
                if (nCnt == 0)
                {
                    deliverySlotName1 = item.Description;
                    deliverySlot1Id = item.Id;
                }
                else if (nCnt == 1)
                {
                    deliverySlotName2 = item.Description;
                    deliverySlot2Id = item.Id;
                }
                nCnt++;
            }

        }


        public override void ViewLoaded()
        {
            IsServiceDepartment = (ApplicationStateContext.DepartmentId == 420);

            this.IsMorningSelected = ApplicationStateContext.IsMorningTime;
            this.IsEveningSelected = ApplicationStateContext.IsEveningTime;
            this.SelectedDate = sharedDataService.CustomerDateTime;
            FillDeliverySlot();

        }

        public override void ViewUnloaded()
        {

        }

        #region Commands

        public ICommand PreviousScreenCommand { get; private set; }
        public ICommand NextScreenCommand { get; private set; }

        #endregion

        #region Properties

        DateTime selectedDate = DateTime.Today.Date;
        public DateTime SelectedDate
        {
            get => sharedDataService.CustomerDateTime;
            set
            {
                sharedDataService.CustomerDateTime = value.Date;
                OnPropertyChanged();
            }
        }

        bool isServiceDepartment;
        public bool IsServiceDepartment 
        {
            get => isServiceDepartment;
            set
            {
                isServiceDepartment = value;
                OnPropertyChanged();
            }
        }

        bool isMorningSelected;
        public bool IsMorningSelected 
        {
            get => isMorningSelected;
            set
            {
                isMorningSelected = value;

                if (value)
                    IsEveningSelected = false;
                OnPropertyChanged();
            }
        }

        bool isEveningSelected;
        public bool IsEveningSelected
        {
            get => isEveningSelected;
            set
            {
                isEveningSelected = value;

                if (value)
                    IsMorningSelected = false;

                OnPropertyChanged();
            }
        }



        #endregion
    }
}
