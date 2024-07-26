using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models.BestTech
{
    public class SalesOrderForEditResponse
    {
        public SalesOrderWrapper result { get; set; }
        public string targetUrl { get; set; }
        public bool success { get; set; }
        public object error { get; set; }
        public bool unAuthorizedRequest { get; set; }
        public bool __abp { get; set; }
    }

    public class SalesOrderWrapper
    {
        public SalesOrderForEdit salesOrder { get; set; }
        public List<SalesOrderDetailForEdit> salesOrderDetail { get; set; }
        public List<Vendor> vendors { get; set; }
    }

    public class SalesOrderDetailForEdit
    {
        public int id { get; set; }
        public int salesOrderId { get; set; }
        public int productId { get; set; }
        public string inventoryCode { get; set; }
        public string productName { get; set; }
        public double qty { get; set; }
        public double price { get; set; }
        public double averageCost { get; set; }
        public int unitId { get; set; }
        public string unitName { get; set; }
        public double itemDiscountPerc { get; set; }
        public double itemDiscount { get; set; }
        public double subTotal { get; set; }
        public double tax { get; set; }
        public double netTotal { get; set; }
        public int slNo { get; set; }
        public object remarks { get; set; }
        public List<object> taxForProduct { get; set; }
        public List<object> applicableTaxes { get; set; }
        public object productBarcode { get; set; }
        public double totalValue { get; set; }
        public double qtyStock { get; set; }
        public double lastPurchasePrice { get; set; }
        public double netDiscount { get; set; }
        public bool isBatch { get; set; }
        public int? vendorId { get; set; }
        public double poQuantity { get; set; }
        public double orderedQuantity { get; set; }
        public int departmentId { get; set; }
        public int categoryId { get; set; }
        public int? brandId { get; set; }
        public double netCost { get; set; }
        public object priceGroupId { get; set; }
        public object transactionRemarks { get; set; }
        public object purchaseOrderId { get; set; }
        public object salesPersonId { get; set; }
        public object soNo { get; set; }
        public object productGroup { get; set; }
        public int parentId { get; set; }
        public object productAttributes { get; set; }
        public bool isDeleted { get; set; }
        public object deleterUserId { get; set; }
        public object deletionTime { get; set; }
        public object lastModificationTime { get; set; }
        public object lastModifierUserId { get; set; }
        public DateTime creationTime { get; set; }
        public object creatorUserId { get; set; }
    }

    public class Vendor
    {
        public object vendorCode { get; set; }
        public string name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string city { get; set; }
        public object state { get; set; }
        public string country { get; set; }
        public string postalCode { get; set; }
        public string email { get; set; }
        public string phoneNo { get; set; }
        public string faxNo { get; set; }
        public double purchaseLimit { get; set; }
        public object thirdPartyId { get; set; }
        public bool isTaxExempt { get; set; }
        public string taxRegistrationNo { get; set; }
        public double balanceAmount { get; set; }
        public string bankDetails { get; set; }
        public int productCount { get; set; }
        public bool isDeleted { get; set; }
        public object deleterUserId { get; set; }
        public object deletionTime { get; set; }
        public object lastModificationTime { get; set; }
        public object lastModifierUserId { get; set; }
        public DateTime creationTime { get; set; }
        public object creatorUserId { get; set; }
        public int id { get; set; }
    }

    public class SalesOrderForEdit
    {
        public int id { get; set; }
        public string soNo { get; set; }
        public DateTime soDate { get; set; }
        public int locationId { get; set; }
        public object erpInternalId { get; set; }
        public DateTime deliveryDate { get; set; }
        public int customerId { get; set; }
        public object memberId { get; set; }
        public string customerName { get; set; }
        public bool isCancelled { get; set; }
        public object poNo { get; set; }
        public double soItemDiscount { get; set; }
        public double soNetDiscountPerc { get; set; }
        public double soNetDiscount { get; set; }
        public double soSubTotal { get; set; }
        public double soTax { get; set; }
        public double soNetTotal { get; set; }
        public double soRoundingAmount { get; set; }
        public double otherCharges { get; set; }
        public double soGrandTotal { get; set; }
        public object salespersonId { get; set; }
        public object deliveryPersonId { get; set; }
        public object employeeId { get; set; }
        public double soTotalValue { get; set; }
        public double profitMargin { get; set; }
        public object remarks { get; set; }
        public int creditDays { get; set; }
        public string status { get; set; }
        public object salesOrderStatusId { get; set; }
        public object paymentTermId { get; set; }
        public int tenantId { get; set; }
        public object salesPersonName { get; set; }
        public object branchId { get; set; }
        public object taxName { get; set; }
        public string type { get; set; }
        public double paidAmount { get; set; }
        public double balance { get; set; }
        public double usedAmount { get; set; }
        public object fromLocationId { get; set; }
        public object salesInvoiceId { get; set; }
        public object employeeName { get; set; }
        public object fromLocationName { get; set; }
        public double orderedQty { get; set; }
        public double receivedQty { get; set; }
        public double balanceQty { get; set; }
        public bool notified { get; set; }
        public bool collected { get; set; }
        public object binLocation { get; set; }
        public bool isAutoPayment { get; set; }
        public object paymentReference { get; set; }
        public object currencyId { get; set; }
        public double currencyRate { get; set; }
        public bool isLuggageAccepted { get; set; }
        public int module { get; set; }
        public double poQuantity { get; set; }
        public double orderedQuantity { get; set; }
        public object addOn { get; set; }
        public object purchaseOrderId { get; set; }
        public string walkInCustomerDetail { get; set; }
        public object walkInCustomer { get; set; }
        public bool isDeleted { get; set; }
        public object deleterUserId { get; set; }
        public object deletionTime { get; set; }
        public DateTime lastModificationTime { get; set; }
        public int lastModifierUserId { get; set; }
        public DateTime creationTime { get; set; }
        public int creatorUserId { get; set; }
    }
}
