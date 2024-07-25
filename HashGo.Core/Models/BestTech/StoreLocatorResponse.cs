using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models.BestTech
{
    public class StoreLocatorResponse
    {
        public Result result { get; set; }
        public object targetUrl { get; set; }
        public bool success { get; set; }
        public object error { get; set; }
        public bool unAuthorizedRequest { get; set; }
        public bool __abp { get; set; }

    }

    public class Result
    {
        public List<StoreLocators> items { get; set; }
    }


    public class StoreLocators
    {
       
        public string code { get; set; }
        public string name { get; set; }
        public string companyName { get; set; }
        public int companyId { get; set; }
        public object locationGroupId { get; set; }
        public object menuId { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string city { get; set; }
        public object state { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
        public string website { get; set; }
        public string email { get; set; }
        public bool isPurchaseAllowed { get; set; }
        public bool isSalesAllowed { get; set; }
        public string remarks { get; set; }
        public bool iswareHouse { get; set; }
        public bool isDeleted { get; set; }
        public object deleterUserId { get; set; }
        public object deletionTime { get; set; }
        public DateTime lastModificationTime { get; set; }
        public int lastModifierUserId { get; set; }
        public DateTime creationTime { get; set; }
        public object creatorUserId { get; set; }
        public int id { get; set; }
    }
}
