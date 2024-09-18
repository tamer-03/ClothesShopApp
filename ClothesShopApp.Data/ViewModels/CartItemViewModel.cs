using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Data.ViewModels
{
	public class CartItemViewModel
	{
		public int ProductId { get; set; }
		public int UserId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
		public string Size { get; set; }
		public string Img { get; set; }

		public int SelectedAddressId { get; set; }
	}
}
