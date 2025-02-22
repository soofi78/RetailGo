﻿using CommunityToolkit.Mvvm.ComponentModel;
using HashGo.Core.Models.BestTech;
using HashGo.Infrastructure.Common;
using HashGo.Infrastructure.DataContext;
using HashGo.Infrastructure.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace HashGo.Domain.Models.Base
{
    public class BaseModel : ObservableObject
    {
    }

    public class Unit : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int UnitId { get; set; }
        string unitName;

        public string UnitName { get => unitName; set { unitName = value; OnPropertyChanged(); } }

        string imageSource;

        public string ImageSource { get => imageSource; set { imageSource = value; OnPropertyChanged(); } }

        public double UnitPrice { get => unitPrice; set { unitPrice = value; OnPropertyChanged(); } }

        public double AddOnsPrice
        {
            get
            {
                if (this.LstUnitInstallationTypes != null && this.LstUnitInstallationTypes.Count > 0)
                {
                    return LstUnitInstallationTypes.Where(ee => ee.InstallationTypeCount > 0).Sum(xx => xx.AddOnPrice * xx.InstallationTypeCount);
                }
                return 0;
            }
        }


        int unitCount = 1;
        public int UnitCount
        {
            get => unitCount;
            set
            {
                unitCount = value;

                TotalPrice = unitCount * UnitPrice;
                OnPropertyChanged();
            }
        }

        double totalPrice;

        public List<SelectedUnitInstallationType> LstUnitInstallationTypes
        {
            get => lstUnitInstallationTypes;
            set
            {
                lstUnitInstallationTypes = value;
                OnPropertyChanged();
            }
        }

        public string SelectedAddOns
        {
            get
            {
                var selectedAddons = this.LstSelectedUnitInstallationTypes?.Where(ee => ee.InstallationTypeCount > 0);
                string addOnText = string.Empty;
                foreach (var addOn in selectedAddons)
                {
                    if (addOn.InstallationType == "No Add-Ons")
                    {
                        addOnText += ".   " + addOn.InstallationType + "\n";
                    }
                    else
                    {
                        addOnText += ".   " + addOn.InstallationType + "\t" +
                                              addOn.InstallationTypeCount +
                                              "X" + "\t" + "+  " + " $" +
                                              string.Format("{0:N}", addOn.InstallationTypeCount * addOn.AddOnPrice) + "\n";
                    }

                }
                return addOnText;
            }
        }

        public string SelectedAddOnsCheckOutPage
        {
            get
            {
                var selectedAddons = LstUnitInstallationTypes?.Where(ee => ee.InstallationTypeCount > 0);
                string addOnText = string.Empty;
                foreach (var addOn in selectedAddons)
                {
                    if (addOn.InstallationType == "No Add-Ons")
                    {
                        addOnText += ".   " + addOn.InstallationType + "\n";
                    }
                    else
                    {
                        addOnText += ".   " + addOn.InstallationTypeCount +
                                              "X" + "\t" + addOn.InstallationType + "\t" +
                                              "+$" + (addOn.InstallationTypeCount * addOn.AddOnPrice) + "\n";
                    }
                }
                return addOnText;
            }
        }

        double unitPrice;

        public double TotalPrice
        {
            get => totalPrice;
            set
            {
                totalPrice = value;
                OnPropertyChanged();
            }
        }

        decimal taxPercantage = 0.0M;
        public decimal TaxPercantage
        {
            get => taxPercantage;
            set => taxPercantage = value;
        }

        public int TaxId { get; }

        decimal taxAmount;
        public decimal TaxAmount
        {
            get { return taxAmount; }
            set
            {
                //taxAmount = value;

                if (value != default(decimal))
                {
                    taxAmount = Math.Round(value, 2);
                }
            }
        }

        List<string> imageSources = new List<string>();

        string descriptionNotes;

        public List<string> ImageSources { get => imageSources; set { imageSources = value; } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }

        public Unit()
        {
            //LstUnitInstallationTypes = new List<SelectedUnitInstallationType>()
            //                       {
            //                           new SelectedUnitInstallationType(UniqueIdGenerator.GenerateId(), "No Add-Ons", CommonConstants.NOADDONIAMGE, UnitId),
            //                           new SelectedUnitInstallationType(UniqueIdGenerator.GenerateId(),"Power Point", CommonConstants.DEFAULTIMAGE, UnitId),
            //                           new SelectedUnitInstallationType(UniqueIdGenerator.GenerateId(),"Stainless Steel Bracket", CommonConstants.DEFAULTIMAGE, UnitId),
            //                       };
        }

        public Unit(int unitId, int id, string unitName, string name, string imageSource, double unitPrice, string remarks, string taxPerc, int taxId)
        {
            Name = name;
            UnitId = unitId;
            Id = id;
            UnitName = unitName;
            ImageSource = imageSource;
            UnitPrice = unitPrice;
            TotalPrice = unitPrice * unitCount;
            DescriptionNotes = remarks;
            TaxPercantage = Convert.ToDecimal(taxPerc);
            TaxId = taxId;
            //descriptionNotes = "An article is a piece of writing written for a large audience. The main motive behind writing an article is that it should be published in either newspapers or magazines or journals so as to make some difference to the world. It may be the topics of interest of the writer or it may be related to some current issues.An article is a piece of writing written for a large audience. The main motive behind writing an article is that it should be published in either newspapers or magazines or journals so as to make some difference to the world. It may be the topics of interest of the writer or it may be related to some current issues.An article is a piece of writing written for a large audience. The main motive behind writing an article is that it should be published in either newspapers or magazines or journals so as to make some difference to the world. It may be the topics of interest of the writer or it may be related to some current issues.";

            //if (ApplicationStateContext.Tax  == null)
            //{
            //    ApplicationStateContext.Tax = TaxPercantage;
            //}

            if (TaxPercantage > 0)
            {
                if (ApplicationStateContext.IsSalesTaxInclusive)
                {
                    TaxAmount = (Convert.ToDecimal(UnitPrice) * TaxPercantage) / (100 + TaxPercantage);
                }
                else
                {
                    TaxAmount = Convert.ToDecimal(UnitPrice) * (TaxPercantage / 100);
                }
            }
        }

        SelectedUnitInstallationType selectedUnitInstallationTypeObj;

        public SelectedUnitInstallationType SelectedUnitInstallationTypeObj
        {
            get => selectedUnitInstallationTypeObj;
            set
            {
                selectedUnitInstallationTypeObj = value;
                OnPropertyChanged();
            }
        }

        public void AddAddOns(IReadOnlyCollection<ServiceUnit> result)
        {
            List<SelectedUnitInstallationType> tmpPlst = new List<SelectedUnitInstallationType>();
            tmpPlst.Add(new SelectedUnitInstallationType(-1, "No Add-Ons", CommonConstants.NOADDONIAMGE, UnitId, 0, 0.0M, 0, 0, "Others",0));

            if (result != null && result.Count > 0)
            {
                foreach (var addOn in result)
                {
                    int installationTypeCount = 0;
                    if (this.LstSelectedUnitInstallationTypes != null && this.LstSelectedUnitInstallationTypes.Count > 0)
                    {
                        var slctdInstallationType = this.LstSelectedUnitInstallationTypes.FirstOrDefault(ee => ee.UnitId == addOn.unitId &&
                                                                                ee.InstallationTypeId == addOn.id);

                        if (slctdInstallationType != null)
                        {
                            installationTypeCount = slctdInstallationType.InstallationTypeCount;
                        }


                    }

                    tmpPlst.Add(new SelectedUnitInstallationType(addOn.id,
                                                                 addOn.name,
                                                                 string.IsNullOrEmpty(addOn.imagePath) ? CommonConstants.DEFAULTIMAGE : addOn.imagePath,
                                                                 addOn.unitId, addOn.price,
                                                                 Convert.ToDecimal(addOn.taxPercentage),
                                                                 addOn.taxId,
                                                                 addOn.subCategoryId ?? 0,
                                                                 addOn.subCategoryName ?? "Others",
                                                                 installationTypeCount));
                }
            }

            LstUnitInstallationTypes = new List<SelectedUnitInstallationType>(tmpPlst);

            if (LstSelectedUnitInstallationTypes.Count == 1 &&
                           LstSelectedUnitInstallationTypes.Any(ee => ee.InstallationType == "No Add-Ons"))
            {
                this.SelectedUnitInstallationTypeObj = LstUnitInstallationTypes.FirstOrDefault(ee => ee.InstallationType == "No Add-Ons");
            }


        }

        List<SelectedUnitInstallationType> lstUnitInstallationTypes = new List<SelectedUnitInstallationType>();
        public List<SelectedUnitInstallationType> LstSelectedUnitInstallationTypes { get; set; } = new List<SelectedUnitInstallationType>();
        public string DescriptionNotes { get => descriptionNotes; set => descriptionNotes = value; }
    }

    public class SelectedUnitInstallationType : INotifyPropertyChanged
    {
        public int UnitId { get; set; }
        public int InstallationTypeId { get; set; }
        string installationType;

        public string InstallationType
        {
            get => installationType;
            set
            {
                installationType = value;
            }
        }

        string imageSource;

        public string ImageSource { get => imageSource; set { imageSource = value; OnPropertyChanged(); } }

        public int InstallationTypeCount
        {
            get => installationTypeCount;
            set
            {
                installationTypeCount = value;
                OnPropertyChanged();
            }
        }

        public double AddOnPrice
        {
            get => addOnPrice;
            set
            {
                addOnPrice = value;
                OnPropertyChanged();
            }
        }

        double addOnPrice = 1500.00;

        decimal taxPercantage = 0.0M;
        public decimal TaxPercantage
        {
            get => taxPercantage;
            set => taxPercantage = value;
        }

        decimal taxAmount = 0.0M;
        public decimal TaxAmount
        {
            get { return taxAmount; }
            set
            {
                //taxAmount = value;

                if (value != default(decimal))
                {
                    taxAmount = Math.Round(value, 2);
                }
            }
        }

        public int? SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }

        int installationTypeCount;
        public int TaxId { get; }

        public SelectedUnitInstallationType(int installationTypeId,
                                            string installationType,
                                            string imageSource, int unitId,
                                            double addOnPrice,
                                            decimal taxPercentage,
                                            int taxId,
                                            int subcategoryId,
                                            string subcategoryName,
                                            int installationTypeCount = 0)
                                            
        {
            InstallationTypeId = installationTypeId;
            InstallationType = installationType;
            ImageSource = imageSource;
            UnitId = unitId;
            AddOnPrice = addOnPrice;
            InstallationTypeCount = installationTypeCount;
            TaxPercantage = taxPercentage;
            TaxId = taxId;
            SubCategoryId = subcategoryId;
            SubCategoryName = subcategoryName;

            if (TaxPercantage > 0)
            {
                if (ApplicationStateContext.IsSalesTaxInclusive)
                {
                    TaxAmount = (Convert.ToDecimal(AddOnPrice) * TaxPercantage) / (100 + TaxPercantage);
                }
                else
                {
                    TaxAmount = Convert.ToDecimal(AddOnPrice) * (TaxPercantage / 100);
                }
            }
        }
    }

    public class Category : INotifyPropertyChanged
    {
        string categoryName;


        public string CategoryName { get => categoryName; set { categoryName = value; OnPropertyChanged(); } }

        ObservableCollection<Unit> lstUnits = new ObservableCollection<Unit>();
        public ObservableCollection<Unit> LstUnits { get => lstUnits; set { lstUnits = value; OnPropertyChanged(); } }

        string background = "White";
        public string Background { get => background; set { background = value; OnPropertyChanged(); } }

        public List<Unit> SelectedUnits { get => selectedUnits; set { selectedUnits = value; OnPropertyChanged(); } }

        List<Unit> selectedUnits = new List<Unit>();


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }

        public Category(string categoryName)
        {
            CategoryName = categoryName;
        }
    }

    public class SharedDataService   //: INotifyPropertyChanged
    {
        public string DepartmentName { get; set; }
        public int AddOnId { get; set; }
        public List<Unit> SelectedUnits { get; set; } = new List<Unit>();

        Unit selectedUnit = null;
        public Unit SelectedUnit
        {
            get
            {
                return selectedUnit;
            }
            set
            {
                selectedUnit = value;
                //if(value != null)
                //{
                //    AddItem(value);
                //}
            }
        }

        public void ClearData()
        {
            SelectedUnits = new List<Unit>();
            SelectedUnit = null;
            RefferalCode = null;
            AddOnId = 0;
            DepartmentName = null;
        }

        public void ClearCustomerData()
        {
            CustomerDetailsObj = new CustomerDetails();
            CustomerDateTime = DateTime.Now;
        }

        public void AddItem(Unit item)
        {
            if (item != null)
                SelectedUnits.Add(item);
        }

        public CustomerDetails CustomerDetailsObj { get; set; } = new CustomerDetails();
        public DateTime CustomerDateTime { get; set; } = DateTime.Now;

        public string RefferalCode { get; set; } = null;
    }

    public class UniqueIdGenerator
    {
        private static Random _random = new Random();
        private static HashSet<int> _generatedIds = new HashSet<int>();

        public static int GenerateId()
        {
            int newId;
            do
            {
                newId = _random.Next(int.MinValue, int.MaxValue);
            } while (_generatedIds.Contains(newId));

            _generatedIds.Add(newId);
            return newId;
        }
    }
}
