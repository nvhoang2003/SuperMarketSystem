using Microsoft.EntityFrameworkCore;
using SuperMarketSystem.Data;
using SuperMarketSystem.Models;
using SuperMarketSystem.Repositories.Interfaces;

namespace SuperMarketSystem.Repositories.Implements
{
    public class CustomerInfoRepository : ICustomerInfoRepository
    {
        private readonly MyDBContext _context;

        public CustomerInfoRepository(MyDBContext context)
        {
            _context = context;
        }
        #region Get By Id
        public CustomerInfo GetById(int? id)
        {
            return _context.CustomerInfos.FirstOrDefault(p => p.Id == id);
        }

        public async Task<CustomerInfo> GetByIdAsync(int? id)
        {
            return await _context.CustomerInfos.FirstOrDefaultAsync(p => p.Id == id);
        }
        #endregion

        #region Create
        public void Add(CustomerInfo info)
        {
            _context.CustomerInfos.Add(info);
        }
        #endregion

        #region Update

        public void Update(CustomerInfo info)
        {
            _context.CustomerInfos.Update(info);
        }
        #endregion

        #region Delete
        public void Remove(CustomerInfo info)
        {
            _context.CustomerInfos.Remove(info);
        }
        #endregion

        #region Exists
        public bool Exists(int id)
        {
            return (_context.CustomerAddresses?.Any(e => e.Id == id)).GetValueOrDefault();
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
