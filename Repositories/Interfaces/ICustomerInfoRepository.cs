using SuperMarketSystem.Models;

namespace SuperMarketSystem.Repositories.Interfaces
{
    public interface ICustomerInfoRepository
    {
        CustomerInfo GetById(int? id);

        Task<CustomerInfo> GetByIdAsync(int? id);

        void Add(CustomerInfo info);

        void Update(CustomerInfo info);

        void Remove(CustomerInfo info);

        bool Exists(int id);

        void SaveChanges();

        Task SaveChangesAsync();
    }
}
