using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bakery.Core.Contracts;
using Bakery.Core.DTOs;
using Bakery.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bakery.Persistence
{
  public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> GetCountAsync()
        {
            return await _dbContext.Products.CountAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Product> products)
        {
            await _dbContext.Products.AddRangeAsync(products);
        }


        public async Task<IEnumerable<ProductDto>> GetFilteredProductsAsync(double priceFrom, double priceTo)
        {
            var productsFromTo = _dbContext.Products
                .Where(p => p.Price >= priceFrom && p.Price <= priceTo)
                .AsQueryable();

            return await productsFromTo
                .Select(p => new ProductDto(p))
                .ToArrayAsync();

        }

        public async Task<Product[]> GetAllAsync()
        {
            return await _dbContext
            .Products
            .Include(p => p.OrderItems)
            .ToArrayAsync();
        }

        public async Task AddAsync(Product product)
        {
             await _dbContext.Products.AddAsync(product);
        }
        public void Update(Product productDb)
        {
            _dbContext.Products.Update(productDb);
        }
        public async Task<Product> GetByIdAsync(int id)
        {
            return await _dbContext.Products
                .Where(p => p.Id == id)
                .SingleOrDefaultAsync();
        }


    }
}
