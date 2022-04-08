using Autoglass.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autoglass.Services
{
    public interface IProductsService
    {
        Task<ProductModel> GetByIdAsync(int id);
        Task<List<ProductModel>> GetAllAsync(int amount, int page, string criteria, DateTime manufacturingDateStart, DateTime manufacturingDateEnd,
            DateTime dueDateStart, DateTime dueDateEnd, string supplier);
        Task<int> CreateAsync(ProductModel productModel);
        Task UpdateAsync(ProductModel productModel);
        Task DeleteAsync(int id);
    }
}
