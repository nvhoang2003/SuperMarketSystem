using SuperMarketSystem.Models;

namespace SuperMarketSystem.Repositories.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> IsProductOfTheWeek { get; } 

        IEnumerable<Product> GetAll();

        Task<IEnumerable<Product>> GetAllAsync();

        IEnumerable<Product> GetAllIncluded();

        Task<IEnumerable<Product>> GetAllIncludedAsync();

        Product GetById(int? id);

        Task<Product> GetByIdAsync(int? id);

        Product GetByCode(Guid code);

        Task<Product> GetByCodeAsync(Guid code);

        Product GetByIdIncluded(int? id);

        Task<Product> GetByIdIncludedAsync(int? id);

        Product GetByCodeIncluded(Guid code);

        Task<Product> GetByCodeIncludedAsync(Guid code);

        void Add(Product product);

        void Update(Product product);

        void Remove(Product product);

        bool Exists(int id);

        bool ExistsByCode(Guid code); 

        void SaveChanges();

        Task SaveChangesAsync();
    }
}
