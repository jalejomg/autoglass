using AutoFixture;
using Autoglass.Data.Entities;
using Autoglass.Data.Repositories;
using Autoglass.Services.Models;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Autoglass.Services.Tests
{
    public class ProductsServiceTests
    {
        private ProductsService CreateSut(
            IProductsRepository _productsRepository = null,
            IMapper _mapper = null)
        {
            return new ProductsService(
                _productsRepository ?? Mock.Of<IProductsRepository>(),
                _mapper ?? Mock.Of<IMapper>());
        }

        [Fact]
        public async Task Should_create_new_product()
        {
            // Arrange
            var fixture = new Fixture();
            var productModel = fixture.Create<ProductModel>();
            productModel.ManufacturingDate = DateTime.Now;
            productModel.DueDate = DateTime.Now.AddMinutes(1);

            var productEntity = new Product()
            {
                Id = productModel.Id,
                Description = productModel.Description,
                ManufacturingDate = productModel.ManufacturingDate,
                DueDate = productModel.DueDate
            };

            var _mapperMock = new Mock<IMapper>();

            _mapperMock
                .Setup(m => m.Map<Product>(productModel))
                .Returns(productEntity);

            var _productsRepositoryMock = new Mock<IProductsRepository>();

            var sut = CreateSut(_productsRepositoryMock.Object, _mapperMock.Object);

            // Act
            await sut.CreateAsync(productModel);

            // Assert
            _productsRepositoryMock
                .Verify(p => p.CreateProductAsync(productEntity), Times.Once);
        }

        [Fact]
        public async Task Should_not_create_product_because_due_date_is_greater_than_manufacturing_date()
        {
            // Arrange
            var fixture = new Fixture();
            var productModel = fixture.Create<ProductModel>();
            productModel.DueDate = DateTime.Now;
            productModel.ManufacturingDate = DateTime.Now.AddMinutes(1);

            var productEntity = new Product()
            {
                Id = productModel.Id,
                Description = productModel.Description,
                ManufacturingDate = productModel.ManufacturingDate,
                DueDate = productModel.DueDate
            };

            var _mapperMock = new Mock<IMapper>();

            _mapperMock
                .Setup(m => m.Map<Product>(productModel))
                .Returns(productEntity);

            var _productsRepositoryMock = new Mock<IProductsRepository>();

            var sut = CreateSut(_productsRepositoryMock.Object, _mapperMock.Object);

            // Act and Assert
            await Assert.ThrowsAnyAsync<InvalidOperationException>(async () => await sut.CreateAsync(productModel));

            _productsRepositoryMock
                .Verify(p => p.CreateProductAsync(productEntity), Times.Never);
        }

    }
}
