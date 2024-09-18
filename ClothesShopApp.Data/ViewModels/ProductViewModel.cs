using ClothesShopApp.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ClothesShopApp.Data.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CategoryName { get; set; }
        public decimal price { get; set; }
        public int stock { get; set; }
        public List<int> PictureIds { get; set; }
        
        public double AverageRating { get; set; }
        public int TotalReview {  get; set; }
        public int? CategoryId { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public string categoryPath  { get; set; }
        public List<int?> SelectedCategoryIds { get; set; } = new List<int?>();

        // Dinamik dropdown listleri tutar, her bir dropdown list bir seviye kategoriyi temsil eder
        public List<List<SelectListItem>> CategoryLevels { get; set; } = new List<List<SelectListItem>>();
        public List<string> PictureUrl { get; set; } = new List<string>();
    }
}
