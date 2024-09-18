using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Data.ViewModels
{
	public class PaymentRequestViewModel
	{
		public List<CartItemViewModel> Cart { get; set; }
		public int SelectedAddressId { get; set; }
	}
}
