using ClothesShopApp.Business;
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Web.WebPages.Html;

namespace ClothesShopApp.Areas.WebPage.Controllers
{
    [Area("WebPage")]
    public class CartController : Controller
	{
		private readonly AddressService _addressService;
		private readonly UserService _userService;
        public CartController(AddressService addressService, UserService userService)
        {
            _addressService = addressService;
			_userService = userService;
        }
        public IActionResult Index()
		{
			var model = new AddressViewModel();
			var user = GetUser();
			if (user != null)
			{
				var address = _addressService.GetAllAddress()
					.Where(u => u.userId == user.userId)
					.Include(l => l.Location)
					.ThenInclude(p => p.parentLocation).ToList();

				model = new AddressViewModel
				{
					Addresses = address.Select(a => new SelectListItem
					{
						Value = a.addressId.ToString(),
						Text = $"{a.addressHeader},{a.Location.parentLocation.name}, {a.Location.name}, {a.addresses}"
					}).ToList()
				};
				return View(model);
			}
			
			return View(model);
		}

		

        public User GetUser()
		{
			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = _userService.GetUserById(Convert.ToInt32(currentUserId));
			return user;
		}

		
	}
}
