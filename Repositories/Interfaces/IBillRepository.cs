using SuperMarketSystem.Models;

namespace SuperMarketSystem.Repositories.Interfaces
{
    public interface IBillRepository
    {
        IEnumerable<Bill> GetAll();

        Task<IEnumerable<Bill>> GetAllAsync();

        Bill GetById(int? id);

        Task<Bill> GetByIdAsync(int? id);

        void Add(Bill bill);

        void Update(Bill bill);

        void Remove(Bill bill);

        bool Exists(int id);

        void SaveChanges();

        Task SaveChangesAsync();
    }
}
