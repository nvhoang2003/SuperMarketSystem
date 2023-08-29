using SuperMarketSystem.Models;

namespace SuperMarketSystem.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll();

        Task<IEnumerable<Category>> GetAllAsync();

        Category GetById(int? id);

        Task<Category> GetByIdAsync(int? id);

        Category GetByCode(Guid code);

        Task<Category> GetByCodeAsync(Guid code);

        void Add(Category category);

        void Update(Category category);

        void Remove(Category category);

        bool Exists(int id);

        bool ExistsByCode(Guid code);

        void SaveChanges();

        Task SaveChangesAsync();
    }
}
