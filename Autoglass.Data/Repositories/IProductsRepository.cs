using Autoglass.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autoglass.Data.Repositories
{
    public interface IProductsRepository
    {
        Task<int> CreateProductAsync(Product product);
        Task<IList<Product>> GetAllProductsAsync(int amount, int page, string criteria, DateTime ManufacturingDateStart, DateTime ManufacturingDateEnd,
            DateTime DueDateStart, DateTime DueDateEnd, string supplier);
        Task<Product> GetProductByIdAsync(int id);
        Task UpdateProductAsync(Product product);
        Task<bool> ProductExistsAsync(int id);
        Task DeleteProductAsync(Product product);
    }
}
