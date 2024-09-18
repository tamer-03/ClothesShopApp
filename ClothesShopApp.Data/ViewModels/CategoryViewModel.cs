using Microsoft.AspNetCore.Http;
using System.Web.Mvc;

namespace ClothesShopApp.Data.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Picture { get; set; }
        public int? ParentId { get; set; }
        public List<SelectListItem>? Categories { get; set; }
        public List<CategoryViewModel> SubCategories { get; set; } = new List<CategoryViewModel>();
        public IFormFile UpdatedImages { get; set; }


		public int Level { get; set; }
    }
}
