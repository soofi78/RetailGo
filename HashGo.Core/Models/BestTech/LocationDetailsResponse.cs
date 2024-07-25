using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models.BestTech
{
    public class LocationDetailsResponse
    {
        public LocationDetailsWrapper result { get; set; }
        public object targetUrl { get; set; }
        public bool success { get; set; }
        public object error { get; set; }
        public bool unAuthorizedRequest { get; set; }
        public bool __abp { get; set; }
    }

    public class LocationDetails
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public int companyId { get; set; }
        public object locationGroupId { get; set; }
        public object menuId { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public object address3 { get; set; }
        public object city { get; set; }
        public object state { get; set; }
        public string country { get; set; }
        public string postalCode { get; set; }
        public string phone { get; set; }
        public object fax { get; set; }
        public object website { get; set; }
        public object email { get; set; }
        public bool isPurchaseAllowed { get; set; }
        public bool isSalesAllowed { get; set; }
        public int tenantId { get; set; }
        public bool isNewTenant { get; set; }
        public string companyName { get; set; }
        public object businessRegistrationNo { get; set; }
        public string imagePath { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public object autoAcceptStockTransfer { get; set; }
        public object autoApproveStockRequest { get; set; }
        public object companyCountry { get; set; }
        public object companyPostalCode { get; set; }
        public object isCustomerByLocation { get; set; }
        public object priceGroupId { get; set; }
        public object remarks { get; set; }
        public object webSite { get; set; }
        public bool isWarehouse { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
    }

    public class LocationDetailsWrapper
    {
        public LocationDetails location { get; set; }
    }
}
