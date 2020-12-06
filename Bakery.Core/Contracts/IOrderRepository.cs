using Bakery.Core.DTOs;
using Bakery.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bakery.Core.Contracts
{
  public interface IOrderRepository
  {
    Task<int> GetCountAsync();
        Task<IEnumerable<OrderDto>> GetAllDtosAsync();
        Task<IEnumerable<OrderDto>> GetFilteredByLastname(string filterLastName);
        void Add(Order order);
        Task<OrderWithItemsDto> GetByIdAsync(int id);
        Task<Order> GetAllByIdAsync(int id);

        void Remove(Order order);
        Task<IEnumerable<OrderWithItemsDto>> GeItemsAsync();
    }
}