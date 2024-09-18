
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ClothesShopApp.Data.Entity
{
	public class Product
	{
		public int productId { get; set; }
		
		public int? categoryId { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public decimal price { get; set; }
		public int stock { get; set; }
		
		public Category categories { get; set; }
		public ICollection<ProductImage> productImages { get; set; }
		public ICollection<Like> likes { get; set; }
		public ICollection<Review> reviews { get; set; }
		public ICollection<OrderItem> orderItems { get;set; }
        public ICollection<ProductSize> ProductSizes { get; set; }
		public ICollection<ProductVariant> ProductVariants { get; set; }

	}
}
