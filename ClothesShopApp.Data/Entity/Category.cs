using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Data.Entity
{
    public class Category
    {
        public int categoryId { get; set; }
        public string? name { get; set; }
        public string? picture { get; set; }
        public int? parentCategoryId { get; set; }
        public Category? parentCategory { get; set; }
        public ICollection<Category>? parentIds { get; set; }
        public ICollection<Product>? products { get; set; }
    }
}
