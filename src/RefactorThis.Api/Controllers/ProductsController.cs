using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RefactorThis.Api.Models;
using RefactorThis.Core.Interfaces;
using RefactorThis.Core.Models;

namespace RefactorThis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetAllResponse<Product>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProducts(string name = null) =>
             Ok(new GetAllResponse<Product>(await _productService.GetProducts(name)));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductById(Guid id) => Ok(await _productService.GetProduct(id));

        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct(CreateProductRequest newProductRequest)
        {
            var newProduct = await _productService.CreateProduct(newProductRequest);
            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductRequest updatedProduct)
        {
            if (id != updatedProduct.Id)
            {
                _logger.LogError("Cannot Update Product, product {id} in path does not math {@payload}", id, updatedProduct);
                return BadRequest(new ErrorResponse("product id in payload must match id in path"));
            }
            await _productService.UpdateProduct(updatedProduct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _productService.DeleteProduct(id);
            return NoContent();
        }

        [HttpGet("{productId}/options")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductOptions(Guid productId) =>
            Ok(new GetAllResponse<ProductOption>(await _productService.GetProductOptions(productId)));

        [HttpGet("{productId}/options/{id}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductOption(Guid productId, Guid id) => Ok(await _productService.GetProductOption(productId, id));

        [HttpPost("{productId}/options")]
        [ProducesResponseType(typeof(ProductOption), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProductOption(Guid productId, CreateProductOptionRequest newProductOptionRequest)
        {
            var newProductOption = await _productService.AddOptionToProduct(productId, newProductOptionRequest);
            return CreatedAtAction(nameof(GetProductOption), new { productId, id = newProductOption.Id }, newProductOption);
        }

        [HttpPut("{productId}/options/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProductOption(Guid productId, Guid id, UpdateProductOptionRequest updatedOption)
        {
            if (id != updatedOption.Id)
            {
                _logger.LogError("Cannot Update Option, option {id} in path does not math {@payload}", id, updatedOption);
                return BadRequest(new ErrorResponse("Option id in payload must match id in path"));
            }
            await _productService.UpdateProductOption(productId, updatedOption);
            return NoContent();
        }

        [HttpDelete("{productId}/options/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProductOption(Guid productId, Guid id)
        {
            await _productService.DeleteProductOption(productId, id);
            return NoContent();
        }
    }
}