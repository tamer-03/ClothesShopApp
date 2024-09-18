using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Data.ViewModels
{
    public class ProductVariantsViewModel
    {
        public int ProductId { get; set; }
        public List<VariantTypeViewModel> VariantTypes { get; set; }
        
    }

    

}
