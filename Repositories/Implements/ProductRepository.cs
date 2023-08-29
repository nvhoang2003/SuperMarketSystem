using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SuperMarketSystem.Data;
using SuperMarketSystem.Models;
using System;
using System.Data;

namespace SuperMarketSystem.Repositories.Implements
{
    public class ProductRepository
    {
        private readonly MyDBContext _context;

        public ProductRepository(MyDBContext context)
        {
            _context = context;
        }
        #region Get Top Product of The Week
        public IEnumerable<Product> ProductOfTheWeek => _context.Products.Where(p => p.IsTopOfTheWeek).Include(p => p.Categories);
        #endregion

        #region Get All
        public IEnumerable<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }
        #endregion

        #region Get All Include
        public async Task<IEnumerable<Product>> GetAllIncludedAsync()
        {
            return await _context.Products.Include(p => p.Categories).Include(p => p.Rates).ToListAsync();
        }

        public IEnumerable<Product> GetAllIncluded()
        {
            return _context.Products.Include(p => p.Categories).Include(p => p.Rates).ToList();
        }
        #endregion

        #region Get By Id
        public Product GetById(int? id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }
        public async Task<Product> GetByIdAsync(int? id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }
        #endregion 

        #region Get By Code
        public Product GetByCode(Guid code)
        {
            return _context.Products.FirstOrDefault(p => p.ProductCode == code);
        }
        public async Task<Product> GetByCodeAsync(Guid code)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.ProductCode == code);
        }
        #endregion

        #region Get By Id Include
        public Product GetByIdIncluded(int? id)
        {
            return _context.Products.Include(p => p.Categories).Include(p => p.Rates).FirstOrDefault(p => p.Id == id);
        }
        public async Task<Product> GetByIdIncludedAsync(int? id)
        {
            return await _context.Products.Include(p => p.Categories).Include(p => p.Rates).FirstOrDefaultAsync(p => p.Id == id);
        }
        #endregion

        #region Get By Code Include

        public Product GetByCodeIncluded(Guid code)
        {
            return _context.Products.Include(p => p.Categories).Include(p => p.Rates).FirstOrDefault(p => p.ProductCode == code);
        }
        public async Task<Product> GetByCodeIncludedAsync(Guid code)
        {
            return await _context.Products.Include(p => p.Categories).Include(p => p.Rates).FirstOrDefaultAsync(p => p.ProductCode == code);
        }
        #endregion

        #region Create
        public void Add(Product product)
        {
            _context.Add(product);
        }
        #endregion

        #region Update
        public void Update(Product product)
        {
            _context.Update(product);
        }
        #endregion

        #region Delete
        public void Remove(Product product)
        {
            _context.Remove(product);
        }
        #endregion

        #region Exists
        public bool Exists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public bool ExistsByCode(Guid code)
        {
            return (_context.Products?.Any(p => p.ProductCode == code)).GetValueOrDefault();
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
