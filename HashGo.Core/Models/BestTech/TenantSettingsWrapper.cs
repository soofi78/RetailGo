using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models.BestTech
{
    public class TenantSettingsResponse
    {
        public TenantSettingsWrapper result { get; set; }
        public object targetUrl { get; set; }
        public bool success { get; set; }
        public object error { get; set; }
        public bool unAuthorizedRequest { get; set; }
        public bool __abp { get; set; }
    }

    public class TenantSettingsWrapper
    {
        public General general { get; set; }
        public UserManagement userManagement { get; set; }
        public object email { get; set; }
        public object ldap { get; set; }
        public Connect connect { get; set; }
        public Inventory inventory { get; set; }
        public ChartofAccount chartofAccount { get; set; }
        public ReportTermAndCondition reportTermAndCondition { get; set; }
        public DocumentRuleEditDto documentRuleEditDto { get; set; }
        public Security security { get; set; }
    }

    public class Security
    {
        public bool useDefaultPasswordComplexitySettings { get; set; }
        public PasswordComplexity passwordComplexity { get; set; }
        public DefaultPasswordComplexity defaultPasswordComplexity { get; set; }
        public UserLockOut userLockOut { get; set; }
        public TwoFactorLogin twoFactorLogin { get; set; }
    }

    public class TwoFactorLogin
    {
        public bool isEnabledForApplication { get; set; }
        public bool isEnabled { get; set; }
        public bool isEmailProviderEnabled { get; set; }
        public bool isSmsProviderEnabled { get; set; }
        public bool isRememberBrowserEnabled { get; set; }
    }

    public class UserLockOut
    {
        public bool isEnabled { get; set; }
        public int maxFailedAccessAttemptsBeforeLockout { get; set; }
        public int defaultAccountLockoutSeconds { get; set; }
    }

    public class UserManagement
    {
        public bool allowSelfRegistration { get; set; }
        public bool isNewRegisteredUserActiveByDefault { get; set; }
        public bool isEmailConfirmationRequiredForLogin { get; set; }
        public bool useCaptchaOnRegistration { get; set; }
    }

    public class ChartofAccount
    {
        public bool accountCodeAuto { get; set; }
        public DateTime accountEndPeriod { get; set; }
        public bool useDefaultAccounts4Depratment { get; set; }
        public bool payoutVoucherPosting { get; set; }
        public bool accountDetailWithDepartmentAndBrand { get; set; }
    }

    public class Connect
    {
        public double operateHours { get; set; }
        public string timeZoneUtc { get; set; }
        public int decimals { get; set; }
        public string currencySymbol { get; set; }
        public object dateTimeFormat { get; set; }
        public object schedules { get; set; }
        public object weekDay { get; set; }
        public object weekEnd { get; set; }
        public bool salesTaxInclusive { get; set; }
        public string currencyCode { get; set; }
        public string countryCode { get; set; }
        public double posInvoiceRounded { get; set; }
    }

    public class DefaultPasswordComplexity
    {
        public int minLength { get; set; }
        public int maxLength { get; set; }
        public bool useNumbers { get; set; }
        public bool useUpperCaseLetters { get; set; }
        public bool useLowerCaseLetters { get; set; }
        public bool usePunctuations { get; set; }
    }

    public class DocumentRuleEditDto
    {
        public bool poIsActive { get; set; }
        public bool poLocatinPrefix { get; set; }
        public string poType { get; set; }
        public string poPrefix { get; set; }
        public int poLength { get; set; }
        public string poSuffix { get; set; }
        public int poBegining { get; set; }
        public string poDisplay { get; set; }
        public bool piIsActive { get; set; }
        public bool piLocatinPrefix { get; set; }
        public string piType { get; set; }
        public string piPrefix { get; set; }
        public int piLength { get; set; }
        public string piSuffix { get; set; }
        public int piBegining { get; set; }
        public string piDisplay { get; set; }
        public bool prIsActive { get; set; }
        public bool prLocatinPrefix { get; set; }
        public string prType { get; set; }
        public string prPrefix { get; set; }
        public int prLength { get; set; }
        public string prSuffix { get; set; }
        public int prBegining { get; set; }
        public string prDisplay { get; set; }
        public bool preIsActive { get; set; }
        public bool preLocatinPrefix { get; set; }
        public string preType { get; set; }
        public string prePrefix { get; set; }
        public int preLength { get; set; }
        public string preSuffix { get; set; }
        public int preBegining { get; set; }
        public string preDisplay { get; set; }
        public bool precIsActive { get; set; }
        public bool precLocatinPrefix { get; set; }
        public string precType { get; set; }
        public string precPrefix { get; set; }
        public int precLength { get; set; }
        public string precSuffix { get; set; }
        public int precBegining { get; set; }
        public string precDisplay { get; set; }
        public bool soIsActive { get; set; }
        public bool soLocatinPrefix { get; set; }
        public string soType { get; set; }
        public string soPrefix { get; set; }
        public int soLength { get; set; }
        public string soSuffix { get; set; }
        public int soBegining { get; set; }
        public string soDisplay { get; set; }
        public bool doIsActive { get; set; }
        public bool doLocatinPrefix { get; set; }
        public string doType { get; set; }
        public string doPrefix { get; set; }
        public int doLength { get; set; }
        public string doSuffix { get; set; }
        public int doBegining { get; set; }
        public string doDisplay { get; set; }
        public bool posIsActive { get; set; }
        public bool posLocatinPrefix { get; set; }
        public string posType { get; set; }
        public string posPrefix { get; set; }
        public int posLength { get; set; }
        public string posSuffix { get; set; }
        public int posBegining { get; set; }
        public string posDisplay { get; set; }
        public bool siIsActive { get; set; }
        public bool siLocatinPrefix { get; set; }
        public string siType { get; set; }
        public string siPrefix { get; set; }
        public int siLength { get; set; }
        public string siSuffix { get; set; }
        public int siBegining { get; set; }
        public string siDisplay { get; set; }
        public bool siMemberIsActive { get; set; }
        public bool siMemberLocatinPrefix { get; set; }
        public string siMemberType { get; set; }
        public string siMemberPrefix { get; set; }
        public int siMemberLength { get; set; }
        public string siMemberSuffix { get; set; }
        public int siMemberBegining { get; set; }
        public string siMemberDisplay { get; set; }
        public bool srIsActive { get; set; }
        public bool srLocatinPrefix { get; set; }
        public string srType { get; set; }
        public string srPrefix { get; set; }
        public int srLength { get; set; }
        public string srSuffix { get; set; }
        public int srBegining { get; set; }
        public string srDisplay { get; set; }
        public bool poiIsActive { get; set; }
        public bool poiLocatinPrefix { get; set; }
        public string poiType { get; set; }
        public string poiPrefix { get; set; }
        public int poiLength { get; set; }
        public string poiSuffix { get; set; }
        public int poiBegining { get; set; }
        public string poiDisplay { get; set; }
        public bool sreIsActive { get; set; }
        public bool sreLocatinPrefix { get; set; }
        public string sreType { get; set; }
        public string srePrefix { get; set; }
        public int sreLength { get; set; }
        public string sreSuffix { get; set; }
        public int sreBegining { get; set; }
        public string sreDisplay { get; set; }
    }

    public class General
    {
        public object webSiteRootAddress { get; set; }
        public string timezone { get; set; }
        public string timezoneForComparison { get; set; }
    }

    public class Inventory
    {
        public bool itemCodeAuto { get; set; }
        public bool itemCodeNumber { get; set; }
        public int itemCodeDefaultLength { get; set; }
        public bool inventoryStockSimple { get; set; }
        public bool categoryAnalysisSimple { get; set; }
        public bool promotionAnalysisSimple { get; set; }
        public bool dashboardOrderTagChart { get; set; }
        public bool printSalesInvoiceBalance { get; set; }
        public string itemCodeByGroup { get; set; }
        public bool autoComplete { get; set; }
        public bool books { get; set; }
        public bool productTax { get; set; }
        public bool inventoryStock { get; set; }
        public bool autoCartonBundle { get; set; }
        public bool cartonBundle { get; set; }
        public string toUser { get; set; }
        public bool autoCreateVendor { get; set; }
        public bool createPurchaseFromPO { get; set; }
        public bool directPurchase { get; set; }
        public bool itemListbyVendor { get; set; }
        public bool vendorInvoiceNo { get; set; }
        public bool tradeIn { get; set; }
        public bool mrp { get; set; }
        public bool department { get; set; }
        public bool category { get; set; }
        public bool cost { get; set; }
        public bool averageCost { get; set; }
        public string purchaseAverageCost { get; set; }
        public bool brand { get; set; }
        public bool stockOverWrite { get; set; }
        public bool itemCode { get; set; }
        public bool itemName { get; set; }
        public bool aliasName { get; set; }
        public bool aliasNameForSearch { get; set; }
        public bool printQRForSalesInvoice { get; set; }
        public bool exportProductWithPriceGroup { get; set; }
        public bool showCostAmountOnStockTransfer { get; set; }
        public bool noChangeToSalesInvoiceAdvance { get; set; }
        public bool unit { get; set; }
        public bool barcode { get; set; }
        public bool vendor { get; set; }
        public bool price { get; set; }
        public bool mrpListItem { get; set; }
        public bool stockOnHand { get; set; }
        public bool autoExcessStockTransfer { get; set; }
        public bool printBarcode { get; set; }
        public bool updateStock { get; set; }
        public bool stockTakeQty { get; set; }
        public bool updateAllStockTake { get; set; }
        public bool posPriceEdit { get; set; }
        public bool isBarcode13Digit { get; set; }
        public bool allowNegativeStock { get; set; }
        public bool hsnCode { get; set; }
        public bool poExpiryAndDeliveryDate { get; set; }
        public bool salesReturnCreditNote { get; set; }
        public bool allDailyStockMovement { get; set; }
        public bool excludeZeroQuantity { get; set; }
        public bool sortByCategory { get; set; }
        public bool sortByProductCode { get; set; }
        public bool printSerialNumber { get; set; }
        public bool isSubCategoryMandatory { get; set; }
        public bool isBrandMandatory { get; set; }
        public bool isVendorMandatory { get; set; }
        public bool isMRPMandatory { get; set; }
        public bool isSalePriceMandatory { get; set; }
        public bool isUnitCostMandatory { get; set; }
        public bool qtyEditImportProduct { get; set; }
        public bool priceEditImportProduct { get; set; }
        public bool allowNegativeStockImportProduct { get; set; }
        public bool allowZeroPriceUpdateProduct { get; set; }
        public bool enablePaymentTypeForPurchase { get; set; }
        public bool enableProductTaxMRPInBulkEdit { get; set; }
        public bool soBalancePayCurrentTax { get; set; }
        public bool loyaltyByReceiptTotal { get; set; }
        public bool showProductBatchUnitCost { get; set; }
        public bool showProductBatchPrice { get; set; }
        public bool syncShopifyProductByInventoryCode { get; set; }
        public bool syncShopifyProductStockByInventoryCode { get; set; }
        public bool purchaseOrderCondition { get; set; }
        public bool inventoryProfitMarkup { get; set; }
        public bool changePrice { get; set; }
        public bool transactionRemarks { get; set; }
        public bool purchaseOrderWithSO { get; set; }
        public bool soSalesPersonMandatory { get; set; }
        public bool soStatusMandatory { get; set; }
        public bool soDeliveryDateMandatory { get; set; }
        public bool productPriceGroup { get; set; }
        public string printBarcodeDefaultQty { get; set; }
        public string customerPaymentRoundingAmount { get; set; }
        public bool customerAddresslineMandatory { get; set; }
        public bool customerPostalCodeMandatory { get; set; }
        public bool customerPhoneNoMandatory { get; set; }
        public bool customerCodeMandatory { get; set; }
        public bool customerNameMandatory { get; set; }
        public bool isInclusiveTaxDefault { get; set; }
        public bool currencySalesModule { get; set; }
        public bool searchParentProduct { get; set; }
        public bool y3UpdateStock { get; set; }
        public bool canEditMemberCode { get; set; }
        public bool productExportExcelSimple { get; set; }
        public bool showCompanyRemarks { get; set; }
        public bool showCustomerRemarks { get; set; }
        public bool showSalesInvoiceEmployee { get; set; }
        public bool showSalesOrderEmployee { get; set; }
        public bool showSalesOrderFromLocation { get; set; }
        public bool dailySalesDetailReportPrintSimple { get; set; }
        public bool defaultWholeSalesPrice { get; set; }
        public bool showLocationProductPrice { get; set; }
        public bool showPaymentTermPurchaseInvoice { get; set; }
        public bool walkInCustmerDetailOnSO { get; set; }
        public bool customerVendorPaymentDefaultLocation { get; set; }
    }

    public class PasswordComplexity
    {
        public int minLength { get; set; }
        public int maxLength { get; set; }
        public bool useNumbers { get; set; }
        public bool useUpperCaseLetters { get; set; }
        public bool useLowerCaseLetters { get; set; }
        public bool usePunctuations { get; set; }
    }

    public class ReportTermAndCondition
    {
        public string salesQuatation { get; set; }
        public string salesOrder { get; set; }
        public string salesDO { get; set; }
        public string salesInvoice { get; set; }
        public string tradeSalesInvoice { get; set; }
        public string purchaseOrder { get; set; }
        public string purchaseReturn { get; set; }
        public bool purchaseFOCQtyCol { get; set; }
        public bool purchaseUOMCol { get; set; }
        public bool purchaseDiscAmountCol { get; set; }
        public bool purchaseDiscAmountPercCol { get; set; }
        public bool salesUOMCol { get; set; }
        public bool salesInvoiceBarcode { get; set; }
        public bool printSalesOrderQuoatationBarcode { get; set; }
        public bool printSalesInvoiceDOBarcode { get; set; }
        public bool salesDiscAmountCol { get; set; }
        public bool salesDiscAmountPercCol { get; set; }
        public bool barCodeCol { get; set; }
        public bool reportCurrency { get; set; }
        public string subTotalLabel { get; set; }
        public bool poSku { get; set; }
        public bool poBarcode { get; set; }
        public bool poExpiryDate { get; set; }
        public bool poDeliveryDate { get; set; }
        public bool taxRegistrationLabel { get; set; }
        public bool profitCalculationWithTax { get; set; }
        public bool showDeliveryAddressOnSalesInvoice { get; set; }
        public bool showAttributesOnPurchaseDetailReport { get; set; }
        public bool salesInvoiceCenterHeader { get; set; }
        public bool enableCrystalReportForSalesInvoice { get; set; }
        public string crystalReportPathForSalesInvoice { get; set; }
        public bool enableCrystalReportForSalesOrder { get; set; }
        public string crystalReportPathForSalesOrder { get; set; }
        public bool salesOrderTemporaryInvoice { get; set; }
        public string crystalReportPathForSalesOrderTempInvoice { get; set; }
        public bool enableCrystalReportForDeliveryOrder { get; set; }
        public string crystalReportPathForDeliveryOrder { get; set; }
        public bool enableCrystalReportForServiceAgreementMemo { get; set; }
        public string crystalReportPathForServiceAgreementMemo { get; set; }
        public bool enableCrystalReportForComplaintService { get; set; }
        public string crystalReportPathForComplaintService { get; set; }
        public bool salesOrderPayment { get; set; }
        public bool enableCrystalReportForSalesInvoiceDO { get; set; }
        public string crystalReportPathForSalesInvoiceDO { get; set; }
    }
}
