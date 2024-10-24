using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public CatalogController(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentException(nameof(productRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepository.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _productRepository.GetProduct(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet("GetByCategory/{category}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category)
        {
            if (category == null)
                return BadRequest("Invalid category");

            var products = await _productRepository.GetProductByCategory(category);

            return Ok(products);
        }

        [HttpGet("GeyByName/{name}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByName(string name)
        {
            if (name == null)
                return BadRequest("Invalid category");

            var products = await _productRepository.GetProductByName(name);

            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            if (product == null)
                return BadRequest("Invalid product");

            await _productRepository.CreateProduct(product);

            return CreatedAtRoute("GetProductById", new {id = product.Id}, product);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            if (product == null)
                return BadRequest("Invalid product");

            return Ok(await _productRepository.UpdateProduct(product));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {

            return Ok(await _productRepository.DeleteProduct(id));
        }
    }
}
