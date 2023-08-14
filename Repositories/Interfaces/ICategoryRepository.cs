using DataAccessLayer.DataObject;

namespace SuperMarketSystem.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAll();
        Task<Category> GetById(int? id);
        void Add(Category category);
        void Update(Category category);
        void Remove(Category category);
        bool Exists(int id);
        Task SaveChangesAsync();
    }
}
