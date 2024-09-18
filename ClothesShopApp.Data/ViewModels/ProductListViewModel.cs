using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Data.ViewModels
{
	public class ProductListViewModel
	{

		public IEnumerable<ProductViewModel> Products { get; set; }
        
        public int CategoryId { get; set; }
		public List<Category> Categories { get; set; }
		public string CategoyName { get; set; }
		public int? ParentId { get; set; }
		public List<ProductListViewModel> SubCategories { get; set; } = new List<ProductListViewModel>();

		public int Level { get; set; }
	}
}
