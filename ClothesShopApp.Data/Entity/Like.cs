
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Data.Entity
{
    public class Like
    {
        public int likeId { get; set; }
        public int productId { get; set; }
        public int userId { get; set; }

        public User user { get; set; }
        public Product product { get; set; }
    }
}
