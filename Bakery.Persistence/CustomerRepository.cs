using Bakery.Core.Contracts;
using Bakery.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bakery.Persistence
{
  public class CustomerRepository : ICustomerRepository
  {
    private readonly ApplicationDbContext _dbContext;

    public CustomerRepository(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public async Task<int> GetCountAsync()
    {
      return await _dbContext.Customers.CountAsync();
    }

     public async Task<Customer> GetByIdAsync(int customerId)
        {
            return await _dbContext.Customers
                .SingleOrDefaultAsync(s => s.Id == customerId);
        }


        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _dbContext.Customers
                .ToArrayAsync();
        }

    }
}
