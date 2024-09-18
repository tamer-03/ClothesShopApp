using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Data.Entity
{
    public class Address
    {
        public int addressId { get; set; }
        public int LocationId { get; set; } 
        public Location Location { get; set; }
        public string addresses { get; set; }
        public string addressHeader { get; set; }
        public int userId { get; set; }
        public string phone {  get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public User user { get; set; }
        public ICollection<Order> orders { get; set; }

    }
}
