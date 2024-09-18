using ClothesShopApp.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ClothesShopApp.Data.ViewModels
{
	public class ProductDetailViewModel
	{
		public string? Name { get; set; }
		public string? Description { get; set; }
		public decimal price { get; set; }
		public string categoryPath { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public List<string> PictureUrl { get; set; } = new List<string>();
		public int ProductId { get; set; }
		public List<SelectListItem> Sizes { get; set; } // SelectListItem yerine List<string> kullanıyoruz
		public int SelectedSizeId { get; set; }
		public Dictionary<string, List<SelectListItem>> Variants { get; set; }
	}
}
