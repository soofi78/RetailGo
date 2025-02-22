﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.View;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
using HashGo.Domain.DataContext;
using HashGo.Domain.Models;
using HashGo.Domain.Models.Base;
using HashGo.Domain.ViewModels.Base;
using HashGo.Infrastructure.DataContext;
using HashGo.Infrastructure.Events;
using HashGo.Infrastructure.Models;
using HashGo.Wpf.App.Behavior;
using HashGo.Wpf.App.Services;
using HashGo.Wpf.App.Views.Controls.KeyboardControl;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Windows.Storage;

namespace HashGo.Wpf.App.BestTech.ViewModels
{
    public class CustomerDetailsPageViewModel : BaseViewModel
    {
        readonly ILoggingService logger;
        readonly INavigationService navigationService;
        readonly IEventAggregator eventAggregator;
        SharedDataService sharedDataService;
        IPopupService popupService;
        DispatcherTimer keyboardMonitorTimer;

        public CustomerDetailsPageViewModel(ILoggingService loggingService,
                                            INavigationService navigationService,
                                            IEventAggregator eventAggregator,
                                            SharedDataService sharedDataService,
                                            IPopupService popupService)
        {
            this.sharedDataService = sharedDataService;
            logger = loggingService;
            this.navigationService = navigationService;
            this.eventAggregator = eventAggregator;
            this.popupService = popupService;

            NextScreenCommand = new RelayCommand(OnMoveToNextScreen);  //,CanMoveToNextScreen
            PreviousScreenCommand = new RelayCommand(OnMoveBackToPreviousScreen);
        }

        private void OnKeyboardMonitorTick(object sender, EventArgs e)
        {
            bool isTabTipRunning = IsTabTipRunning();

            IsKeyboardOpenButtonVisible = !isTabTipRunning;
        }

        private bool IsTabTipRunning()
        {
            var tabTipProcesses = Process.GetProcessesByName("TabTip");
            return tabTipProcesses.Any();
        }



        void SetIsEnabled()
        {
            IsEnabled = !string.IsNullOrEmpty(sharedDataService.CustomerDetailsObj.Name) &&
                   !string.IsNullOrEmpty(sharedDataService.CustomerDetailsObj.ContactNumber) &&
                   sharedDataService.CustomerDetailsObj.PostalCode != null &&
                   !string.IsNullOrEmpty(sharedDataService.CustomerDetailsObj.UnitNo) &&
                   !string.IsNullOrEmpty(sharedDataService.CustomerDetailsObj.AddressLine1) &&
                   !string.IsNullOrEmpty(sharedDataService.CustomerDetailsObj.Email);
        }

        void OnClearData(bool isClearData)
        {
            CustomerDetailsObj = new CustomerDetails();
            CustomerDetailsObj.PropertyChanged += (sender, args) =>
            {
                SetIsEnabled();
            };
        }

        void OnMoveToNextScreen()
        {
            ApplicationStateContext.CustomerDetailsObj = this.CustomerDetailsObj;

            bool canProceedFurthur = popupService.ShowPopup(Core.Contracts.Enums.PopupType.eConfirmCustomerDetails);


        }

        void OnMoveBackToPreviousScreen()
        {
            if(EnableTextBoxKeyboardBehaviour.KeyboardControl!=null)
                EnableTextBoxKeyboardBehaviour.KeyboardControl.Close();
            navigationService.NavigateToAsync(Pages.DineDateSelect.ToString());
        }

        #region Commands

        public ICommand PreviousScreenCommand { get; private set; }
        public ICommand NextScreenCommand { get; private set; }
        public ICommand OpenKeyboardCommand { get; private set; }

        #endregion

        #region Properties

        public CustomerDetails CustomerDetailsObj { get => sharedDataService.CustomerDetailsObj; set { sharedDataService.CustomerDetailsObj = value; OnPropertyChanged(); } }


        bool isKeyboardOpenButtonVisible = false;
        public bool IsKeyboardOpenButtonVisible
        {
            get => isKeyboardOpenButtonVisible;
            set
            {
                isKeyboardOpenButtonVisible = value;
                OnPropertyChanged();
            }
        }

        bool isEnabled = false;
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsNavigateToConfirmCustomerDetailsScreen { get; set; }


        #endregion

        public override void ViewLoaded()
        {
            CustomerDetailsObj = sharedDataService.CustomerDetailsObj;

            CustomerDetailsObj.PropertyChanged += (sender, args) =>
            {
                SetIsEnabled();
            };
            SetIsEnabled();
        }
    }


}
