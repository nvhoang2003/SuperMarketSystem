using SuperMarketSystem.Models;

namespace SuperMarketSystem.Repositories.Interfaces
{
    public interface IShoppingCartItemRepository
    {
        ShoppingCartItem GetById(string itemId);
        Task<ShoppingCartItem> GetByIdAsync(string itemId);


        void Add(ShoppingCartItem item);
        void Update(ShoppingCartItem item);
        void Remove(ShoppingCartItem item);

        bool Exists(string itemId);
        void SaveChanges();

        Task SaveChangesAsync();
    }
}
