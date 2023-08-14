using DataAccessLayer.DataObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SuperMarketSystem.Data;
using System;

namespace SuperMarketSystem.Models
{
    public class ShoppingCart
    {
        private readonly MyDBContext _context;

        private ShoppingCart(MyDBContext context)
        {
            _context = context;
        }

        public string ShoppingCartId { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }


        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            var context = services.GetService<MyDBContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

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
                    Quantity = 1,
                    DateCreated = DateTime.UtcNow,
                };

                _context.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Quantity++;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveFromCartAsync(Product product)
        {
            var shoppingCartItem =
                    await _context.ShoppingCartItems.SingleOrDefaultAsync(
                        s => s.Product.Id == product.Id && s.ItemId == ShoppingCartId);

            var localQuantity = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Quantity > 1)
                {
                    shoppingCartItem.Quantity--;
                    localQuantity = shoppingCartItem.Quantity;
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

        public float GetShoppingCartTotal()
        {
            var total = _context.ShoppingCartItems.Where(c => c.ItemId == ShoppingCartId)
                .Select(c => c.Product.UnitCost * c.Quantity).Sum();
            return total;
        }
    }
}
