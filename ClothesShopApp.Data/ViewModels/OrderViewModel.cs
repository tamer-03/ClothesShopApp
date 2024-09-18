using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Data.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDateTime { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
        public string OrderStatus { get; set; }
    }
}
