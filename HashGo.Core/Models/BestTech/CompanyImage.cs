namespace HashGo.Core.Models.BestTech
{
    public class CompanyImageResponse
    {
        public CompanyImage result { get; set; }
        public object targetUrl { get; set; }
        public bool success { get; set; }
        public object error { get; set; }
        public bool unAuthorizedRequest { get; set; }
        public bool __abp { get; set; }
    }


    public class CompanyImage
    {
        public string backgroundImage { get; set; }

        public string backgroundColor { get; set; }

        public int rowOfItems { get; set; }
    }

}
