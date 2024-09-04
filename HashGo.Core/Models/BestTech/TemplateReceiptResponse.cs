using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Models.BestTech
{
    public class TemplateReceiptResponse
    {
        public List<TemplateReceipt> result { get; set; }
        public object targetUrl { get; set; }
        public bool success { get; set; }
        public object error { get; set; }
        public bool unAuthorizedRequest { get; set; }
        public bool __abp { get; set; }
    }

    public class TemplateReceipt
    {
        public string name { get; set; }
        public string template { get; set; }
        public int type { get; set; }
        public bool group { get; set; }
        public string locations { get; set; }
        public string receiptTypeName { get; set; }
        public int tenantId { get; set; }
        public bool isDeleted { get; set; }
        public object deleterUserId { get; set; }
        public object deletionTime { get; set; }
        public object lastModificationTime { get; set; }
        public object lastModifierUserId { get; set; }
        public DateTime creationTime { get; set; }
        public int creatorUserId { get; set; }
        public int id { get; set; }
    }
}
