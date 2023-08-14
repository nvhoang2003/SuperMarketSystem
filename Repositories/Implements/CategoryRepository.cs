using AutoMapper;
using DataAccessLayer.DataObject;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls;
using SuperMarketSystem.Data;
using SuperMarketSystem.Repositories.Interfaces;
using System.Reflection.Metadata.Ecma335;

namespace SuperMarketSystem.Repositories.Implements
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MyDBContext _context;

        public CategoryRepository(MyDBContext context)
        {
            _context = context;
        }

        public void Add(Category category)
        {
            _context.Categories.Add(category);
        }

        public bool Exists(int id)
        {
            return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetById(int? id)
        {
            return await _context.Categories.FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Remove(Category category)
        {
            _context.Categories.Remove(category);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Category category)
        {
            _context.Update(category);
        }
    }
}
