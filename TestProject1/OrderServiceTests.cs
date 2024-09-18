using ClothesShopApp.Business;
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShop.Business.Test
{
	public class OrderServiceTests
	{
		private readonly OrderService _orderService;
		private readonly Mock<IRepository<Order>> _repositoryMock;


        public OrderServiceTests()
        {
            _repositoryMock = new Mock<IRepository<Order>>();
            _orderService = new OrderService(_repositoryMock.Object);
        }
		public List<Order> GetOrders()
		{
			
			return new List<Order>
			{
				new Order
				{
					orderId = 1,
					user = new User { userId = 1, name = "John Doe" },
					OrderStatus = "Shipped",
					OrderDateTime = new DateTime(2024, 09, 10),
					totalPrice = 100,
					orderItems = new List<OrderItem>
					{
						new OrderItem
						{
							product = new Product { name = "T-Shirt" },
							quantity = 2,
							price = 50.25m,
							ImageUrl = "images/tshirt.jpg"
						}
					}
				},
				new Order
				{
					orderId = 2,
					user = new User { userId = 1, name = "John Doe" },
					OrderStatus = "hazırlanıyor",
					OrderDateTime = new DateTime(2024, 09, 10),
					totalPrice = 100.50m,
					orderItems = new List<OrderItem>
					{
						new OrderItem
						{
							product = new Product { name = "pantolon" },
							quantity = 2,
							price = 50.25m,
							ImageUrl = "images/tshirt.jpg"
						}
					}
				},new Order
				{
					orderId = 3,
					user = new User { userId = 1, name = "John Doe" },
					OrderStatus = "hazırlanıyor",
					OrderDateTime = new DateTime(2024, 09, 10),
					totalPrice = 100.50m,
					orderItems = new List<OrderItem>
					{
						new OrderItem
						{
							product = new Product { name = "pantolon" },
							quantity = 2,
							price = 50.25m,
							ImageUrl = "images/tshirt.jpg"
						}
					}
				}

			};
		}

        [Fact]
        public void GetOrdersByUserId_Test()
        {

            var order = new List<Order>
            {
                new Order
                {
                    orderId = 1,
                    userId = 1,
                    totalPrice = 123,
                },

				new Order
				{
					orderId = 2,
					userId = 1,
					totalPrice = 103,
				}
			};

            _repositoryMock.Setup(repo=> repo.GetAll()).Returns(order.AsQueryable);

            var result = _orderService.GetOrdersByUserId(1);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(123, result[0].totalPrice);
        }


        [Fact]
        public void GetUsersOrders_Test() 
        {
			var order = GetOrders();

			_repositoryMock.Setup(repo=> repo.GetAll()).Returns(order.AsQueryable);

			var result = _orderService.GetUsersOrders();

			// Assert
			Assert.NotNull(result); 
			Assert.Equal(3, result.Count);
			
			Assert.Equal("John Doe", result[0].User.name); // Kullanıcı adı doğru olmalı
			Assert.Equal("Shipped", result[0].OrderStatus); // Sipariş durumu doğru olmalı
			Assert.Equal("2024-09-10 00-00-00", result[0].OrderDate); // Sipariş tarihi doğru formatta olmalı
			Assert.Equal("100", result[0].TotalPrice); // Toplam fiyat doğru olmalı


			Assert.Single(result[0].Items);
			Assert.Equal("T-Shirt", result[0].Items[0].ProductName);
			Assert.Equal("pantolon", result[1].Items[0].ProductName);
			Assert.Equal(2, result[0].Items[0].Quantity); 
			Assert.Equal(50.25m, result[0].Items[0].Price); 
			Assert.Equal("images/tshirt.jpg", result[0].Items[0].ImageUrl);
		}

		//[Fact]
		//public void GetOrderByUserIdViewMdel_Test()
		//{
		//	var order = GetOrders();

		//	_repositoryMock.Setup(repo => repo.GetAll()).Returns(order.AsQueryable);

		//	var result = _orderService.GetOrderByUserIdViewMdel(1, 1, 1, 2);

		//	Assert.NotNull(result);
		//	Assert.Equal(2, result[0].OrderId);
		//	Assert.Equal("hazırlanıyor", result[0].OrderStatus);
		//	Assert.Equal(3, result[1].OrderId);


		//}

		[Fact]
		public void GetUserOrderDetail_Test()
		{
			var order = GetOrders();

			_repositoryMock.Setup(repo=> repo.GetAll()).Returns(order.AsQueryable);

			var result = _orderService.GetUserOrderDetail(1);

			Assert.NotNull(result);
			Assert.Equal(1,result.OrderId);
			Assert.Equal("Shipped",result.OrderStatus);
			Assert.Equal("100", result.TotalPrice);

		}
	}
}
