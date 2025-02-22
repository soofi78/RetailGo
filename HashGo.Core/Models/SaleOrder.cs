﻿using System.Xml.Serialization;

namespace HashGo.Core.Models
{
    //public class SaleOrder
    //{   
    //    //public string Date { get; set; }
    //    public DateTime Date { get; set; }

    //    public string Name { get; set; }

    //    public string ReferralCode { get; set; }

    //    public string ContactNo { get; set; }

    //    public string Address { get; set; }

    //    public string PostalCode { get; set; }

    //    public string UnitName { get; set; }

    //    public string FloorNumber { get; set; }

    //    public int? PaymentTypeId { get; set; }

    //    public decimal NetTotal { get; set; }

    //    public int LocationId { get; set; }


    //    public List<SaleOrderDetail> SaleOrderDetails { get; set; }
    //}

    public class SalesOrderRequest
    {
        public SalesOrder salesOrder { get; set; }
        public List<SalesOrderDetail> salesOrderDetail { get; set; }
    }

    public class SalesOrder
    {
        public DateTime date { get; set; }
        public string type { get; set; }
        public decimal subTotal { get; set; }
        public decimal tax { get; set; }
        public decimal netTotal { get; set; }
        public decimal paidAmount { get; set; }
        public int locationId { get; set; }
        public string name { get; set; }
        public object referralCode { get; set; }
        public string Remarks { get; set; }
        public string contactNo { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string unitName { get; set; }
        public string postalCode { get; set; }
        public string email { get; set; }
        public string floorNumber { get; set; }
        public int? DeliveryTimingId { get; set; }

    }

    public class SalesOrderDetail
    {
        public int productId { get; set; }
        public int taxId { get; set; }
        public int unitId { get; set; }
        public decimal price { get; set; }
        public decimal subTotal { get; set; }
        public decimal tax { get; set; }
        public decimal netTotal { get; set; }
        public int qty { get; set; }
        public string productName { get; set; }
    }
}
