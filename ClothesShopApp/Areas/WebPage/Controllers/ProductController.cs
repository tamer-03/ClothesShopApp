using ClothesShopApp.Business;
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ClothesShopApp.Areas.WebPage.Controllers
{
    [Area("WebPage")]
    public class ProductController : Controller
    {
        private readonly CategoryService _categoryService;
        private readonly ProductService _productService;
        private readonly ReviewService _reviewService;
        public ProductController(CategoryService categoryService, ProductService productService, ReviewService reviewService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _reviewService = reviewService;
        }
        public IActionResult Index(int? categoryId)
        {
            var categories = _categoryService.GetCategoryHierarchy();
            IEnumerable<ProductViewModel> products;
            if (categoryId != null)
            {
                //kategori seçilmişse ona ait ürünleri listele
                products = _productService.GetProductsByCategory(categoryId.Value)
                    .Select(p=> new ProductViewModel
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        price = p.price,
                        PictureUrl = p.PictureUrl,
                        AverageRating = _productService.GetAverageRating(p.ProductId),
                        TotalReview = _reviewService.GetReviewCount(p.ProductId),
                    }).ToList();
                ViewBag.SelectedCategoryId = categoryId.Value;
            }
            else
            {
                products = _productService.ProductMapToViewModel(null)
                    .Select(p=> new ProductViewModel
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        price = p.price,
                        PictureUrl = p.PictureUrl,
                        AverageRating = _productService.GetAverageRating(p.ProductId),
                        TotalReview = _reviewService.GetReviewCount(p.ProductId)
                    }).ToList();

            }

            var model = new ProductListViewModel
            {
                Products = products,
                CategoryId = categoryId ?? 0,
                SubCategories = MapToViewModel(categories, 0),
            };
            return View(model);

        }
        private List<ProductListViewModel> MapToViewModel(List<Category> categories, int level)
        {


            return categories
                .Select(c => new ProductListViewModel
                {
                    CategoryId = c.categoryId,
                    CategoyName = c.name,
                    ParentId = c.parentCategoryId,
                    Level = level,
                    SubCategories = MapToViewModel(c.parentIds.ToList(), level + 1),


                })
                .ToList();

        }

        public IActionResult ProductDetail(int productId)
        {
            var product = _productService.GetSelectedProductDetails(productId);
            return View(product);
        }
        [HttpGet]
        public IActionResult GetMoreReviews(int productId, int skip , int pageSize = 3)
        {
            var reviews = _reviewService.GetReviewsByProductId(productId)
                .Skip(skip) // Sayfalama için
                .Take(pageSize)
                .ToList();

            var reviewDtos = reviews.Select(review => new ReviewViewModel
            {
                Rating = review.rating,
                Comment = review.comment,
                UserName = review.user.name,
                UserLastName = review.user.lastName,
                ReviewDate = review.reviewDate.ToString("yyyy-MM-dd")
            }).ToList();

            return Json(reviewDtos);
        }
        public IActionResult GetProducts(string sortOrder, decimal? minPrice, decimal? maxPrice,int? categoryId)
        {
            var products = _productService.GetAllProducts();
            if (categoryId.HasValue)
            {
                var subCategories = _categoryService.GetParentCategory(categoryId.Value);
                var categoryIds = subCategories.Select(c => c.categoryId).ToList();

                //products = products.Where(p => p.categoryId == categoryId.Value);
                products = products.Where(p => categoryIds.Contains(p.categoryId?? 1));

            }
            // Apply filtering by price
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.price >= minPrice.Value);
               
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.price <= maxPrice.Value);
            }

            // Apply sorting by price
            switch (sortOrder)
            {
                case "price_asc":
                    products = products.OrderBy(p => p.price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.price);
                    break;
                default:
                    break; 
            }
            
            var product = products.ToList();
            Console.WriteLine(product);
            
            var productList = _productService.ProductMapToViewModel(product);

            return Json(productList);
        }


        public IActionResult CardItem()
        {
            return View();
        }
    }
}