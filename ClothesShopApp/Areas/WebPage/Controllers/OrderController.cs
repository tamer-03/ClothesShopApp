using ClothesShopApp.Business;
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClothesShopApp.Areas.WebPage.Controllers
{
    [Area("WebPage")]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        private readonly ProductService _productService;
        public OrderController(OrderService orderService,ProductService productService) 
        {
            _orderService = orderService;
            _productService = productService;
        }

        [HttpPost]
        public IActionResult Index([FromBody] List<CartItemViewModel> cartItems)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            

                var order = new Order
                {
                    userId = userId,
                    totalPrice = cartItems.Sum(item => item.Price * item.Quantity),
                    orderItems = cartItems.Select(item => new OrderItem
                    {
                        productId = item.ProductId,
                        quantity = item.Quantity,
                        price = item.Price,
                        ImageUrl = item.Img
                    }).ToList()
                };
                //_orderService.CreateOrder(order);
                //foreach (var item in cartItems)
                //{
                //    var product = _productService.GetProductById(item.ProductId);
                //    if (product != null)
                //    {
                //        product.stock -= item.Quantity;
                //        _productService.UpdateProduct(product);
                //    }
                //}
            

			//return RedirectToAction("Index","Product",new {area= "WebPage"});
			return RedirectToAction("Index", "Payment", new { order = order });
		}
    }
}
