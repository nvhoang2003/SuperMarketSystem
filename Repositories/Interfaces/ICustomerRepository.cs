using SuperMarketSystem.Models;

namespace SuperMarketSystem.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAll();

        Task<IEnumerable<Customer>> GetAllAsync();

        Customer GetById(int? id);

        Task<Customer> GetByIdAsync(int? id);

        Customer GetByCode(Guid code);

        Task<Customer> GetByCodeAsync(Guid code);

        void Add(Customer category);

        void Update(Customer category);

        void Remove(Customer category);

        bool Exists(int id);

        bool ExistsByCode(Guid code);

        void SaveChanges();

        Task SaveChangesAsync();
    }
}
