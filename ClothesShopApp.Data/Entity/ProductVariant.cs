using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Data.Entity
{
	public class ProductVariant
	{
		public int ProductVariantId { get; set; }
		public int ProductId { get; set; }
		public int VariantValueId { get; set; }
		public int Stock {  get; set; }
		public Product Product { get; set; }
		public VariantValue VariantValue { get; set; }
	}
}
