﻿using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.StoreService;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Db;
using HashGo.Core.Enum;
using HashGo.Domain.DataContext;
using HashGo.Domain.ViewModels;
using HashGo.Domain.ViewModels.Base;
using HashGo.Infrastructure;
using HashGo.Infrastructure.Common;
using HashGo.Infrastructure.DataContext;
using HashGo.Infrastructure.HttpHelper;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HashGo.Wpf.App.BestTech.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        ITenantConnectStoreService tenantConnectStoreService;
        ILoggingService loggingService;
        INavigationService navigationService;
        IRetailConnectService retailConnectService;

        public SettingsPageViewModel(ITenantConnectStoreService tenantConnectStoreService,
            IRetailConnectService retailConnectService,
            INavigationService navigationService,
                                     ILoggingService loggingService) 
        {
            this.tenantConnectStoreService = tenantConnectStoreService;
            this.loggingService = loggingService;
            this.navigationService = navigationService;
            this.retailConnectService = retailConnectService;

            PreviousScreenCommand = new RelayCommand(OnPreviousScreenClicked);
            CancelCommand = new RelayCommand(OnCancelClicked);
            AddOrUpdateTenantCommand = new RelayCommand(OnAddOrUpdateClicked);
            SelectBackgroundImageCommand = new RelayCommand(OnSelectBackgroundClicked);
        }

        void OnSelectBackgroundClicked()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Title = "Open File";

            if (openFileDialog.ShowDialog() == true)
            {
                BackgroundImage = openFileDialog.FileName;
            }
        }

        void OnAddOrUpdateClicked()
        {
            //Save Tenant back to HashGoAppSettings
            HashGoAppSettings.Url = ConnectItem.Url;
            HashGoAppSettings.User = ConnectItem.User;
            HashGoAppSettings.Password = ConnectItem.Password;
            HashGoAppSettings.Tenant = ConnectItem.TenantName;
            HashGoAppSettings.DeviceId = ConnectItem.DeviceId;
            HashGoAppSettings.LocationId = ConnectItem.LocationId;
            HashGoAppSettings.TenantId = ConnectItem.TenantId;
            HashGoAppSettings.SortOrder = ConnectItem.SortOrder.ToString();
            HashGoAppSettings.PaymentScreenVisibleDelay = ConnectItem.PaymentScreenVisibleDelay;
            HashGoAppSettings.NETSPort = ConnectItem.NETSPort;
            HashGoAppSettings.BackgroundImage = BackgroundImage;
            HashGoAppSettings.CurrencySymbol = CurrencySymbol;
            HashGoAppSettings.MenuBackgroundTransparency = MenuBackgroundTransparency.ToString();
            HashGoAppSettings.ShowLanguageSelection = ShowLanguageSelection;
            HashGoAppSettings.ShowMemberButton = ShowMemberButton;
            HashGoAppSettings.PrinterName = PrinterName;
            HashGoAppSettings.NETSIP = NetsIP;
            HashGoAppSettings.NETSQRHOSTID = NetsQrHostId;
            HashGoAppSettings.NETSQRHOSTMID = NetsQrHostMId;
            HashGoAppSettings.NETSQRGATEWAYTOKEN = NetsQrGatewaytoken;
            HashGoAppSettings.NETSQRTIMER = NetsQRTimer;

            HashGoAppSettings.SaveSettings();
            ConnectItem = new TenantConnect();
            ApplicationStateContext.LoadSettings();

            retailConnectService.TryLogin(HashGoAppSettings.Tenant, HashGoAppSettings.User, HashGoAppSettings.Password);

            navigationService.NavigateToAsync(Pages.RestaurantStartup.ToString());
        }

        void OnCancelClicked()
        {
            ConnectItem = new TenantConnect();
            navigationService.NavigateToAsync(Pages.RestaurantStartup.ToString());
        }

        void OnPreviousScreenClicked()
        {
            
            navigationService.NavigateToAsync(Pages.RestaurantStartup.ToString());
        }

        public override void ViewLoaded()
        {
            HashGoAppSettings.LoadSettings();
            ApplicationStateContext.LoadSettings();
            SelectedTabIndex = 0;

            this.ConnectItem = new TenantConnect()
            {
                Url = ApplicationStateContext.ConnectItem?.Url,
                User = ApplicationStateContext.ConnectItem?.User,
                Password = ApplicationStateContext.ConnectItem?.Password,
                TenantName = ApplicationStateContext.ConnectItem?.TenantName,
                DeviceId = ApplicationStateContext.ConnectItem?.DeviceId,
                LocationId = ApplicationStateContext.ConnectItem?.LocationId,
                TenantId = ApplicationStateContext.ConnectItem?.TenantId,
                SortOrder = (ApplicationStateContext.ConnectItem?.SortOrder == 0)?0: Convert.ToInt32(ApplicationStateContext.ConnectItem?.SortOrder),
                PaymentScreenVisibleDelay = ApplicationStateContext.ConnectItem?.PaymentScreenVisibleDelay,
                 NETSPort = HashGoAppSettings.NETSPort
        };
            NetsIP = HashGoAppSettings.NETSIP;
            NetsQrHostId= HashGoAppSettings.NETSQRHOSTID;
            NetsQrHostMId = HashGoAppSettings.NETSQRHOSTMID;
            NetsQrGatewaytoken = HashGoAppSettings.NETSQRGATEWAYTOKEN;
            NetsQRTimer = HashGoAppSettings.NETSQRTIMER;
            BackgroundImage = HashGoAppSettings.BackgroundImage;
            CurrencySymbol = HashGoAppSettings.CurrencySymbol;
            MenuBackgroundTransparency = Convert.ToDouble(HashGoAppSettings.MenuBackgroundTransparency);
            ShowLanguageSelection = HashGoAppSettings.ShowLanguageSelection;
            ShowMemberButton = HashGoAppSettings.ShowMemberButton;
            PrinterName = HashGoAppSettings.PrinterName;
        }

        #region Properties

        TenantConnect connectItem;

        public TenantConnect ConnectItem
        {
            get => ApplicationStateContext.ConnectItem;
            set
            {
                ApplicationStateContext.ConnectItem = value;
                OnPropertyChanged();
            }
        }

        int selectedTabIndex = 0;
        public int SelectedTabIndex
        {
            get => selectedTabIndex;
            set
            {
                selectedTabIndex = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _printerNameList;

        public ObservableCollection<string> PrinterNameList
        {
            get
            {
                if (_printerNameList == null)
                {
                    _printerNameList = new ObservableCollection<string>();

                    foreach (string printer in PrinterSettings.InstalledPrinters) _printerNameList.Add(printer);
                }

                return _printerNameList;
            }
        }

        string printerName;
        public string PrinterName
        {
            get => printerName;
            set
            {
                printerName = value;
                OnPropertyChanged();
            }
        }

        string backgroundImage;

        public string BackgroundImage 
        {
            get => backgroundImage;
            set
            {
                backgroundImage = value;
                OnPropertyChanged();
            }
        }

        string netsIP;
        public string NetsIP
        {
            get => netsIP;
            set
            {
                netsIP = value;
                OnPropertyChanged();
            }
        }

        string netsQrHostId;
        public string NetsQrHostId
        {
            get => netsQrHostId;
            set
            {
                netsQrHostId = value;
                OnPropertyChanged();
            }
        }

        string netsQrHostMId;
        public string NetsQrHostMId
        {
            get => netsQrHostMId;
            set
            {
                netsQrHostMId = value;
                OnPropertyChanged();
            }
        }

        string netsQrGatewaytoken;
        public string NetsQrGatewaytoken
        {
            get => netsQrGatewaytoken;
            set
            {
                netsQrGatewaytoken = value;
                OnPropertyChanged();
            }
        }

        string netsQRTimer;
        public string NetsQRTimer
        {
            get => netsQRTimer;
            set
            {
                netsQRTimer = value;
                OnPropertyChanged();
            }
        }


        bool showLanguageSelection;
        public bool ShowLanguageSelection
        {
            get => showLanguageSelection;
            set
            {
                showLanguageSelection = value;
                OnPropertyChanged();
            }
        }

        bool showMemberButton;

        public bool ShowMemberButton
        {
            get => showMemberButton;
            set
            {
                showMemberButton = value;
                OnPropertyChanged();
            }
        }

        string currencySymbol;

        public string CurrencySymbol
        {
            get => currencySymbol;
            set
            {
                currencySymbol = value;
                OnPropertyChanged();
            }
        }

        private double menuBackgroundTransparency = 0;
        public double MenuBackgroundTransparency
        {
            get => menuBackgroundTransparency;
            set
            {
                menuBackgroundTransparency = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand PreviousScreenCommand { get; private set; }
        public ICommand AddOrUpdateTenantCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand SelectBackgroundImageCommand { get; private set; }
        #endregion
    }
}
