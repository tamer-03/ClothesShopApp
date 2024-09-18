using ClothesShopApp.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Data.ViewModels
{
    public class ReviewViewModel
    {
        
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public string ReviewDate { get; set; }
        //public int ProductId { get; set; }
        public Product product { get; set; }

    }
}
