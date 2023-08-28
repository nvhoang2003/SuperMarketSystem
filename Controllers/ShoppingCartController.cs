using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperMarketSystem.Data;
using SuperMarketSystem.Models;
using SuperMarketSystem.Repositories.Interfaces;
using SuperMarketSystem.Services.ShoppingCart;
using SuperMarketSystem.ViewModels;
using System;

namespace SuperMarketSystem.Controllers
{
    public class ShoppingCartController : Controller
    {
        #region Fields
        private readonly IProductRepository _productRepository;
        private readonly MyDBContext _context;
        private readonly ShoppingCartService _shoppingCart;
        #endregion

        #region Constructor
        public ShoppingCartController(IProductRepository productRepository,
            ShoppingCartService shoppingCart, MyDBContext context)
        {
            _productRepository = productRepository;
            _shoppingCart = shoppingCart;
            _context = context;
        }
        #endregion

        #region Index
        [AllowAnonymous]
        public async Task<IActionResult> Index()
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
        #endregion

        #region AddToCart
        [AllowAnonymous]
        public async Task<IActionResult> AddToShoppingCart(int productId)
        {
            var selectedProduct = await _productRepository.GetByIdAsync(productId);

            if (selectedProduct != null)
            {
                await _shoppingCart.AddToCartAsync(selectedProduct, 1);
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Remove From Cart
        public async Task<IActionResult> RemoveFromShoppingCart(int productId)
        {
            var selectedProduct = await _productRepository.GetByIdAsync(productId);

            if (selectedProduct != null)
            {
                await _shoppingCart.RemoveFromCartAsync(selectedProduct);
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Clear Cart
        public async Task<IActionResult> ClearCart()
        {
            await _shoppingCart.ClearCartAsync();

            return RedirectToAction("Index");
        }
        #endregion
    }
}

