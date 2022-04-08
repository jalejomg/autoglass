using Autoglass.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autoglass.Data.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> CreateProductAsync(Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            return product.Id;
        }

        public async Task DeleteProductAsync(Product product)
        {
            _dbContext.Remove(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IList<Product>> GetAllProductsAsync(int amount, int page, string criteria, DateTime manufacturingDateStart, DateTime manufacturingDateEnd,
            DateTime dueDateStart, DateTime dueDateEnd, string supplier)
        {
            return await _dbContext.Products.Include(p => p.Supplier)
                .Where(p =>
                p.ManufacturingDate > manufacturingDateStart && p.ManufacturingDate < manufacturingDateEnd &&
                p.DueDate > dueDateStart && p.DueDate < dueDateEnd &&
                p.Supplier.Description.Contains(supplier) &&
                p.Description.Contains(criteria)).Skip((page - 1) * amount).Take(amount).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> ProductExistsAsync(int id)
        {
            return await _dbContext.Products.AnyAsync(p => p.Id == id);
        }

        public async Task UpdateProductAsync(Product product)
        {
            _dbContext.Update(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}
