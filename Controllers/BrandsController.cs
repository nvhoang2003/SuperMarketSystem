using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.DataObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SuperMarketSystem.Data;
using SuperMarketSystem.Models;

//namespace SuperMarketSystem.Controllers
//{
//    public class BrandsController : Controller
//    {
//        private readonly MyDBContext _context;
//        private readonly IWebHostEnvironment _hostEnvironment;

//        public BrandsController(MyDBContext context, IWebHostEnvironment hostEnvironment)
//        {
//            _hostEnvironment = hostEnvironment;
//            _context = context;
//        }

//        // GET: Brands
//        public async Task<IActionResult> Index()
//        {
//            return _context.Brand != null ?
//                        View(await _context.Brand.ToListAsync()) :
//                        Problem("Entity set 'MyDBContext.Brand'  is null.");
//        }

//        // GET: Brands/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null || _context.Brand == null)
//            {
//                return NotFound();
//            }

//            var brand = await _context.Brand
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (brand == null)
//            {
//                return NotFound();
//            }

//            return View(brand);
//        }

//        // GET: Brands/Create
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // POST: Brands/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Id,Name,Description,ImageFile")] Brand brand)
//        {
//            try
//            {
//                string wwwRootPath = _hostEnvironment.WebRootPath;
//                string fileName = Path.GetFileNameWithoutExtension(brand.ImageFile.FileName);
//                string extension = Path.GetExtension(brand.ImageFile.FileName);
//                brand.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
//                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
//                using (var fileStream = new FileStream(path, FileMode.Create))
//                {
//                    await brand.ImageFile.CopyToAsync(fileStream);
//                }
//                //Insert record
//                _context.Add(brand);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            catch
//            {
//                ViewData["Id"] = new SelectList(_context.Brand, "Id", "Name", brand.Id);
//                return View(brand);
//            }
//        }

//        // GET: Brands/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null || _context.Brand == null)
//            {
//                return NotFound();
//            }

//            var brand = await _context.Brand.FindAsync(id);
//            if (brand == null)
//            {
//                return NotFound();
//            }
//            return View(brand);
//        }

//        // POST: Brands/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImageFile")] Brand brand)
//        {
//            if (id != brand.Id)
//            {
//                return NotFound();
//            }

//            try
//            {
//                if (brand.ImageFile != null)
//                {
//                    string wwwRootPath = _hostEnvironment.WebRootPath;
//                    string fileName = Path.GetFileNameWithoutExtension(brand.ImageFile.FileName);
//                    string extension = Path.GetExtension(brand.ImageFile.FileName);
//                    brand.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
//                    string path = Path.Combine(wwwRootPath + "/Image/", fileName);
//                    using (var fileStream = new FileStream(path, FileMode.Create))
//                    {
//                        await brand.ImageFile.CopyToAsync(fileStream);
//                    }
//                }
//                else
//                {
//                    var productExisted = await _context.Products
//                                            .FirstOrDefaultAsync(m => m.Id == brand.Id);
//                    brand.ImageName = productExisted.ImageName;
//                }
//                _context.Update(brand);
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                ViewData["Id"] = new SelectList(_context.Categories, "Id", "Name", brand.Id);
//                return View(brand);
//            }
//            return RedirectToAction(nameof(Index));
//        }

//        // GET: Brands/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null || _context.Brand == null)
//            {
//                return NotFound();
//            }

//            var brand = await _context.Brand
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (brand == null)
//            {
//                return NotFound();
//            }

//            return View(brand);
//        }

//        // POST: Brands/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            if (_context.Brand == null)
//            {
//                return Problem("Entity set 'MyDBContext.Brand'  is null.");
//            }
//            var brand = await _context.Brand.FindAsync(id);
//            if (brand != null)
//            {
//                _context.Brand.Remove(brand);
//            }

//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool BrandExists(int id)
//        {
//            return (_context.Brand?.Any(e => e.Id == id)).GetValueOrDefault();
//        }
//    }
//}
