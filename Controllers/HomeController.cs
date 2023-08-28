using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperMarketSystem.Data;
using SuperMarketSystem.DTOs;
using SuperMarketSystem.Models;
using System.Diagnostics;

namespace SuperMarketSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDBContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HomeController(ILogger<HomeController> logger, MyDBContext context, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var productResponse = await _context.Products.Where(u => u.Quantity > 0).Include(p => p.Brand).
                Include(p => p.Category).
                Select(u => _mapper.Map<CustomerProductDTO>(u)).
                ToListAsync();

            foreach(var item in productResponse)
            {
                item.ImageName = _context.Images.Where(c => c.ProductId == item.Id).Select(i => i.ImageName).ToList();
            }

            return View(productResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string search)
        {
            var productResponse = await _context.Products.
                Where(u => u.Quantity > 0 && (search == null || u.Name.Contains(search))).
                Include(p => p.Brand).
                Include(p => p.Category).
                Select(u => _mapper.Map<CustomerProductDTO>(u)).
                ToListAsync();

            ViewBag.Search = search;

            foreach (var item in productResponse)
            {
                item.ImageName = _context.Images.Where(c => c.ProductId == item.Id).Select(i => i.ImageName).ToList();
            }

            return View(productResponse);
        }

        [HttpGet("/Home/Product/{id}")]
        public async Task<IActionResult> Product(int id)
        {
            var productResponse = await _context.Products.Include(p => p.Brand).
                Include(p => p.Category).
                Where(p => p.Id == id).
                Select(u => _mapper.Map<CustomerProductDTO>(u)).
                FirstOrDefaultAsync();

            int numberOfRate = _context.Rates.Where(r => r.ProductId == id).Count();

            productResponse.ImageName = _context.Images.Where(c => c.ProductId == id).Select(i => i.ImageName).ToList();
            productResponse.RateStar = numberOfRate == 0 ? 0 : _context.Rates.Where(r => r.ProductId == id).Sum(r => r.Star) / numberOfRate;
            //productResponse.NumberOfOrder = _context.Orders.Where(o => o.)

            return View(productResponse);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}