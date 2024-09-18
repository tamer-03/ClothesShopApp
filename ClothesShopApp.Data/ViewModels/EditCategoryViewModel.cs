using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Data.ViewModels
{
	public class EditCategoryViewModel
	{
		public int CategoryId { get; set; }
		public string? Name { get; set; }
		public string? Picture { get; set; }
		public int? ParentId { get; set; }
		public IFormFile UpdatedImages { get; set; }
	}
}
