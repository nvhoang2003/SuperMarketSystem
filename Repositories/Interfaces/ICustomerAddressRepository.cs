using SuperMarketSystem.Models;

namespace SuperMarketSystem.Repositories.Interfaces
{
    public interface ICustomerAddressRepository
    {
        CustomerAddress GetById(int? id);

        Task<CustomerAddress> GetByIdAsync(int? id);

        void Add(CustomerAddress address);

        void Update(CustomerAddress address);

        void Remove(CustomerAddress address);

        bool Exists(int id);

        void SaveChanges();

        Task SaveChangesAsync();
    }
}
