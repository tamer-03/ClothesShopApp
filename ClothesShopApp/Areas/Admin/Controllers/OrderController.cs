using ClothesShopApp.Business;
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ClothesShopApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
       
        private readonly OrderService _orderService;
        public OrderController( OrderService orderService)
        {
            
            _orderService = orderService;
        }

        public IActionResult Index()
        {

            var model = _orderService.GetUsersOrders();


            return View(model);
        }

        public IActionResult OrderDetail(int id)
        {
            var model1 = _orderService.GetUserOrderDetail(id);
            //var model = _orderService.GetAllOrders().Where(o=>o.orderId == id)
            //    .Include(u => u.user)
                
            //    .Include(o => o.orderItems)
            //    .ThenInclude(p => p.product)
            //    .Select(item => new AdminOrderUserItemsViewModel
            //    {
            //        OrderId = item.orderId,
            //        ShippingAddress = item.ShippingAddress,
            //        User = item.user,
            //        OrderStatus = item.OrderStatus,
            //        OrderDate = item.OrderDateTime.ToString("yyyy-MM-dd/HH-mm-ss"),
            //        TotalPrice = item.totalPrice.ToString(),
            //        Items = item.orderItems.Select(o => new OrderItemViewModel
            //        {
            //            ProductName = o.product.name,
            //            Quantity = o.quantity,
            //            Price = o.price,
            //            ImageUrl = o.ImageUrl,

            //        }).ToList(),
            //    }).FirstOrDefault();

            return View(model1); 
        }

        public IActionResult UpdateOrderStatus(int orderId,string orderStatus)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order != null)
            {
                order.OrderStatus = orderStatus;
                _orderService.UpdateOrder(order);
                return Json(new { Success = true,newStatus = order.OrderStatus});
            }
            return Json(new {Success = false , messeage =  "Sipraiş bulunamadı"}); 
        }



    }
}
