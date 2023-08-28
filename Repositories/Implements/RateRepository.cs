using Microsoft.EntityFrameworkCore;
using SuperMarketSystem.Data;
using SuperMarketSystem.Models;
using System;

namespace SuperMarketSystem.Repositories.Implements
{
    public class RateRepository
    {
        private readonly MyDBContext _context;

        public RateRepository(MyDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Rate> Rates => _context.Rates.Include(x => x.Product);

        public void Add(Rate review)
        {
            _context.Rates.Add(review);
        }

        public IEnumerable<Rate> GetAll()
        {
            return _context.Rates.ToList();
        }

        public async Task<IEnumerable<Rate>> GetAllAsync()
        {
            return await _context.Rates.ToListAsync();
        }

        public Rate GetById(int? id)
        {
            return _context.Rates.FirstOrDefault(p => p.Id == id);
        }

        public async Task<Rate> GetByIdAsync(int? id)
        {
            return await _context.Rates.FirstOrDefaultAsync(p => p.Id == id);
        }

        public bool Exists(int id)
        {
            return _context.Rates.Any(p => p.Id == id);
        }

        public void Remove(Rate review)
        {
            _context.Rates.Remove(review);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Rate review)
        {
            _context.Rates.Update(review);
        }
    }
}
