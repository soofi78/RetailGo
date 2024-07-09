﻿using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
using HashGo.Domain.Models.Base;
using HashGo.Domain.Services;
using HashGo.Domain.ViewModels.Base;
using HashGo.Infrastructure.Events;
using HashGo.Wpf.App.BestTech.Views;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace HashGo.Wpf.App.BestTech.ViewModels
{
    public class AddOnsSelectionPageViewmodel : BaseViewModel
    {
        readonly ILoggingService logger;
        readonly INavigationService navigationService;
        readonly IEventAggregator eventAggregator;
        SharedDataService sharedDataService;
        readonly IRetailConnectService retailConnectService;

        public AddOnsSelectionPageViewmodel(ILoggingService loggingService,
                                           INavigationService navigationService,
                                           IEventAggregator eventAggregator,
                                           SharedDataService sharedDataService,
                                           IRetailConnectService retailConnectService)
        {
            this.sharedDataService = sharedDataService;
            logger = loggingService;
            this.navigationService = navigationService;
            this.eventAggregator = eventAggregator;
            this.retailConnectService = retailConnectService;

            NextScreenCommand = new RelayCommand(OnMoveToNextScreen);
            PreviousScreenCommand = new RelayCommand(OnMoveToPreviousScreen);
            //SelectedTabChangedCommand = new RelayCommand<object>(OnSelectedTabChanged);
            AddOnsSelectionChangedCommand = new RelayCommand<object>(OnAddOnsSelectionChanged);
            SelectedAddonCommand = new RelayCommand<SelectedUnitInstallationType>(OnSelectedAddon);
            SubtractProductCommand = new RelayCommand<SelectedUnitInstallationType>(OnRemoveAddOn);
            AddProductCommand = new RelayCommand<object>(OnAddProductClicked);
        }

        void OnAddProductClicked(object obj)
        {
            //if(CanAddItem)
            //{
            //    sharedDataService.AddItem(this.SelectedUnit);
            //    sharedDataService.SelectedUnit = null;

            //    ItemsAddedPopup itemsAddedPopup = new ItemsAddedPopup() { UnitsCount = sharedDataService.SelectedUnits.Sum(ee => ee.UnitCount) };
            //    itemsAddedPopup.Show();

            //    var timer = new System.Windows.Threading.DispatcherTimer();
            //    timer.Tick += (s, args) =>
            //    {
            //        itemsAddedPopup.Close();
            //        timer.Stop();
            //    };
            //    timer.Interval = TimeSpan.FromSeconds(1);
            //    timer.Start();
            //}

            //navigationService.NavigateToAsync(Pages.ProductSelection.ToString());

            if (CanAddItem)
            {
                sharedDataService.AddItem(this.SelectedUnit);
                sharedDataService.SelectedUnit = null;
                navigationService.NavigateToAsync(Pages.ItemAdded.ToString());
            }
            else
            {
                if(this.IsNavigateToPaymentsPage)
                {
                    navigationService.NavigateToAsync(Pages.Payment.ToString());
                }
                else
                {
                    navigationService.NavigateToAsync(Pages.ProductSelection.ToString());
                }
            }
        }

        private void OnRemoveAddOn(SelectedUnitInstallationType selectedUnitInstallationType)
        {
            //if (selectedUnitInstallationType == null || selectedUnitInstallationType.InstallationTypeCount == 0)
            //    return;

            //selectedUnitInstallationType.InstallationTypeCount--;
        }

        void OnSelectedAddon(SelectedUnitInstallationType selectedUnitInstallationType)
        {
            //if (selectedUnitInstallationType == null)
            //    return;

            //selectedUnitInstallationType.InstallationTypeCount++;
        }

        void OnAddOnsSelectionChanged(object obj)
         {
            //TODO
            if (obj is SelectedUnitInstallationType selectedUnitInstallationType )
            {
                selectedUnitInstallationType.InstallationTypeCount++;

                if (selectedUnitInstallationType.InstallationType ==  "No Add-Ons" &&
                   sharedDataService.SelectedUnit != null)
                {
                    var otherSelectedInstallationTypes = sharedDataService.SelectedUnit
                                                                           .LstUnitInstallationTypes.Where(ee =>ee.InstallationType != selectedUnitInstallationType.InstallationType &&
                                                                                                            ee.InstallationTypeCount > 0);
                    if(otherSelectedInstallationTypes.Count() > 0)
                    {
                        foreach (var item in otherSelectedInstallationTypes)
                        {
                            item.InstallationTypeCount = 0;
                        }
                    }
                }

                OnPropertyChanged(nameof(SelectedAddOns));
            }
         }

        private void OnMoveToPreviousScreen()
        {
            var parameters = new Dictionary<string, object>
                {
                    { nameof(SelectedUnit), null },
                };
            navigationService.NavigateToAsync(Pages.ProductSelection.ToString(), parameters);  //parameters
        }

        private void OnMoveToNextScreen()
        {
            ItemsAddedPopup itemsAddedPopup = new ItemsAddedPopup() { UnitsCount  = sharedDataService.SelectedUnits.Sum(ee => ee.UnitCount) };
            itemsAddedPopup.Show();

            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += (s, args) =>
            {
                itemsAddedPopup.Close();
                timer.Stop();

                //Open payments screen
                if(CanAddItem)
                {
                    var parameters = new Dictionary<string, object>
                    {
                        { "SelectedUnits", sharedDataService.SelectedUnits },
                    };
                    navigationService.NavigateToAsync(Pages.Payment.ToString(), parameters);
                }
                else
                {
                    navigationService.NavigateToAsync(Pages.Payment.ToString());
                }
            };
            timer.Interval = TimeSpan.FromSeconds(1); 
            timer.Start();
        }

        public override void ViewLoaded()
        {
            if(sharedDataService.SelectedUnit?.LstUnitInstallationTypes != null)
                LstUnitInstallationTypes = new List<SelectedUnitInstallationType>(sharedDataService.SelectedUnit?.LstUnitInstallationTypes);
            eventAggregator.GetEvent<ClearAllSelectedDataEvent>().Subscribe(OnClearData);

            if (sharedDataService.AddOnId != 0)
            {
                sharedDataService.SelectedUnit?.AddAddOns(this.retailConnectService.GetProductsByCategoryId(sharedDataService.AddOnId).Result);
                OnPropertyChanged(nameof(LstUnitInstallationTypes));
            }
        }

        private void OnClearData(bool obj)
        {
            this.sharedDataService.ClearData();
            //SelectedUnit = null;
            //SelectedUnits = new List<Unit>();
        }

        public override void ViewUnloaded()
        {
            eventAggregator.GetEvent<ClearAllSelectedDataEvent>().Unsubscribe(OnClearData);
        }

        #region Commands

        public ICommand PreviousScreenCommand { get; private set; }
        public ICommand NextScreenCommand { get; private set; }
        //public RelayCommand<object> SelectedTabChangedCommand { get; private set; }
        public RelayCommand<object> AddOnsSelectionChangedCommand { get; private set; }

        public RelayCommand<SelectedUnitInstallationType> SelectedAddonCommand { get; private set; }

        public RelayCommand<SelectedUnitInstallationType> SubtractProductCommand { get; private set; }

        public RelayCommand<object> AddProductCommand { get; private set; }

        #endregion

        #region Properties

        public bool IsNavigateToPaymentsPage { get; set; }

        public string SelectedUnitName { get { return sharedDataService?.SelectedUnit?.Name; } }

        public string SelectedUnitImage { get { return sharedDataService?.SelectedUnit?.ImageSource; } }

        public bool CanAddItem { get; set; } = true;

        Unit selectedUnit;

        public Unit SelectedUnit 
        {
            get => sharedDataService.SelectedUnit;
            set
            {
                sharedDataService.SelectedUnit = value;
            }
        }

        public string SelectedAddOns
        {
            get { return sharedDataService.SelectedUnit.SelectedAddOns; }
        }

        public List<SelectedUnitInstallationType> LstUnitInstallationTypes
        {
            get
            {
                return sharedDataService?.SelectedUnit?.LstUnitInstallationTypes;
            }
            set
            {
                sharedDataService.SelectedUnit.LstUnitInstallationTypes = value;
                OnPropertyChanged();
            }
        }

        public List<SelectedUnitInstallationType> SelectedUnitUpgradeTypes
        {
            get => selectedUnitUpgradeTypes;
            set
            {
                selectedUnitUpgradeTypes = value;
                OnPropertyChanged();
            }
        }

        

        List<SelectedUnitInstallationType> selectedUnitInstallationTypes  = new List<SelectedUnitInstallationType>();
        List<SelectedUnitInstallationType> selectedUnitUpgradeTypes = new List<SelectedUnitInstallationType>();

        #endregion
    }
}
