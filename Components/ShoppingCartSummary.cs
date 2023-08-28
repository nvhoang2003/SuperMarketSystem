using Microsoft.AspNetCore.Mvc;
using SuperMarketSystem.Services.ShoppingCart;
using SuperMarketSystem.ViewModels;

namespace SuperMarketSystem.Components
{
    public class ShoppingCartSummary : ViewComponent
    {
        private readonly ShoppingCartService _shoppingCart;
        public ShoppingCartSummary(ShoppingCartService shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _shoppingCart.GetShoppingCartItemsAsync();
            _shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };
            return View(shoppingCartViewModel);
        }
    }
}
