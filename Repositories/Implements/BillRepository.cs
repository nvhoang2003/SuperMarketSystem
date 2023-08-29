using Microsoft.EntityFrameworkCore;
using SuperMarketSystem.Data;
using SuperMarketSystem.Models;
using SuperMarketSystem.Repositories.Interfaces;

namespace SuperMarketSystem.Repositories.Implements
{
    public class BillRepository : IBillRepository
    {
        private readonly MyDBContext _context;

        public BillRepository(MyDBContext context)
        {
            _context = context;
        }
        #region Get All

        public IEnumerable<Bill> GetAll()
        {
            return _context.Bills.ToList();
        }

        public async Task<IEnumerable<Bill>> GetAllAsync()
        {
            return await _context.Bills.ToListAsync();
        }
        #endregion

        #region Get By Id
        public Bill GetById(int? id)
        {
            return _context.Bills.FirstOrDefault(p => p.Id == id);
        }

        public async Task<Bill> GetByIdAsync(int? id)
        {
            return await _context.Bills.FirstOrDefaultAsync(p => p.Id == id);
        }
        #endregion

        #region Create
        public void Add(Bill bill)
        {
            _context.Bills.Add(bill);
        }
        #endregion

        #region Update

        public void Update(Bill bill)
        {
            _context.Bills.Update(bill);
        }
        #endregion

        #region Delete
        public void Remove(Bill bill)
        {
            _context.Bills.Remove(bill);
        }
        #endregion

        #region Exists
        public bool Exists(int id)
        {
            return (_context.Bills?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        #endregion

        #region SaveChange
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
