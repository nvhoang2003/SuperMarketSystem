using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SuperMarketSystem.Data;
using SuperMarketSystem.Models;
using SuperMarketSystem.Repositories.Interfaces;
using SuperMarketSystem.ViewModels;
using System;

namespace SuperMarketSystem.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly MyDBContext _context;
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartController(IProductRepository productRepository,
            ShoppingCart shoppingCart, MyDBContext context)
        {
            _productRepository = productRepository;
            _shoppingCart = shoppingCart;
            _context = context;
        }

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

        public async Task<IActionResult> AddToShoppingCart(int productId)
        {
            var selectedProduct = await _productRepository.GetByIdAsync(productId);

            if (selectedProduct != null)
            {
                await _shoppingCart.AddToCartAsync(selectedProduct, 1);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveFromShoppingCart(int productId)
        {
            var selectedProduct = await _productRepository.GetByIdAsync(productId);

            if (selectedProduct != null)
            {
                await _shoppingCart.RemoveFromCartAsync(selectedProduct);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ClearCart()
        {
            await _shoppingCart.ClearCartAsync();

            return RedirectToAction("Index");
        }

    }
}

