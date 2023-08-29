using Microsoft.EntityFrameworkCore;
using SuperMarketSystem.Data;
using SuperMarketSystem.Models;
using SuperMarketSystem.Repositories.Interfaces;

namespace SuperMarketSystem.Repositories.Implements
{
    public class CustomerRepository : ICustomerRepository
    {

        private readonly MyDBContext _context;

        public CustomerRepository(MyDBContext context)
        {
            _context = context;
        }
        #region Get All
        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }
        #endregion

        #region Get By Id

        public Customer GetById(int? id)
        {
            return _context.Customers.FirstOrDefault(p => p.Id == id);
            
        }
        public async Task<Customer> GetByIdAsync(int? id)
        {
            return await _context.Customers.FirstOrDefaultAsync(p => p.Id == id);
        }
        #endregion

        #region Get By Code
        public Customer GetByCode(Guid code)
        {
            return _context.Customers.FirstOrDefault(p => p.CustomerCode == code);

        }
        public async Task<Customer> GetByCodeAsync(Guid code)
        {
            return await _context.Customers.FirstOrDefaultAsync(p => p.CustomerCode == code);
        }

        #endregion

        #region Create
        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
        }
        #endregion

        #region Update
        public void Update(Customer customer)
        {
            _context.Update(customer);
        }
        #endregion

        #region Delete
        public void Remove(Customer customer)
        {
            _context.Customers.Remove(customer);
        }
        #endregion

        #region Exists
        public bool Exists(int id)
        {
            return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public bool ExistsByCode(Guid code)
        {
            return (_context.Customers?.Any(p => p.CustomerCode == code)).GetValueOrDefault();
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

