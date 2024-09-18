using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClothesShopApp.Data.Entity
{
    public class Location
    {
        [Key]
        public int locationId { get; set; }

        [Required]
        [MaxLength(100)]
        public string name { get; set; }

        public int? parentLocationId { get; set; }

        public Location parentLocation { get; set; }

        // Bir şehrin birden fazla ilçesi olabilir
        public ICollection<Location> subLocations { get; set; }
        public ICollection<Address> addresss { get; set; }
    }
}
