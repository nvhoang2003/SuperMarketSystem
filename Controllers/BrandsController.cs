using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SuperMarketSystem.Data;
using SuperMarketSystem.Models;

namespace SuperMarketSystem.Controllers
{
    public class BrandsController : Controller
    {
        #region Fields
        private readonly MyDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        #endregion

        #region Constructor
        public BrandsController(MyDBContext context, IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
        }
        #endregion

        #region Index
        // GET: Brands
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return _context.Brands != null ?
                        View(await _context.Brands.ToListAsync()) :
                        Problem("Entity set 'MyDBContext.Brand'  is null.");
        }
        #endregion

        #region Details
        // GET: Brands/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }
        #endregion

        #region Create
        // GET: Brands/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ImageFile")] Brand brand)
        {
            try
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(brand.ImageFile.FileName);
                string extension = Path.GetExtension(brand.ImageFile.FileName);
                brand.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await brand.ImageFile.CopyToAsync(fileStream);
                }
                //Insert record
                _context.Add(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewData["Id"] = new SelectList(_context.Brands, "Id", "Name", brand.Id);
                return View(brand);
            }
        }
        #endregion

        #region Edit
        // GET: Brands/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImageFile")] Brand brand)
        {
            if (id != brand.Id)
            {
                return NotFound();
            }

            try
            {
                if (brand.ImageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(brand.ImageFile.FileName);
                    string extension = Path.GetExtension(brand.ImageFile.FileName);
                    brand.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await brand.ImageFile.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    var productExisted = await _context.Products
                                            .FirstOrDefaultAsync(m => m.Id == brand.Id);
                    brand.ImageName = productExisted.Name;
                }
                _context.Update(brand);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                ViewData["Id"] = new SelectList(_context.Categories, "Id", "Name", brand.Id);
                return View(brand);
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete
        // GET: Brands/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: Brands/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Brands == null)
            {
                return Problem("Entity set 'MyDBContext.Brand'  is null.");
            }
            var brand = await _context.Brands.FindAsync(id);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Exists
        private bool BrandExists(int id)
        {
            return (_context.Brands?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        #endregion
    }
}
