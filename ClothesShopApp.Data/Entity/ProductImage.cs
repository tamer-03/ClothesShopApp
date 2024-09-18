using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Data.Entity
{
	public class ProductImage
	{
		public int productImageId { get; set; }
		public string url { get; set; }
		

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
