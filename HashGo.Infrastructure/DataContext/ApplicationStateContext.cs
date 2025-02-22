﻿using HashGo.Core.Db;
using HashGo.Core.Models;
using HashGo.Core.Models.BestTech;
using HashGo.Infrastructure;
using HashGo.Infrastructure.Models;
using Newtonsoft.Json.Linq;

namespace HashGo.Infrastructure.DataContext
{
    public static class ApplicationStateContext
    {
        public static string ServerImagePath {get;set;}
        public static List<TenantConnect> TenantConnectItems = new List<TenantConnect>();
        public static string OrderQueue = string.Empty;
        public static int DepartmentId = 0;
        public static CustomerDetails? CustomerDetailsObj = null;
        //public static string? CustomerDate = null;
        public static DateTime CustomerDate = DateTime.Now;
        public static bool IsMorningTime = false;
        public static int? deliverySlotId = 0;
        
        public static bool IsEveningTime = false;
        public static TenantConnect ConnectItem = null;
        public static readonly int IdleTimeOutInSecs = 30;
        public static SalesOrderRequest SalesOrderRequestObject { get; set; }
        public static PaymentMethod PaymentMethodObject { get; set; }
        public static string TransactionNo { get; set; }
        public static int TransactionId { get; set; }
        public static decimal NetAmountToPay { get; set; }
        public static LocationDetails LocationDetailsObj { get; set; }
        public static SalesOrderWrapper SalesOrderWrapperobj { get; set; }
        public static string NETQRImageBase64String { get;set; }
        public static string NETQRStanId { get; set; }
        public static bool IsSalesTaxInclusive { get; set; }
        public static decimal? Tax { get; set; }
        public static decimal? Deposit { get; set; }
        public static string Template { get; set; }
        public static string ReferralCode { get; set; }
        public static int NoOfUnitItems { get; set; }

        public static string BackgroundColor { get; set; }

        static ApplicationStateContext()
        {
            LoadSettings();
        }

        public static void ClearData()
        {
            CustomerDate = DateTime.Now;
            DepartmentId = 0;
            CustomerDetailsObj = null;
            IsMorningTime = false;
            IsEveningTime = false;
            SalesOrderRequestObject = null;
            PaymentMethodObject = null;
            TransactionNo = null;
            TransactionId = 0;
            NetAmountToPay = 0;
            LocationDetailsObj = null;
            SalesOrderWrapperobj = null;
            NETQRImageBase64String = null;
            IsSalesTaxInclusive = false;
            Deposit = null;
            Template = null;
        }

        public static void LoadSettings()
        {
            HashGoAppSettings.LoadSettings();
            ConnectItem = new TenantConnect()
            {
                Url = HashGoAppSettings.Url,
                User = HashGoAppSettings.User,
                Password = HashGoAppSettings.Password,
                TenantName = HashGoAppSettings.Tenant,
                DeviceId = HashGoAppSettings.DeviceId,
                LocationId = HashGoAppSettings.LocationId,
                TenantId = HashGoAppSettings.TenantId,
                SortOrder = string.IsNullOrEmpty(HashGoAppSettings.SortOrder) ? 0 : Convert.ToInt32(HashGoAppSettings.SortOrder),
                 PaymentScreenVisibleDelay = HashGoAppSettings.PaymentScreenVisibleDelay
            };
        }
    }
}
