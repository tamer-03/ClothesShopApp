using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Data.Entity
{
	public class Variant
	{
		public int VariantId { get; set; }
		public string Name { get; set; }
		public ICollection<VariantValue> VariantValues { get; set; }
	}
}
