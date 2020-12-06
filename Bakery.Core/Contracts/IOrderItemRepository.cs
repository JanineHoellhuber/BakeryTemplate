using Bakery.Core.Entities;
using System.Threading.Tasks;

namespace Bakery.Core.Contracts
{
  public interface IOrderItemRepository
  {
    Task<int> GetCountAsync();
        void Add(OrderItem orderItem);

  }
}