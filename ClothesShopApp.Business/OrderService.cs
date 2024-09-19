using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using ClothesShopApp.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Business
{
	public class OrderService
	{
		private readonly IRepository<Order> _repository;
		public OrderService(IRepository<Order> repository) 
		{
			_repository = repository;
		}

		public List<Order> GetOrdersByUserId(int userId) 
		{
			return GetAllOrders().Include(o=> o.orderItems).ThenInclude(op=>op.product).Where(u=> u.userId == userId).ToList();
		}

		public List<OrderViewModel> GetOrderByUserIdViewMdel(int userId,int page,int pageSize,int take)
		{
            var orders = GetAllOrders()
			.Include(o => o.orderItems)
			.ThenInclude(oi => oi.product)
			.Where(o => o.userId == userId)
			.ToList();


            var orderViewModels = orders.Skip((page-1)* pageSize).Take(take).Select(order => new OrderViewModel
            {
                OrderId = order.orderId,
                TotalPrice = order.totalPrice,
                OrderDateTime = order.OrderDateTime,
				OrderStatus = order.OrderStatus,
                OrderItems = order.orderItems.Select(orderItem => new OrderItemViewModel
                {
                    OrderItemId = orderItem.orderItemId,
                    ProductId = orderItem.productId,
                    ProductName = orderItem.product?.name, // Ürün adını ekle (null check ile)
                    Quantity = orderItem.quantity,
                    Price = orderItem.price,
                    ImageUrl = orderItem.ImageUrl
                }).ToList()
            }).ToList();
			return orderViewModels;
        }

		public List<AdminOrderUserItemsViewModel> GetUsersOrders()
		{
			return GetAllOrders()
				.Include(u => u.user)
				.Include(o => o.orderItems)
				.ThenInclude(p => p.product)
				.Select(item => new AdminOrderUserItemsViewModel
				{
					OrderId = item.orderId,
					User = item.user,
					OrderStatus = item.OrderStatus,
					OrderDate = item.OrderDateTime.ToString("yyyy-MM-dd HH-mm-ss"),
					TotalPrice = item.totalPrice.ToString(),
					Items = item.orderItems.Select(o => new OrderItemViewModel
					{
						ProductName = o.product.name,
						Quantity = o.quantity,
						Price = o.price,
						ImageUrl = o.ImageUrl,

					}).ToList()
				}).ToList();
		}

		public AdminOrderUserItemsViewModel? GetUserOrderDetail(int id)
		{
			return GetAllOrders().Where(o => o.orderId == id)
				.Include(u => u.user)
				.Include(o => o.orderItems)
				.ThenInclude(p => p.product)
				.Select(item => new AdminOrderUserItemsViewModel
				{
					OrderId = item.orderId,
					User = item.user,
					OrderStatus = item.OrderStatus,
					OrderDate = item.OrderDateTime.ToString("yyyy-MM-dd HH-mm-ss"),
					TotalPrice = item.totalPrice.ToString(),
					address = item.address,
					Items = item.orderItems.Select(o => new OrderItemViewModel
					{
						ProductName = o.product.name,
						Quantity = o.quantity,
						Price = o.price,
						ImageUrl = o.ImageUrl,

					}).ToList()
				}).FirstOrDefault();
		}
		
		public IQueryable<Order> GetAllOrders()
		{
			return _repository.GetAll();
		}
		public Order GetOrderById(int id)
		{
			return _repository.GetById(id);
		}
		public void DeleteOrder(int id)
		{
			_repository.Delete(id);
		}
		public void UpdateOrder(Order order)
		{

			_repository.Update(order);
		}
		public void CreateOrder(Order order)
		{
			_repository.Create(order);
		}
	}
}
