using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SuperMarketSystem.Repositories.Interfaces;
using System.Data;
using System;
using SuperMarketSystem.Data;
using SuperMarketSystem.ViewModels;

//namespace SuperMarketSystem.Controllers
//{
    //[Authorize(Roles = "Admin")]
//    public class ProductController : Controller
//    {
   
//        private readonly MyDBContext _context;
//        private readonly IProductRepository _product;
//        private readonly ICategoryRepository _category;

//        public ProductController(MyDBContext context, IProductRepository product, ICategoryRepository category)
//        {
//            _context = context;
//            _category = category;
//            _product = product;
//        }

//        // GET: Pizzas
//        public async Task<IActionResult> Index()
//        {
//            return View(await _product.GetAllIncludedAsync());
//        }

//        // GET: Pizzas
//        [AllowAnonymous]
//        public async Task<IActionResult> ListAll()
//        {
//            var model = new SearchProductViewModel()
//            {
//                ProductList = await _product.GetAllIncludedAsync(),
//                SearchText = null
//            };

//            return View(model);
//        }

//        private async Task<List<Product>> GetPizzaSearchList(string userInput)
//        {
//            var userInputTrimmed = userInput?.ToLower()?.Trim();

//            if (string.IsNullOrWhiteSpace(userInputTrimmed))
//            {
//                return await _context.Products.Include(p => p.Category)
//                    .Select(p => p).OrderBy(p => p.Name)
//                    .ToListAsync();
//            }
//            else
//            {
//                return await _context.Products.Include(p => p.Category)
//                    .Where(p => p
//                    .Name.ToLower().Contains(userInputTrimmed))
//                    .Select(p => p).OrderBy(p => p.Name)
//                    .ToListAsync();
//            }
//        }

//        [AllowAnonymous]
//        [HttpGet]
//        public async Task<IActionResult> AjaxSearchList(string searchString)
//        {
//            var productList = await GetProductSearchList(searchString);

//            return PartialView(productList);
//        }

//        [AllowAnonymous]
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> ListAll([Bind("SearchText")] SearchProductViewModel model)
//        {
//            var products = await _product.GetAllIncludedAsync();
//            if (model.SearchText == null || model.SearchText == string.Empty)
//            {
//                model.ProductList = products;
//                return View(model);
//            }

//            var input = model.SearchText.Trim();
//            if (input == string.Empty || input == null)
//            {
//                model.ProductList = products;
//                return View(model);
//            }
//            var searchString = input.ToLower();

//            if (string.IsNullOrEmpty(searchString))
//            {
//                model.ProductList = products;
//            }
//            else
//            {
//                var productList = await _context.Products.Include(x => x.Category).Include(x => x.Rates).OrderBy(x => x.Name)
//                     .Where(p =>
//                     p.Name.ToLower().Contains(searchString)
//                  || p.Quantity.ToString("c").ToLower().Contains(searchString)
//                  || p.Category.Name.ToLower().Contains(searchString)

//                if (productList.Any())
//                {
//                    model.ProductList = productList;
//                }
//                else
//                {
//                    model.ProductList = new List<Product>();
//                }

//            }
//            return View(model);
//        }

//        // GET: Pizzas
//        [AllowAnonymous]
//        public async Task<IActionResult> ListCategory(string categoryName)
//        {
//            bool categoryExtist = _context.Categories.Any(c => c.Name == categoryName);
//            if (!categoryExtist)
//            {
//                return NotFound();
//            }

//            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);

//            if (category == null)
//            {
//                return NotFound();
//            }

//            bool anyProducts = await _context.Products.AnyAsync(x => x.Category == category);
//            if (!anyProducts)
//            {
//                return NotFound($"No Pizzas were found in the category: {categoryName}");
//            }

//            var products = _context.Products.Where(x => x.Category == category)
//                .Include(x => x.Category).Include(x => x.Rates);

//            ViewBag.CurrentCategory = category.Name;
//            return View(products);
//        }

//        // GET: Pizzas/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var products = await _product.GetByIdIncludedAsync(id);

//            if (products == null)
//            {
//                return NotFound();
//            }

//            return View(products);
//        }

//        // GET: Pizzas/Details/5
//        [AllowAnonymous]
//        public async Task<IActionResult> DisplayDetails(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var products = await _product.GetByIdIncludedAsync(id);

