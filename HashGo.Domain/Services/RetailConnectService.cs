using HashGo.Core.Contracts.Services;
using HashGo.Core.Contracts.StoreService;
using HashGo.Core.Models;
using HashGo.Core.Models.BestTech;
using HashGo.Domain.DataContext;
using HashGo.Domain.Models.Base;
using HashGo.Infrastructure;
using HashGo.Infrastructure.Common;
using HashGo.Infrastructure.DataContext;
using HashGo.Infrastructure.HttpHelper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Category = HashGo.Core.Models.BestTech.Category;

namespace HashGo.Domain.Services
{
    public class RetailConnectService : IRetailConnectService
    {
        private readonly ITenantConnectStoreService tenantConnectStoreService;
        private readonly ILoggingService logger;
        private const string DATA = @"{""object"":{""name"":""Name""}}";
        private readonly IQueueSettingStoreService queueSettingDetailService;


        public RetailConnectService(ILoggingService logger, ITenantConnectStoreService tenantConnectStoreService, IQueueSettingStoreService queueSettingService)
        {
            this.logger = logger;
            this.tenantConnectStoreService = tenantConnectStoreService;
            this.queueSettingDetailService = queueSettingService;
            HashGoAppSettings.LoadSettings();

            if(!string.IsNullOrEmpty(HashGoAppSettings.Url) &&
                !string.IsNullOrEmpty(HashGoAppSettings.User) &&
                !string.IsNullOrEmpty(HashGoAppSettings.Password) &&
                !string.IsNullOrEmpty(HashGoAppSettings.Tenant))
            {
                this.Login(HttpHelper.GetInstance(), HashGoAppSettings.Tenant, HashGoAppSettings.User, HashGoAppSettings.Password);
            }
            else
            {
                logger.Info("Please provide valid settings inorder to connect to the service.");
            }
        }

        public void Login(HttpHelper Instance, string _tenancyName, string _usernameOrEmailAddress, string _password)
        {
            try
            {
                string? responeString = Instance.Post(
                    JsonConvert.SerializeObject(new
                    {
                        tenancyName = _tenancyName,
                        usernameOrEmailAddress = _usernameOrEmailAddress,
                        password = _password
                    }),
                    ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.LOGIN);
                BaseResponse<string>? result = JsonConvert.DeserializeObject<BaseResponse<string>>(responeString);
                if (result != null && result.Success)
                {
                    HttpHelper.GetInstance().SetToken(result.Result);
                }
            }
            catch (Exception ex)
            {
                logger.TraceException(ex);
            }
        }


        public async Task<IReadOnlyCollection<Department>> GetAllDepartments()
        {
            // Todo make sure login is done and token is set
            List<Department> departments = new List<Department>();
            try
            {
                var client = HttpHelper.GetInstance();
                if (client == null) return departments;

                string? responeString = client.Post(
                        JsonConvert.SerializeObject(new
                        {
                            tenantId = ApplicationStateContext.ConnectItem.TenantId,
                            LastSyncDateTime = "",
                        }),
                        //AppConfigKeys.RETAIL_CONNECT_URL+ RetailConnectApiRouterNames.GET_ALL_DEPARTMENT);
                        ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.GET_ALL_DEPARTMENT);

                DepartmentRoot result = JsonConvert.DeserializeObject<DepartmentRoot>(responeString);

                if (result != null && result.success && result.result.items != null)
                {
                    return (departments = result.result.items);
                }
            }
            catch (Exception ex)
            {
                logger.TraceException(ex);
            }

            return departments;
        }

        public async Task<IReadOnlyCollection<Category>> GetCategoryiesByDepartmentId(int deptId)
        {
            try
            {
                var client = HttpHelper.GetInstance();

                if (client == null) throw new Exception("Unable to create HttpClient.");

                string? responeString = client.Post(
                       JsonConvert.SerializeObject(new
                       {
                           departmentId = deptId,
                       }),
                         ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.GET_CATEGORIES_BY_DEPTID);

                Base<Category> result = JsonConvert.DeserializeObject<Base<Category>>(responeString);

                if (result != null && result.success && result.result.items != null)
                {
                    return result.result.items;
                }
            }
            catch (Exception ex)
            {
                logger.TraceException(ex);
            }

            return new List<Category>();
        }

