using HashGo.Core.Models;
using HashGo.Core.Models.BestTech;

namespace HashGo.Core.Contracts.Services
{
    public interface IRetailConnectService : IService
    {
        void TryLogin(string tenancyName, string usernameOrEmailAddress, string password);
        Task<IReadOnlyCollection<Department>> GetAllDepartments();

        Task<IReadOnlyCollection<Category>> GetCategoryiesByDepartmentId(int deptId);

        Task<IReadOnlyCollection<SubCategory>> GetSubCategoriesByCategoryId(int catId);
        Task<IReadOnlyCollection<ServiceUnit>> GetProductsByCategoryId(int categoryId);
        Task<IReadOnlyCollection<DeliveryTimings>> DeliveryTimingByDept(int departmentId);
        Task<int> BalanceSlotByDeliveryTiming(int departmentId, int deliveryTimingId);

        Task<TransactionDetails> CreateSalesOrderWithPayment(SalesOrderRequest saleOrder);

        Task<IReadOnlyCollection<ServiceUnit>> GetProductsByCategoryAndSubcategoryId(int categoryId, int subCtgryId);

        Task<bool> CreateEnquiryRequest(EnquiriesRequestObject enquiriesRequest);

        Task<string> GetCompanyLogo(string LocationId);

        Task<CompanyImage?> GetCompanyBackgroundImage(string companyId);

        Task<IReadOnlyCollection<StoreLocators>> GetStoreLocations();

        Task<LocationDetails> GetLocationDetails();

        Task<SalesOrderWrapper> GetSalesOrderForEdit();
        Task<TenantSettingsWrapper> GetAllSettings();

        Task<TemplateReceiptResponse> GetTemplateReceiptResponse();
    }
}
