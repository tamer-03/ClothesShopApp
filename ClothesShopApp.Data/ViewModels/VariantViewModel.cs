using ClothesShopApp.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ClothesShopApp.Data.ViewModels
{
    public class VariantViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string VariantValue { get; set; }
        public List<Variant> Variants { get; set; }
        public List<VariantValue>? VariantValues { get; set; }
        public List<VariantTypeViewModel> VariantTypes { get; set; }

    }
}
