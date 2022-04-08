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
    }
}