        public async Task<IReadOnlyCollection<SubCategory>> GetSubCategoriesByCategoryId(int catId)
        {
            try
            {
                var client = HttpHelper.GetInstance();

                if (client == null) throw new Exception("Unable to create HttpClient.");

                string? responeString = client.Post(
                       JsonConvert.SerializeObject(new
                       {
                           Categoryid  = catId,
                       }),
                       ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.GET_SUBCATEGORIES_BY_CATID);

                Base<SubCategory> result = JsonConvert.DeserializeObject<Base<SubCategory>>(responeString);

                if (result != null && result.success && result.result.items != null)
                {
                    return result.result.items.OrderBy(ee=>ee.id).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.TraceException(ex);
            }

            return new List<SubCategory>();
        }

        public async Task<IReadOnlyCollection<ServiceUnit>> GetProductsByCategoryId(int categoryId)
        {
            try
            {
                var client = HttpHelper.GetInstance();

                if (client == null) throw new Exception("Unable to create HttpClient.");

                string? responeString = client.Post(
                       JsonConvert.SerializeObject(new
                       {
                           categoryId = categoryId,
                       }),
                       ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.GET_PRODUCTS_BY_CATEGORY_ID);

                Base<ServiceUnit> result = JsonConvert.DeserializeObject<Base<ServiceUnit>>(responeString);

                if (result != null && result.success && result.result.items != null)
                {
                    return result.result.items;
                }
            }
            catch (Exception ex)
            {
                logger.TraceException(ex);
            }

            return new List<ServiceUnit>();
        }

        public async Task<int> BalanceSlotByDeliveryTiming(int departmentId, int deliveryTimingId)
        {
            try
            {
                var client = HttpHelper.GetInstance();

                if (client == null) throw new Exception("Unable to create HttpClient.");

                string? responeString = client.Post(
                       JsonConvert.SerializeObject(new
                       {
                           departmentId = departmentId,
                           deliveryTimingId = deliveryTimingId,
                           date = ApplicationStateContext.CustomerDate.ToString("yyyy-MM-dd"),
                       }),
                       ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.GET_DELIVERYTIMINGBALSLOT);

                DeliveryTimingSlotResponse result = JsonConvert.DeserializeObject<DeliveryTimingSlotResponse>(responeString);

                if (result != null && result.success)
                {
                    return result.result.availableSlots;
                }
            }
            catch (Exception ex)
            {
                logger.TraceException(ex);
            }

            return 0;
        }


        public async Task<IReadOnlyCollection<DeliveryTimings>> DeliveryTimingByDept(int departmentId)
        {
            try
            {
                var client = HttpHelper.GetInstance();

                if (client == null) throw new Exception("Unable to create HttpClient.");

                string? responeString = client.Post(
                       JsonConvert.SerializeObject(new
                       {
                           departmentId = departmentId,
                       }),
                       ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.GET_DELIVERYTIMINGBYDEPT);

                Base<DeliveryTimings> result = JsonConvert.DeserializeObject<Base<DeliveryTimings>>(responeString);

                if (result != null && result.success && result.result.items != null)
                {
                    return result.result.items;
                }
            }
            catch (Exception ex)
            {
                logger.TraceException(ex);
            }

            return new List<DeliveryTimings>();
        }



        public async Task<TransactionDetails> CreateSalesOrderWithPayment(SalesOrderRequest saleOrder)
        {
            try
            {
                saleOrder.salesOrder.type = "I"; 
                var client = HttpHelper.GetInstance();

                if (client == null) throw new Exception("Unable to create HttpClient.");
                var vv = JsonConvert.SerializeObject(saleOrder);
                string? responeString = client.Post(
                       JsonConvert.SerializeObject(saleOrder),

                       ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.POST_SALEORDER);

                SalesOrderResponse result = JsonConvert.DeserializeObject<SalesOrderResponse>(responeString);

                if (result != null && result.success && result.result != null)
                {
                    return result.result;
                }
            }
            catch (Exception ex)
            {
                logger.TraceException(ex);
            }

            return null;
        }

        public void TryLogin(string tenancyName, string usernameOrEmailAddress, string password)
        {
            this.Login(HttpHelper.GetInstance(), HashGoAppSettings.Tenant, HashGoAppSettings.User, HashGoAppSettings.Password);
        }

        public async Task<IReadOnlyCollection<ServiceUnit>> GetProductsByCategoryAndSubcategoryId(int categoryId, int subCtgryId)
        {
            try
            {
                var client = HttpHelper.GetInstance();

                if (client == null) throw new Exception("Unable to create HttpClient.");

                string? responeString = client.Post(
                       JsonConvert.SerializeObject(new
                       {
                           categoryId = categoryId,
                           subCategoryId = subCtgryId,
                       }),
                       ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.GET_PRODUCTS_BY_CTGRYANDSUBCATEGORY_ID);

                Base<ServiceUnit> result = JsonConvert.DeserializeObject<Base<ServiceUnit>>(responeString);

                 if (result != null && result.success && result.result.items != null)
                {
                    return result.result.items;
                }
            }
            catch (Exception ex)
            {
                logger.TraceException(ex);
            }

            return new List<ServiceUnit>();
        }

        public async Task<bool> CreateEnquiryRequest(EnquiriesRequestObject enquiriesRequest)
        {
            try
            {
                var client = HttpHelper.GetInstance();

                if (client == null) throw new Exception("Unable to create HttpClient.");

                string? responeString = client.Post(
                       JsonConvert.SerializeObject(new
                       {
                           enquiriesRequest.enquiry
                       }),
                       ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.CREATE_ENQUIRY);

                EnquiriesResponseObject result = JsonConvert.DeserializeObject<EnquiriesResponseObject>(responeString);

                if (result != null && result.success && result.result != null)
                {
                    return true;
                }
            }
            catch(Exception ex) { logger.TraceException(ex);}

            return  false;
        }

        public async Task<string> GetCompanyLogo(string locationId)
        {
            try
            {
                var client = HttpHelper.GetInstance();

                if (client == null) throw new Exception("Unable to create HttpClient.");

                string? responeString = client.Post(
                       JsonConvert.SerializeObject(new
                       {
                           locationId
                       }),
                       ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.GET_COMPANY_LOGO);

                CompanyLogoResponseObject result = JsonConvert.DeserializeObject<CompanyLogoResponseObject>(responeString);

                if (result != null && result.success && !string.IsNullOrEmpty(result.result))
                {
                    return result.result;
                }
            }
            catch (Exception ex) { logger.TraceException(ex); }

            return CommonConstants.DEFAULTIMAGE;
        }

        public async Task<CompanyImage?> GetCompanyBackgroundImage(string companyId)
        {
            try
            {
                var client = HttpHelper.GetInstance();

                if (client == null) throw new Exception("Unable to create HttpClient.");

                string? responeString = client.Post(
                       JsonConvert.SerializeObject(new
                       {
                           companyId
                       }),
                       ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.GET_COMPANY_BACKGROUND_IMAGE);

                CompanyImageResponse result = JsonConvert.DeserializeObject<CompanyImageResponse>(responeString);

                if (result != null && result.success)
                {
                    return result.result;
                }
            }
            catch (Exception ex) { logger.TraceException(ex); }

            return null;
        }

        public async Task<IReadOnlyCollection<StoreLocators>> GetStoreLocations()
        {
            try
            {
                var client = HttpHelper.GetInstance();

                if (client == null) throw new Exception("Unable to create HttpClient.");

                string? responeString = client.Post(
                       JsonConvert.SerializeObject(new
                       {
                           tenantId = ApplicationStateContext.ConnectItem.TenantId,
                           LastSyncDateTime = "",
                       }),
                       ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.GET_STORELOCATORS);

                StoreLocatorResponse result = JsonConvert.DeserializeObject<StoreLocatorResponse>(responeString);

                if (result != null && result.success && result.result != null)
                {
                    return result.result.items;
                }
            }
            catch (Exception ex) { logger.TraceException(ex); }

            return default(IReadOnlyCollection<StoreLocators>);
        }

        public async Task<LocationDetails> GetLocationDetails()
        {
            try
            {
                var client = HttpHelper.GetInstance();

                if (client == null) throw new Exception("Unable to create HttpClient.");

                string? responeString = client.Post(
                       JsonConvert.SerializeObject(new
                       {
                           id= HashGoAppSettings.LocationId
                       }),
                       ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.GET_LOCATIONDETAILS);

                LocationDetailsResponse result = JsonConvert.DeserializeObject<LocationDetailsResponse>(responeString);

                if (result != null && result.success && result.result != null)
                {
                    return result.result.location;
                }
            }
            catch (Exception ex) { logger.TraceException(ex); }

            return null;
        }

        public async Task<SalesOrderWrapper> GetSalesOrderForEdit()
        {
            try
            {
                var client = HttpHelper.GetInstance();

                if (client == null) throw new Exception("Unable to create HttpClient.");

                string? responeString = client.Post(
                       JsonConvert.SerializeObject(new
                       {
                           id = ApplicationStateContext.TransactionId
                       }),
                       ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.GET_SALESORDER);

                SalesOrderForEditResponse result = JsonConvert.DeserializeObject<SalesOrderForEditResponse>(responeString);

                if (result != null && result.success && result.result != null)
                {
                    return result.result;
                }
            }
            catch (Exception ex) { logger.TraceException(ex); }

            return null;
        }

        public async Task<TenantSettingsWrapper> GetAllSettings()
        {
            try
            {
                var client = HttpHelper.GetInstance();

                if (client == null) throw new Exception("Unable to create HttpClient.");

                string? responeString = client.Post("",
                       ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.GET_ALLSETTINGS);

                TenantSettingsResponse result = JsonConvert.DeserializeObject<TenantSettingsResponse>(responeString);

                if (result != null && result.success && result.result != null)
                {
                    return result.result;
                }
            }
            catch (Exception ex) { logger.TraceException(ex); }

            return null;
        }

        public async Task<TemplateReceiptResponse> GetTemplateReceiptResponse()
        {
            try
            {
                var client = HttpHelper.GetInstance();

                if (client == null) throw new Exception("Unable to create HttpClient.");

                string? responeString = client.Post(
                       JsonConvert.SerializeObject(new
                       {
                           Type = 13,  //doubt -- whether this value is hardcoded always or need to read from anywhere?
                           locationId = HashGoAppSettings.LocationId
                       }),
                       ApplicationStateContext.ConnectItem.Url + RetailConnectApiRouterNames.GET_RECEIPTTEMPLATEBYLOCATION);

                TemplateReceiptResponse result = JsonConvert.DeserializeObject<TemplateReceiptResponse>(responeString);

                if (result != null && result.success && result.result != null)
                {
                    return result;
                }
            }
            catch (Exception ex) { logger.TraceException(ex); }

            return null;
        }
    }
}
