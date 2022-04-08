using Autoglass.Services;
using Autoglass.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autoglass.Api.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductModel>>> GetAllWithPagination(DateTime manufacturingDateStart, DateTime manufacturingDateEnd,
            DateTime dueDateStart, DateTime dueDateEnd, string supplier = "", int amount = 10, int page = 1, string criteria = "")
        {
            return await _productsService.GetAllAsync(amount, page, criteria, manufacturingDateStart, manufacturingDateEnd,
                dueDateStart, dueDateEnd, supplier);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductModel>> GetById(int id)
        {
            try
            {
                return await _productsService.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                return NotFound();
            }

        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(ProductModel productModel)
        {
            try
            {
                return await _productsService.CreateAsync(productModel);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(ProductModel productModel)
        {
            try
            {
                await _productsService.UpdateAsync(productModel);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _productsService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
    }
}
