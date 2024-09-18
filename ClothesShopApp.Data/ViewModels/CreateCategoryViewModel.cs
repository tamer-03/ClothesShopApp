using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages.Html;


namespace ClothesShopApp.Data.ViewModels
{
    public class CreateCategoryViewModel
    {
        public List<int?> SelectedCategoryIds { get; set; } = new List<int?>();

        // Dinamik dropdown listleri tutar, her bir dropdown list bir seviye kategoriyi temsil eder
        public List<List<SelectListItem>> CategoryLevels { get; set; } = new List<List<SelectListItem>>();

        public string Picture { get; set; }
        // Yeni kategori ismi
        public string? NewCategoryName { get; set; }
        public IFormFile? pictureFile { get; set; }
        
	}
}
