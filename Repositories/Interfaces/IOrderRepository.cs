using SuperMarketSystem.Models;

namespace SuperMarketSystem.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order);
        Order GetById(int? id);
        Task<Order> GetByIdAsync(int? id);

        void Add(Order order);
        void Update(Order order);
        void Remove(Order order);

        bool Exists(int id);
        void SaveChanges();

        Task SaveChangesAsync();
    }
}
