using Microsoft.EntityFrameworkCore;
using SuperMarketSystem.Data;
using SuperMarketSystem.Models;
using SuperMarketSystem.Repositories.Interfaces;

namespace SuperMarketSystem.Repositories.Implements
{
    public class CustomerAddressRepository : ICustomerAddressRepository
    {
        private readonly MyDBContext _context;

        public CustomerAddressRepository(MyDBContext context)
        {
            _context = context;
        }
        #region Get By Id
        public CustomerAddress GetById(int? id)
        {
            return _context.CustomerAddresses.FirstOrDefault(p => p.Id == id);
        }

        public async Task<CustomerAddress> GetByIdAsync(int? id)
        {
            return await _context.CustomerAddresses.FirstOrDefaultAsync(p => p.Id == id);
        }
        #endregion

        #region Create
        public void Add(CustomerAddress address)
        {
            _context.CustomerAddresses.Add(address);
        }
        #endregion

        #region Exists
        public bool Exists(int id)
        {
            return (_context.CustomerAddresses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        #endregion

        #region Delete
        public void Remove(CustomerAddress address)
        {
            _context.CustomerAddresses.Remove(address);
        }
        #endregion

        #region Update
        public void Update(CustomerAddress address)
        {
            _context.CustomerAddresses.Update(address);
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
