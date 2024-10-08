﻿using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.View;
using HashGo.Core.Contracts.Views;
using HashGo.Core.Enum;
using HashGo.Domain.Models.Base;
using HashGo.Domain.ViewModels.Base;
using HashGo.Infrastructure.DataContext;
using HashGo.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HashGo.Wpf.App.BestTech.ViewModels.Popups
{
    public class ConfirmCustomerDetailsPopupViewModel : BaseViewModel
    {
        SharedDataService sharedDataService;
        INavigationService navigationService;

        public ConfirmCustomerDetailsPopupViewModel(SharedDataService sharedDataService,
                                                    INavigationService navigationService)
        {
            this.sharedDataService = sharedDataService;
            this.navigationService = navigationService;

            CloseConfirmCustomerDetailsCommand = new RelayCommand(OnCloseConfirmCustomerDetails);
            EditCustomerDetailsCommand = new RelayCommand(OnEditCustomerDetails);
            this.ConfirmCustomerDetailsCommand = new RelayCommand(OnConfirmCustomerDetails);
        }

        void OnEditCustomerDetails()
        {
            DialogResult = true;


            navigationService.NavigateToAsync(Pages.DineDateSelect.ToString());
        }

        void OnConfirmCustomerDetails()
        {
            DialogResult = true;
            navigationService.NavigateToAsync(Pages.PaymentMethod.ToString());
        }

        void OnCloseConfirmCustomerDetails()
        {
            DialogResult = true;
        }

        #region Properties

        bool isConfirm;
        public bool IsConfirm 
        {
            get => isConfirm;
            set
            {
                isConfirm = value;
                OnPropertyChanged();
            }
        }

        bool? dialogResult;
        public bool? DialogResult
        {
            get => dialogResult;
            set
            {
                dialogResult = value;
                OnPropertyChanged();
            }
        }

        public CustomerDetails CustomerDetailsObj 
        {
            get => sharedDataService.CustomerDetailsObj; 
        }

        public DateTime SelectedDate
        {
            get => sharedDataService.CustomerDateTime;
        }


        #endregion

        #region Commands

        public ICommand CloseConfirmCustomerDetailsCommand { get; private set; }

        public ICommand EditCustomerDetailsCommand { get; private set; }
        public ICommand ConfirmCustomerDetailsCommand { get; private set; }
        #endregion
    }
}
