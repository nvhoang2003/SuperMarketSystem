using Microsoft.AspNetCore.Identity;
using SuperMarketSystem.Data;
using SuperMarketSystem.Repositories.Interfaces;
using System;

namespace SuperMarketSystem.Repositories.Implements
{
    public class AdminRepository : IAdminRepository
    {
        private readonly MyDBContext _context;
        private readonly IServiceProvider _serviceProvider;

        public AdminRepository(MyDBContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }
        public void ClearDatabase()
        {
            var rates = _context.Rates.ToList();
            _context.Rates.RemoveRange(rates);

            var shoppingCartItems = _context.ShoppingCartItems.ToList();
            _context.ShoppingCartItems.RemoveRange(shoppingCartItems);

            var users = _context.Users.ToList();
            var userRoles = _context.UserRoles.ToList();

            foreach (var user in users)
            {
                if (!userRoles.Any(r => r.UserId == user.Id))
                {
                    _context.Users.Remove(user);
                }
            }

            var orderDetails = _context.OrderDetails.ToList();
            _context.OrderDetails.RemoveRange(orderDetails);

            var orders = _context.Orders.ToList();
            _context.Orders.RemoveRange(orders);

            var pizzas = _context.Products.ToList();
            _context.Products.RemoveRange(pizzas);

            var categories = _context.Categories.ToList();
            _context.Categories.RemoveRange(categories);

            _context.SaveChanges();
        }

        public void SeedDatabase()
        {
            var _roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var _userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        }
    }
}
