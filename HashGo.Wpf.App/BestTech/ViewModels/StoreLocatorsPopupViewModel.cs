using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Models.BestTech;
using HashGo.Domain.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HashGo.Wpf.App.BestTech.ViewModels
{
    public class StoreLocatorsPopupViewModel : BaseViewModel
    {
        IRetailConnectService retailConnectService;
        public StoreLocatorsPopupViewModel(IRetailConnectService retailConnectService) 
        {
            this.retailConnectService = retailConnectService;
            CloseStoreLocatorsPopupCommand = new RelayCommand(OnCloseStoreLocatorsPopup);
            LoadStoreLocators();
        }

        async void LoadStoreLocators()
        {
            lstStoreLocators = await retailConnectService.GetStoreLocations();

            OnPropertyChanged(nameof(LstStoreLocators));
        }

        void OnCloseStoreLocatorsPopup()
        {
            DiaglogResult = true;
        }

        #region Commands

        public ICommand CloseStoreLocatorsPopupCommand { get; private set; }

        #endregion

        #region Properties

        IReadOnlyCollection<StoreLocators> lstStoreLocators;
        public IReadOnlyCollection<StoreLocators> LstStoreLocators
        {
            get { return lstStoreLocators; }
        }

        bool? diaglogResult;
        public bool? DiaglogResult
        {
            get => diaglogResult;
            set
            {
                diaglogResult = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
