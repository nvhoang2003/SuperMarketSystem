using SuperMarketSystem.Models;

namespace SuperMarketSystem.Repositories.Interfaces
{
    public interface IRateRepository
    {
        IEnumerable<Rate> Rates { get; }

        Rate GetById(int? id);
        Task<Rate> GetByIdAsync(int? id);

        bool Exists(int id);

        IEnumerable<Rate> GetAll();
        Task<IEnumerable<Rate>> GetAllAsync();

        void Add(Rate review);
        void Update(Rate review);
        void Remove(Rate review);

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
