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
using SuperMarketSystem.Repositories.Interfaces;
using SuperMarketSystem.Repositories.Implements;
using Microsoft.AspNetCore.Authorization;

namespace SuperMarketSystem.Controllers
{
    [Authorize(Roles = "Admin")]

    public class CategoriesController : Controller
    {
        private readonly MyDBContext _context;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryrepository;

        public CategoriesController(MyDBContext context, IMapper mapper, ICategoryRepository categoryrepository)
        {
            _context = context;
            _mapper = mapper;
            _categoryrepository = categoryrepository;
        }
        [Authorize(Roles = "Admin")]
        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var dbCategory = await _context.Categories.Select(u => _mapper.Map<CategoryDTO>(u)).ToListAsync();

            return _context.Categories != null ?
                        View(dbCategory) :
                        Problem("Entity set 'MyDBContext.Categories'  is null.");
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await _categoryrepository.GetById(id);

            CategoryDTO categoryDTO = _mapper.Map<CategoryDTO>(category);

            if (categoryDTO == null)
            {
                return NotFound();
            }

            return View(categoryDTO);
        }

        // GET: Categories/Create
        [Authorize(Policy = "AdminPolicy")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [Authorize(Policy = "AdminPolicy")]
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

        // GET: Categories/Edit/5
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryrepository.GetById(id);

            CategoryDTO categoryDTO = _mapper.Map<CategoryDTO>(category);

            if (categoryDTO == null)
            {
                return NotFound();
            }

            return View(categoryDTO);
        }

        // POST: Categories/Edit/5
        [Authorize(Policy = "AdminPolicy")]
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

        [HttpGet]
        // GET: Categories/Delete/5
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryrepository.GetById(id);

            CategoryDTO categoryDTO = _mapper.Map<CategoryDTO>(category);

            if (categoryDTO == null)
            {
                return NotFound();
            }
            return View(categoryDTO);
        }

        // POST: Categories/Delete/5
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'MyDBContext.Categories' is null.");
            }
            var category = await _categoryrepository.GetById(id);
            if (category != null)
            {
                _categoryrepository.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
