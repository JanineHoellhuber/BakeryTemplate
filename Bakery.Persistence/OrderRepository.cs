using Bakery.Core.Contracts;
using Bakery.Core.DTOs;
using Bakery.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bakery.Persistence
{
  public class OrderRepository : IOrderRepository
  {
    private readonly ApplicationDbContext _dbContext;
    public OrderRepository(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public async Task<int> GetCountAsync()
    {
      return await _dbContext.Orders.CountAsync();
    }


     public async Task<IEnumerable<OrderDto>> GetAllDtosAsync()
        {
              return await _dbContext.Orders
                    .OrderBy(o => o.OrderNr)
                    .Select(o => new OrderDto(o))
                    .ToArrayAsync();
        }

      public async Task<IEnumerable<OrderDto>> GetFilteredByLastname(string filterLastName)
        {
            return await _dbContext.Orders
                  .Where(o => o.Customer.LastName.ToUpper().Contains(filterLastName.ToUpper()))
                    .Select(o => new OrderDto(o))
                    .ToArrayAsync();
        }
       public void Add(Order order)
        {
            _dbContext.Orders.Add(order);
        }

        public void Remove(Order order)
        {
            _dbContext.Orders
                .Remove(order);
        }

        public async Task<Order> GetAllByIdAsync(int id)
        {
            return await _dbContext.Orders
                        .SingleOrDefaultAsync(o => o.Id == id);

        }
        public async Task<OrderWithItemsDto> GetByIdAsync(int id)
        {
            return await _dbContext.Orders
                    .Where(o => o.Id == id)
                    .Select(o => new OrderWithItemsDto(o))
                    .SingleOrDefaultAsync();

        }

        public async Task<IEnumerable<OrderWithItemsDto>> GeItemsAsync()
        {
            return await _dbContext.Orders
                    .Select(o => new OrderWithItemsDto(o))
                    .ToArrayAsync();
        }

      

    }
}