//            var listOfIngredients = await _context.Products.Where(x => x.Id == id).Select(x => x.IsProductOfTheWeek).ToListAsync();
//            ViewBag.ListProductOfTheWeek = listOfIngredients;

//            //var listOfReviews = await _context.Reviews.Where(x => x.PizzaId == id).Select(x => x).ToListAsync();
//            //ViewBag.Reviews = listOfReviews;
//            double score;
//            if (_context.Rates.Any(x => x.ProductId == id))
//            {
//                var rate = _context.Rates.Where(x => x.ProductId == id);
//                score = rate.Average(x => x.Star);
//                score = Math.Round(score, 2);
//            }
//            else
//            {
//                score = 0;
//            }
//            ViewBag.AverageReviewScore = score;

//            if (products == null)
//            {
//                return NotFound();
//            }

//            return View(products);
//        }

//        // GET: Pizzas
//        [AllowAnonymous]
//        public async Task<IActionResult> SearchPizzas()
//        {
//            var model = new SearchProductViewModel()
//            {
//                ProductList = await _product.GetAllIncludedAsync(),
//                SearchText = null
//            };

//            return View(model);
//        }

//        [AllowAnonymous]
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> SearchPizzas([Bind("SearchText")] SearchProductViewModel model)
//        {
//            var products = await _product.GetAllIncludedAsync();
//            var search = model.SearchText.ToLower();

//            if (string.IsNullOrEmpty(search))
//            {
//                model.ProductList = products;
//            }
//            else
//            {
//                var productList = await _context.Products.Include(x => x.Category).Include(x => x.Rates).OrderBy(x => x.Name)
//                    .Where(p =>
//                     p.Name.ToLower().Contains(search)
//                  || p.Price.ToString("c").ToLower().Contains(search)
//                  || p.Category.Name.ToLower().Contains(search)
//                  || p.PizzaIngredients.Select(x => x.Ingredient.Name.ToLower()).Contains(search)).ToListAsync();

//                if (pizzaList.Any())
//                {
//                    model.PizzaList = pizzaList;
//                }
//                else
//                {
//                    model.PizzaList = new List<Pizzas>();
//                }

//            }
//            return View(model);
//        }

//        // GET: Pizzas/Create
//        public IActionResult Create()
//        {
//            ViewData["CategoriesId"] = new SelectList(_categoryRepo.GetAll(), "Id", "Name");
//            return View();
//        }

//        // POST: Pizzas/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,ImageUrl,IsPizzaOfTheWeek,CategoriesId")] Pizzas pizzas)
//        {
//            if (ModelState.IsValid)
//            {
//                _pizzaRepo.Add(pizzas);
//                await _pizzaRepo.SaveChangesAsync();
//                return RedirectToAction("Index");
//            }
//            ViewData["CategoriesId"] = new SelectList(_categoryRepo.GetAll(), "Id", "Name", pizzas.CategoriesId);
//            return View(pizzas);
//        }

//        // GET: Pizzas/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var pizzas = await _pizzaRepo.GetByIdAsync(id);

//            if (pizzas == null)
//            {
//                return NotFound();
//            }
//            ViewData["CategoriesId"] = new SelectList(_categoryRepo.GetAll(), "Id", "Name", pizzas.CategoriesId);
//            return View(pizzas);
//        }

//        // POST: Pizzas/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,ImageUrl,IsPizzaOfTheWeek,CategoriesId")] Pizzas pizzas)
//        {
//            if (id != pizzas.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _pizzaRepo.Update(pizzas);
//                    await _pizzaRepo.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!PizzasExists(pizzas.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction("Index");
//            }
//            ViewData["CategoriesId"] = new SelectList(_categoryRepo.GetAll(), "Id", "Name", pizzas.CategoriesId);
//            return View(pizzas);
//        }

//        // GET: Pizzas/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var pizzas = await _pizzaRepo.GetByIdIncludedAsync(id);

//            if (pizzas == null)
//            {
//                return NotFound();
//            }

//            return View(pizzas);
//        }

//        // POST: Pizzas/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var pizzas = await _pizzaRepo.GetByIdAsync(id);
//            _pizzaRepo.Remove(pizzas);
//            await _pizzaRepo.SaveChangesAsync();
//            return RedirectToAction("Index");
//        }

//        private bool PizzasExists(int id)
//        {
//            return _pizzaRepo.Exists(id);
//        }
//    }
//}

