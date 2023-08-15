using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SuperMarketSystem.Data;
using SuperMarketSystem.Models;
using System;
using System.Data;

namespace SuperMarketSystem.Repositories.Implements
{
    [Authorize(Roles = "Admin")]
    public class ProductRepository
    {
        private readonly MyDBContext _context;

        public ProductRepository(MyDBContext context)
        {
            _context = context;
        }

        //public IEnumerable<Product> Products => _context.Products.Include(p => p.Category).Include(p => p.Ra); //include here

        //public IEnumerable<Product> ProductOfTheWeek => _context.Products.Where(p => p.).Include(p => p.Category);

        public void Add(Product product)
        {
            _context.Add(product);
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllIncludedAsync()
        {
            return await _context.Products.Include(p => p.Category).Include(p => p.Rates).ToListAsync();
        }

        public IEnumerable<Product> GetAllIncluded()
        {
            return _context.Products.Include(p => p.Category).Include(p => p.Rates).ToList();
        }

        public Product GetById(int? id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }

        public async Task<Product> GetByIdAsync(int? id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public Product GetByIdIncluded(int? id)
        {
            return _context.Products.Include(p => p.Category).Include(p => p.Rates).FirstOrDefault(p => p.Id == id);
        }

        public async Task<Product> GetByIdIncludedAsync(int? id)
        {
            return await _context.Products.Include(p => p.Category).Include(p => p.Rates).FirstOrDefaultAsync(p => p.Id == id);
        }

        public bool Exists(int id)
        {
            return _context.Products.Any(p => p.Id == id);
        }

        public void Remove(Product product)
        {
            _context.Remove(product);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            _context.Update(product);
        }
    }
}
