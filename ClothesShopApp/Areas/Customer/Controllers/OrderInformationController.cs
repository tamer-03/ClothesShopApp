using ClothesShopApp.Business;
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClothesShopApp.Areas.Customer.Controllers
{
    [Authorize(Roles = "Customer")]
    [Area("Customer")]
    public class OrderInformationController : Controller
    {
        private readonly UserService _userService;
        private readonly OrderService _orderService;
        public OrderInformationController(UserService userService,OrderService orderService) 
        {
            _userService = userService;
            _orderService = orderService;
        }
        public User GetUser()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.GetUserById(Convert.ToInt32(currentUserId));
            return user;
        }

        public IActionResult Index()
        {
            var user = GetUser();
            // Başlangıçta sayfa boyutunu 5 olarak ayarlayın
            //var orders = _orderService.GetOrdersByUserId(user.userId)
            //    .Take(0)
            //    .ToList();
            var orders = _orderService.GetOrderByUserIdViewMdel(user.userId,1,5,5);
            return View(orders);

        }
        [HttpGet]
        public IActionResult LoadOrders(int page, int pageSize)
        {
            var user = GetUser();
            //var orders = _orderService.GetOrdersByUserId(user.userId)
            //    .Skip((page - 1) * pageSize)
            //    .Take(pageSize)
            //    .Select(order => new Order
            //    {
            //        orderId = order.orderId,
            //        totalPrice = order.totalPrice,
            //        orderItems = order.orderItems.Select(x => new OrderItem
            //        {
            //            product = x.product,
            //            price = x.price,
            //            ImageUrl = x.ImageUrl,
            //            quantity = x.quantity,
            //            order = x.order,
            //            orderId = order.orderId,
            //            productId = x.productId,
            //            orderItemId = x.orderItemId,

            //        }).ToList()
            //    }).ToList();

            var orders = _orderService.GetOrderByUserIdViewMdel(user.userId,page,pageSize,5);

            

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true,
            };
            return new  JsonResult(orders, options);
            //return new JsonResult(orders, options);
        }
    }
}
