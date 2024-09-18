

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Data.Entity
{
	public class User
	{
		public int userId { get; set; }
		public string name { get; set; }
		public string lastName { get; set; }
		public string password { get; set; }
		public string email { get; set; }
		public bool role { get; set; }
		public string phone { get; set; }
		public DateTime LastLoginDate { get; set; }
		public string IdentityNumber { get; set; }

        public DateTime RegistrationDate { get; set; }
		public ICollection<Order> orders { get; set; }
		public ICollection<Address> addresss { get; set; }
		public ICollection<Like> likes { get; set; }
		public ICollection<Review> reviews { get; set; }

	}

	
}
