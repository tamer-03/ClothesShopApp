using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Data.Entity
{
    public class Size
    {
        public int sizeId {  get; set; }
        public string Name { get; set; } // Örneğin "S", "M", "L", "36", "37", "42" vb.
        public ICollection<ProductSize> ProductSizes { get; set; }
    }
}
