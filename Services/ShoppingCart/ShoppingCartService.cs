using Microsoft.EntityFrameworkCore;
using SuperMarketSystem.Data;
using SuperMarketSystem.Models;

namespace SuperMarketSystem.Services.ShoppingCart
{
    public class ShoppingCartService
    {
        public string ShoppingCartId { get; set; }
        private readonly MyDBContext _context = new MyDBContext();
        public const string CartSessionKey = "CartId";
        public ShoppingCartService(MyDBContext context)
        {
            _context = context;
        }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public async Task AddToCartAsync(Product product, int amount)
        {
            var shoppingCartItem =
                    await _context.ShoppingCartItems.SingleOrDefaultAsync(
                        s => s.Product.Id == product.Id && s.ItemId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ItemId = ShoppingCartId,
                    Product = product,
                    Amount = 1,
                    DateCreated = DateTime.UtcNow,
                };

                _context.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            await _context.SaveChangesAsync();
        }
        public static ShoppingCartService GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            var context = services.GetService<MyDBContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);

            return new ShoppingCartService(context) { ShoppingCartId = cartId };
        }
        public async Task<int> RemoveFromCartAsync(Product product)
        {
            var shoppingCartItem =
                    await _context.ShoppingCartItems.SingleOrDefaultAsync(
                        s => s.Product.Id == product.Id && s.ItemId == ShoppingCartId);

            var localQuantity = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localQuantity = shoppingCartItem.Amount;
                }
                else
                {
                    _context.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            await _context.SaveChangesAsync();

            return localQuantity;
        }
        public async Task<List<ShoppingCartItem>> GetShoppingCartItemsAsync()
        {
            return ShoppingCartItems ??
                   (ShoppingCartItems = await
                       _context.ShoppingCartItems.Where(c => c.ItemId == ShoppingCartId)
                           .Include(s => s.Product)
                           .ToListAsync());
        }

        public async Task ClearCartAsync()
        {
            var cartItems = _context
                .ShoppingCartItems
                .Where(cart => cart.ItemId == ShoppingCartId);

            _context.ShoppingCartItems.RemoveRange(cartItems);

            await _context.SaveChangesAsync();
        }

        public decimal GetShoppingCartTotal()
        {
            var total = _context.ShoppingCartItems.Where(c => c.ItemId == ShoppingCartId)
                .Select(c => c.Product.UnitCost * c.Amount).Sum();
            return total;
        }
    }
}
