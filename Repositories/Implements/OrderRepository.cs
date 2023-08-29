using Microsoft.EntityFrameworkCore;
using SuperMarketSystem.Data;
using SuperMarketSystem.Models;
using SuperMarketSystem.Repositories.Interfaces;
using SuperMarketSystem.Services.ShoppingCart;
using System;

namespace SuperMarketSystem.Repositories.Implements
{
    public class OrderRepository : IOrderRepository
    {

        private readonly MyDBContext _context;
        private readonly ShoppingCartService _shoppingCart;


        public OrderRepository(MyDBContext context, ShoppingCartService shoppingCart)
        {
            _context = context;
            _shoppingCart = shoppingCart;
        }
        #region Get By Id
        public Order GetById(int? id)
        {
            return _context.Orders.FirstOrDefault(p => p.OrderId == id);
        }
        public async Task<Order> GetByIdAsync(int? id)
        {
            return await _context.Orders.FirstOrDefaultAsync(p => p.OrderId == id);
        }
        #endregion

        #region Create Oder
        public async Task CreateOrderAsync(Order order)
        {
            order.DateOfPurchase = DateTime.Now;
            decimal totalPrice = 0M;

            var shoppingCartItems = _shoppingCart.ShoppingCartItems;

            foreach (var shoppingCartItem in shoppingCartItems)
            {
                var orderDetail = new OrderDetails()
                {
                    Amount = shoppingCartItem.Amount,
                    ProductId = shoppingCartItem.Product.Id,                  
                    Order = order,
                    Price = shoppingCartItem.Product.UnitCost,

                };
                totalPrice += orderDetail.Price * orderDetail.Amount;
                _context.OrderDetails.Add(orderDetail);
            }

            order.OrderTotal = totalPrice;
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();
        }

        #endregion

        #region Crate
        public void Add(Order order)
        {
            _context.Orders.Add(order);
        }
        #endregion

        #region Update
        public void Update(Order order)
        {
            _context.Orders.Update(order);
        }
        #endregion

        #region Delete

        public void Remove(Order order)
        {
            _context.Orders.Remove(order);
        }
        #endregion

        #region Exists
        public bool Exists(int id)
        {
            return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
        #endregion

        #region Save Change
        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
