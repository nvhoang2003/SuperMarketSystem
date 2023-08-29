using SuperMarketSystem.Models;

namespace SuperMarketSystem.Repositories.Interfaces
{
    public interface IOrderDetailRepository
    {
        OrderDetails GetById(int? id);
        Task<OrderDetails> GetByIdAsync(int? id);

        void Add(OrderDetails orderDetails);
        void Update(OrderDetails orderDetails);
        void Remove(OrderDetails orderDetails);

        bool Exists(int id);
        void SaveChanges();

        Task SaveChangesAsync();
    }
}
