using SuperMarketSystem.Models;
using SuperMarketSystem.Services.ShoppingCart;

namespace SuperMarketSystem.ViewModels
{
    public class ShoppingCartViewModel
    {
        public ShoppingCartService ShoppingCart { get; set; }
        public decimal ShoppingCartTotal { get; set; }
    }
}
