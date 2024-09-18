using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Data.Entity
{
	public class OrderItem
	{
		public int orderItemId { get; set; }
		public int productId { get; set; }
		public int orderId { get; set; }
		public int quantity { get; set; }
		public decimal price { get; set; }
		public string ImageUrl { get; set; }
		public Product product { get; set; }
		public Order order { get; set; }
	}
}
