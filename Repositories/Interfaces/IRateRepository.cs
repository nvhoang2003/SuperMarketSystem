using SuperMarketSystem.Models;

namespace SuperMarketSystem.Repositories.Interfaces
{
    public interface IRateRepository
    {
        IEnumerable<Rate> Rates { get; } 

        IEnumerable<Rate> GetAll();

        Task<IEnumerable<Rate>> GetAllAsync();

        Rate GetById(int? id);

        Task<Rate> GetByIdAsync(int? id);

        void Add(Rate rate);

        void Update(Rate rate);
            
        void Remove(Rate rate);

        bool Exists(int id);

        void SaveChanges();

        Task SaveChangesAsync();
    }
}
