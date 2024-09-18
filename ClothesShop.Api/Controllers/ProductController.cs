
using ClothesShop.Api.Response;
using ClothesShopApp.Business;
using ClothesShopApp.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClothesShop.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly ILogger<ProductController> _logger;
        public ProductController(ProductService productService,ILogger<ProductController> logger) 
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet("products")]
        public IActionResult GetProduct([FromQuery]int page =1,[FromQuery]int pageSize =2)
        {
            try
            {
                _logger.LogInformation("GetProduct method started");

                var totalItems = _productService.GetAllProducts().Count();
                var products = _productService.GetAllProducts().Skip((page - 1) * pageSize).Take(pageSize).ToList();
               // var totalItems = products.Count;
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                var productViewModel = _productService.ProductMapToViewModel(products);

                _logger.LogInformation("Product fetched succesfuly");



                return Ok(new PagedResponse<IEnumerable<ProductViewModel>>(true,"başarılı", productViewModel, totalItems, totalPages, page, pageSize));
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex,"error whlie fetching product");
                return StatusCode(500, new ApiResponse<string>(false, "ürünleri getirilirken sorun yaşandı", null));
            }
           
        }
    }
}
