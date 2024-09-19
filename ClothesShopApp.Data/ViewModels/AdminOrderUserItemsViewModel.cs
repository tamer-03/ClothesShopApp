using ClothesShopApp.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Data.ViewModels
{
    
    public class AdminOrderUserItemsViewModel
    {
        public int OrderId { get; set; } 
        public User User { get; set; }
            
        public string OrderDate {  get; set; }
        public string OrderStatus {  get; set; }
        public string TotalPrice {  get; set; }
        public Address address { get; set; }
        
        public List<OrderItemViewModel> Items { get; set; }
    }
}
