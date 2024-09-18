using ClothesShopApp.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Data.ViewModels
{
	public class PaymentItemsViewModel
	{
		public List<PaymentItemViewModel> Item { get; set; } = new List<PaymentItemViewModel>();

		public Address Address { get; set; }

	}
}
