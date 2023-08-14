using DataAccessLayer.DataObject;

namespace SuperMarketSystem.Repositories.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }
        IEnumerable<Product> IsProductOfTheWeek { get; }

        Product GetById(int? id);
        Task<Product> GetByIdAsync(int? id);

        Product GetByIdIncluded(int? id);
        Task<Product> GetByIdIncludedAsync(int? id);

        bool Exists(int id);

        IEnumerable<Product> GetAll();
        Task<IEnumerable<Product>> GetAllAsync();

        IEnumerable<Product> GetAllIncluded();
        Task<IEnumerable<Product>> GetAllIncludedAsync();

        void Add(Product product);
        void Update(Product product);
        void Remove(Product product);

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
