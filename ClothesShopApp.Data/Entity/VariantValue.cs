using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Data.Entity
{
	public class VariantValue
	{
		public int VariantValueId { get; set; }
		public int VariantId { get; set; }
		public string Value { get; set; }
		public Variant Variant { get; set; }
		public ICollection<ProductVariant> ProductVariant { get; set; }
	}
}
