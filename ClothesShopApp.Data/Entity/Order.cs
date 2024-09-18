
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Data.Entity
{
	public class Order
	{
		public int orderId { get; set; }
		public int userId { get; set; }
		public int? addressId { get; set; }
		
		public decimal totalPrice { get; set; }
		public User user { get; set; }
		public DateTime OrderDateTime { get; set; }
		public ICollection<OrderItem> orderItems { get; set; }
		public string OrderStatus { get; set; }
		public string ShippingAddress { get; set; }
		public Address address { get; set; }



	}
}
