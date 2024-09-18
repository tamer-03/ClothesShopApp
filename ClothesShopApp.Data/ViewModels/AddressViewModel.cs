using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages.Html;

namespace ClothesShopApp.Data.ViewModels
{
	public class AddressViewModel
	{
		public int SelectedAddressId { get; set; } 
		public List<SelectListItem> Addresses { get; set; }
		public string SelectedCart { get; set; }
		public  List<SelectListItem> Carts { get; set; }

	}
}
