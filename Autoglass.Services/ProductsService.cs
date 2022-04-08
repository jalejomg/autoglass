using Autoglass.Data.Entities;
using Autoglass.Data.Repositories;
using Autoglass.Services.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autoglass.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IMapper _mapper;

        public ProductsService(
            IProductsRepository productsRepository,
            IMapper mapper)
        {
            _productsRepository = productsRepository;
            _mapper = mapper;
        }
        public async Task<int> CreateAsync(ProductModel productModel)
        {
            if (productModel.ManufacturingDate > productModel.DueDate)
                throw new InvalidOperationException($"Property {nameof(productModel.ManufacturingDate)} can not be greater than {nameof(productModel.DueDate)} ");

            var productEntity = _mapper.Map<Product>(productModel);

            return await _productsRepository.CreateProductAsync(productEntity);
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _productsRepository.GetProductByIdAsync(id);

            if (product == null || !product.Active)
                throw new Exception($"Product with id {id} does not exist");

            await _productsRepository.DeleteProductAsync(product);
        }

        public async Task<List<ProductModel>> GetAllAsync(int amount, int page, string criteria, DateTime manufacturingDateStart, DateTime manufacturingDateEnd,
            DateTime dueDateStart, DateTime dueDateEnd, string supplier)
        {
            var productEntities = await _productsRepository.GetAllProductsAsync(amount, page, criteria, manufacturingDateStart, manufacturingDateEnd,
                dueDateStart, dueDateEnd, supplier);

            var productModels = new List<ProductModel>();

            foreach (var productEntity in productEntities)
            {
                productModels.Add(_mapper.Map<ProductModel>(productEntity));
            }

            return productModels;
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            if (!await _productsRepository.ProductExistsAsync(id))
                throw new Exception($"Product with id {id} does not exist");

            return _mapper.Map<ProductModel>(await _productsRepository.GetProductByIdAsync(id));
        }

        public async Task UpdateAsync(ProductModel productModel)
        {
            if (productModel.ManufacturingDate > productModel.DueDate)
                throw new InvalidOperationException($"Property {nameof(productModel.ManufacturingDate)} can not be greater than {nameof(productModel.DueDate)} ");

            var product = await _productsRepository.GetProductByIdAsync(productModel.Id);

            if (product == null || !product.Active)
                throw new InvalidOperationException($"Product with id {productModel.Id} does not exist");

            await _productsRepository.UpdateProductAsync(product);
        }
    }
}
