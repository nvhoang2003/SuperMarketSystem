using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuperMarketSystem.Data;
using AutoMapper;
using SuperMarketSystem.DTOs;
using SuperMarketSystem.Repositories.Interfaces;
using SuperMarketSystem.Repositories.Implements;
using Microsoft.AspNetCore.Authorization;
using SuperMarketSystem.Models;

namespace SuperMarketSystem.Controllers
{
    public class CategoriesController : Controller
    {
        #region Fields
        private readonly MyDBContext _context;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryrepository;
        #endregion

        #region Constructor
        public CategoriesController(MyDBContext context, IMapper mapper, ICategoryRepository categoryrepository)
        {
            _context = context;
            _mapper = mapper;
            _categoryrepository = categoryrepository;
        }
        #endregion

        #region Index
        [AllowAnonymous]
        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var dbCategory = await _context.Categories.Select(u => _mapper.Map<CategoryDTO>(u)).ToListAsync();

            return _context.Categories != null ?
                        View(dbCategory) :
                        Problem("Entity set 'MyDBContext.Categories'  is null.");
        }
        #endregion

        #region Details
        // GET: Categories/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await _categoryrepository.GetByIdAsync(id);

            CategoryDTO categoryDTO = _mapper.Map<CategoryDTO>(category);

            if (categoryDTO == null)
            {
                return NotFound();
            }

            return View(categoryDTO);
        }
        #endregion

        #region Create
        // GET: Categories/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryDTO createCategoryDTO)
        {
            if (ModelState.IsValid)
            {
                Category category = _mapper.Map<Category>(createCategoryDTO);
                _categoryrepository.Add(category);
                await _categoryrepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(createCategoryDTO);
        }
        #endregion

        #region Edit
        // GET: Categories/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryrepository.GetByIdAsync(id);

            CategoryDTO categoryDTO = _mapper.Map<CategoryDTO>(category);

            if (categoryDTO == null)
            {
                return NotFound();
            }

            return View(categoryDTO);
        }

        // POST: Categories/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( CategoryDTO categoryDTO)
        {
            if (ModelState.IsValid)
            {

                Category category = _mapper.Map<Category>(categoryDTO);
                _context.Update(category);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(categoryDTO);
        }
        #endregion

        #region Delete
        // GET: Categories/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryrepository.GetByIdAsync(id);

            CategoryDTO categoryDTO = _mapper.Map<CategoryDTO>(category);

            if (categoryDTO == null)
            {
                return NotFound();
            }
            return View(categoryDTO);
        }

        // POST: Categories/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'MyDBContext.Categories' is null.");
            }
            var category = await _categoryrepository.GetByIdAsync(id);
            if (category != null)
            {
                _categoryrepository.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Exitsts
        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        #endregion
    }
}
