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

        [Fact]
        public async Task Should_delete_product()
        {
            // Arrange
            var fixture = new Fixture();
            var productId = fixture.Create<int>();
            var productEntity = fixture.Create<Product>();
            productEntity.Active = true;
            productEntity.Id = productId;

            var _productsRepositoryMock = new Mock<IProductsRepository>();

            _productsRepositoryMock
                .Setup(p => p.GetProductByIdAsync(productId))
                .ReturnsAsync(productEntity);

            var sut = CreateSut(_productsRepositoryMock.Object);

            // Act
            await sut.DeleteAsync(productId);

            // Assert
            _productsRepositoryMock
                .Verify(p => p.GetProductByIdAsync(productId), Times.Once);

            _productsRepositoryMock
                .Verify(p => p.DeleteProductAsync(productEntity), Times.Once);
        }

        [Fact]
        public async Task Should_not_delete_product_because_product_does_not_exist()
        {
            // Arrange
            var fixture = new Fixture();
            var productId = fixture.Create<int>();
            var productEntity = fixture.Create<Product>();
            productEntity.Active = false;
            productEntity.Id = productId;

            var _productsRepositoryMock = new Mock<IProductsRepository>();

            _productsRepositoryMock
                .Setup(p => p.GetProductByIdAsync(productId))
                .ReturnsAsync(productEntity);

            var sut = CreateSut(_productsRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await sut.DeleteAsync(productId));

            _productsRepositoryMock
                .Verify(p => p.GetProductByIdAsync(productId), Times.Once);

            _productsRepositoryMock
                .Verify(p => p.DeleteProductAsync(productEntity), Times.Never);
        }

        [Fact]
        public async Task Shoul_return_a_list_of_products()
        {
            // Arrange
            var fixture = new Fixture();
            var productsEntity = fixture.Create<List<Product>>();
            var productsModel = fixture.Create<ProductModel>();

            var _productsRepositoryMock = new Mock<IProductsRepository>();

            _productsRepositoryMock
                .Setup(p => p.GetAllProductsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .ReturnsAsync(productsEntity);

            var _mapperMock = new Mock<IMapper>();

            _mapperMock
                .Setup(m => m.Map<ProductModel>(productsEntity))
                .Returns(productsModel);


            var sut = CreateSut(_productsRepositoryMock.Object, _mapperMock.Object);

            // Act

            var result = await sut.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>());

            // Assert
            Assert.IsType<List<ProductModel>>(result);

            _productsRepositoryMock
                .Verify(p => p.GetAllProductsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Shoul_return_a_product()
        {
            // Arrange
            var fixture = new Fixture();
            var productEntity = fixture.Create<Product>();
            var productModel = fixture.Create<ProductModel>();

            var _productsRepositoryMock = new Mock<IProductsRepository>();

            _productsRepositoryMock
                .Setup(p => p.ProductExistsAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            _productsRepositoryMock
                .Setup(p => p.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(productEntity);

            var _mapperMock = new Mock<IMapper>();

            _mapperMock
                .Setup(m => m.Map<ProductModel>(productEntity))
                .Returns(productModel);

            var sut = CreateSut(_productsRepositoryMock.Object, _mapperMock.Object);

            // Act

            var result = await sut.GetByIdAsync(It.IsAny<int>());

            // Assert
            Assert.IsType<ProductModel>(result);

            _productsRepositoryMock
               .Verify(p => p.ProductExistsAsync(It.IsAny<int>()), Times.Once);

            _productsRepositoryMock
                .Verify(p => p.GetProductByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Shoul_not_return_a_product_because_it_does_not_exist()
        {
            // Arrange
            var _productsRepositoryMock = new Mock<IProductsRepository>();

            _productsRepositoryMock
                .Setup(p => p.ProductExistsAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            var sut = CreateSut(_productsRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await sut.GetByIdAsync(It.IsAny<int>()));

            _productsRepositoryMock
               .Verify(p => p.ProductExistsAsync(It.IsAny<int>()), Times.Once);

            _productsRepositoryMock
                .Verify(p => p.GetProductByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Should_update_a_product()
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

            var _productsRepositoryMock = new Mock<IProductsRepository>();

            _productsRepositoryMock
                .Setup(p => p.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(productEntity);

            var _mapperMock = new Mock<IMapper>();

            _mapperMock
                .Setup(m => m.Map<Product>(productModel))
                .Returns(productEntity);

            var sut = CreateSut(_productsRepositoryMock.Object, _mapperMock.Object);

            // Act
            await sut.CreateAsync(productModel);

            // Assert
            _productsRepositoryMock
                .Verify(p => p.GetProductByIdAsync(It.IsAny<int>()), Times.Once);

            _productsRepositoryMock
                .Verify(p => p.UpdateProductAsync(productEntity), Times.Once);
        }

        [Fact]
        public async Task Should_not_update_product_because_due_date_is_greater_than_manufacturing_date()
        {
            // Arrange
            var fixture = new Fixture();
            var productId = fixture.Create<int>();
            var productModel = fixture.Create<ProductModel>();
            productModel.Id = productId;
            productModel.DueDate = DateTime.Now;
            productModel.ManufacturingDate = DateTime.Now.AddMinutes(1);

            var _productsRepositoryMock = new Mock<IProductsRepository>();

            var sut = CreateSut(_productsRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAnyAsync<InvalidOperationException>(async () => await sut.CreateAsync(productModel));

            _productsRepositoryMock
                .Verify(p => p.GetProductByIdAsync(productId), Times.Never);

            _productsRepositoryMock
                .Verify(p => p.UpdateProductAsync(It.IsAny<Product>()), Times.Never);

        }

        [Fact]
        public async Task Should_not_update_product_because_it_does_not_exist()
        {
            // Arrange
            var fixture = new Fixture();
            var productId = fixture.Create<int>();
            var productModel = fixture.Create<ProductModel>();
            productModel.Id = productId;
            productModel.ManufacturingDate = DateTime.Now;
            productModel.DueDate = DateTime.Now.AddMinutes(1);

            var _productsRepositoryMock = new Mock<IProductsRepository>();

            _productsRepositoryMock
                .Setup(p => p.GetProductByIdAsync(productModel.Id))
                .ReturnsAsync((Product)null);

            var sut = CreateSut(_productsRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAnyAsync<InvalidOperationException>(async () => await sut.UpdateAsync(productModel));

            _productsRepositoryMock
                .Verify(p => p.GetProductByIdAsync(productModel.Id), Times.Once);

            _productsRepositoryMock
                .Verify(p => p.UpdateProductAsync(It.IsAny<Product>()), Times.Never);
        }
    }
}
