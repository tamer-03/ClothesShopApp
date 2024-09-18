using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Data.ViewModels
{
    public class VariantTypeViewModel
    {
        public string VariantTypeName { get; set; }
        public List<int> ProductVariantId { get; set; }
        public List<string> Values { get; set; }
        public List<int> Stock { get; set; }
    }
}
