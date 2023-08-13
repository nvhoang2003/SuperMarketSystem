 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.DataObject;
using SuperMarketSystem.Data;
using AutoMapper;
using SuperMarketSystem.DTOs;
using SuperMarketSystem.Models;

namespace SuperMarketSystem.Controllers
{
    public class ProductsController : Controller
    {
        private readonly MyDBContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductsController(MyDBContext context, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var productResponse = await _context.Products.Include(p => p.Brand).
                Include(p => p.Category).
                Select(u => _mapper.Map<ProductDTO>(u)).
                ToListAsync();


            return View(productResponse);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                ProductDTO productRes = _mapper.Map<ProductDTO>(product);
                productRes.ImageName = _context.Images.Where(c => c.ProductId == id).Select(i => i.ImageName).ToList();
                return View(productRes);
            }
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CategoryId,BrandId,Quantity,UnitCost,ImageFile")] CreateProductDTO createProductDTO)
        {
            try
            {
                Product product = _mapper.Map<Product>(createProductDTO);
            _context.Add(product);
            await _context.SaveChangesAsync();

            List<Image> imgs = new List<Image>();
            string wwwRootPath = _hostEnvironment.WebRootPath;

            foreach (var item in createProductDTO.ImageFile)
            {
                Image img = new Image();

                string fileName = Path.GetFileNameWithoutExtension(item.FileName);
                string extension = Path.GetExtension(item.FileName);
                img.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await item.CopyToAsync(fileStream);
                }
                img.ProductId = product.Id;
                imgs.Add(img);
            }
            //Insert record
            _context.Images.AddRange(imgs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id", createProductDTO.BrandId);
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", createProductDTO.CategoryId);
                return View(createProductDTO);
            }
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");

            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return View(_mapper.Map<CreateProductDTO>(product));
            }
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CategoryId,BrandId,Quantity,UnitCost,ImageFile")] CreateProductDTO updateProductDTO)
        {
            //try
            //{
                Product product = _mapper.Map<Product>(updateProductDTO);
                product.Id = id; 
                _context.Update(product);
                if(updateProductDTO.ImageFile != null)
                {
                    List<Image> imgsDelete = _context.Images.Where(i => i.ProductId == id).ToList();
                    _context.Images.RemoveRange(imgsDelete);

                    List<Image> imgs = new List<Image>();
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    foreach (var item in updateProductDTO.ImageFile)
                    {
                        Image img = new Image();

                        string fileName = Path.GetFileNameWithoutExtension(item.FileName);
                        string extension = Path.GetExtension(item.FileName);
                        img.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await item.CopyToAsync(fileStream);
                        }
                        img.ProductId = product.Id;
                        imgs.Add(img);
                    }
                    //Insert record
                    _context.Images.AddRange(imgs);
                }    
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id", updateProductDTO.BrandId);
            //    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", updateProductDTO.CategoryId);
            //    return View(updateProductDTO);
            //}
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return View(_mapper.Map<CreateProductDTO>(product));
            }
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'MyDBContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                List<Image> imgsDelete = _context.Images.Where(i => i.ProductId == id).ToList();
                _context.Images.RemoveRange(imgsDelete);
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
