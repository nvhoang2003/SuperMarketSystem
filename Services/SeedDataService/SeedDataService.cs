using Microsoft.AspNetCore.Identity;
using SuperMarketSystem.Data;

namespace SuperMarketSystem.Services.SeedDataService
{

    public class SeedDataService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public SeedDataService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public async Task SeedRolesAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "admin", "customer" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }
        }

        public async Task SeedAdminUserAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                string email = "admin123@gmail.com";
                string password = _configuration["SeedUserPW"];

                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new ApplicationUser()
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        RoleType = "admin"
                    };
                    var userResult = await userManager.CreateAsync(user, password);

                    if (userResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "admin");
                    }
                }
            }
        }
        private async Task SeedDatabase(MyDBContext _context, RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager)
        {
        }
        public async Task ClearDatabase(MyDBContext _context)
        {
            var users = _context.Users.ToList();
            var userRoles = _context.UserRoles.ToList();

            foreach (var user in users)
            {
                if (!userRoles.Any(r => r.UserId == user.Id))
                {
                    _context.Users.Remove(user);
                }
            }
            var bills = _context.Bills.ToList();
            _context.Bills.RemoveRange(bills);

            var rates = _context.Rates.ToList();
            _context.Rates.RemoveRange(rates);

            var shoppingCartItems = _context.ShoppingCartItems.ToList();
            _context.ShoppingCartItems.RemoveRange(shoppingCartItems);
            var orderDetails = _context.OrderDetails.ToList();
            _context.OrderDetails.RemoveRange(orderDetails);

            var orders = _context.Orders.ToList();
            _context.Orders.RemoveRange(orders);

            var products = _context.Products.ToList();
            _context.Products.RemoveRange(products);

            var categories = _context.Categories.ToList();
            _context.Categories.RemoveRange(categories);

            _context.SaveChanges();
        }
    }
}

