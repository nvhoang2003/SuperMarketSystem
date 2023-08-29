using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls;
using SuperMarketSystem.Data;
using SuperMarketSystem.Models;
using SuperMarketSystem.Repositories.Interfaces;
using System.Reflection.Metadata.Ecma335;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace SuperMarketSystem.Repositories.Implements
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MyDBContext _context;

        public CategoryRepository(MyDBContext context)
        {
            _context = context;
        }
        #region Get All
        public IEnumerable<Category> GetAll()
        {
            return _context.Categories.ToList();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }
        #endregion

        #region Get By Id
        Category ICategoryRepository.GetById(int? id)
        {
           return _context.Categories.FirstOrDefault(p => p.Id == id);
        }
        public async Task<Category> GetByIdAsync(int? id)
        {
            return await _context.Categories.FirstOrDefaultAsync(p => p.Id == id);
        }
        #endregion

        #region Get By Code
        public Category GetByCode(Guid code)
        {
            return _context.Categories.FirstOrDefault(p => p.CategoryCode == code);

        }
        public async Task<Category> GetByCodeAsync(Guid code)
        {
            return await _context.Categories.FirstOrDefaultAsync(p => p.CategoryCode == code);
        }

        #endregion

        #region Create
        public void Add(Category category)
        {
            _context.Categories.Add(category);
        }
        #endregion

        #region Update
        public void Update(Category category)
        {
            _context.Update(category);
        }
        #endregion

        #region Delete
        public void Remove(Category category)
        {
            _context.Categories.Remove(category);
        }
        #endregion

        #region Exists
        public bool Exists(int id)
        {
            return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public bool ExistsByCode(Guid code)
        {
            return (_context.Categories?.Any(p => p.CategoryCode == code)).GetValueOrDefault();
        }
        #endregion

        #region Save Change
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        #endregion
    }
}
