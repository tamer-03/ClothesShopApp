using ClothesShopApp.Business;
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Twilio.Rest.Bulkexports.V1.Export;

namespace ClothesShopApp.Areas.Customer.Controllers
{

    [Authorize(Roles = "Customer")]
    [Area("Customer")]
    public class ReviewController : Controller
    {
        private readonly ReviewService _reviewService;
        private readonly UserService _userService;
        private readonly ProductService _productService;
        public ReviewController(ReviewService reviewService, UserService userService, ProductService productService)
        {
            _reviewService = reviewService;
            _userService = userService;
            _productService = productService;
        }
        public User GetUser()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.GetUserById(Convert.ToInt32(currentUserId));
            return user;
        }
        [HttpGet]
        public IActionResult Index(int productId)
        {
            var product = _productService.GetProductById(productId);
            var model = new ReviewViewModel {product = product };
            return View(model);
        }

        [HttpPost]
        public IActionResult Index([FromBody] ReviewViewModel model)
        {
            var user = GetUser();

            if (!ModelState.IsValid)
            {
                var review = new Review
                {
                    productId = model.product.productId,
                    userId = user.userId, 
                    rating = model.Rating,
                    comment = model.Comment,
                    reviewDate = DateTime.Now
                };

                _reviewService.CreateReview(review);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Validation failed" });

        }
    }
}
