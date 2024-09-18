using ClothesShopApp.Business;
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClothesShopApp.Areas.Customer.Controllers
{
    [Authorize(Roles = "Customer")]
    [Area("Customer")]
    public class AddressInformationController : Controller
    {
        private readonly AddressService _addressService;
        private readonly UserService _userService;
        private readonly LocationService _locationService;
        public AddressInformationController(AddressService addressService, UserService userService,LocationService locationService)
        {
            _addressService = addressService;
            _userService = userService;
            _locationService = locationService;
        }
        public User GetUser()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.GetUserById(Convert.ToInt32(currentUserId));
            return user;
        }
        
        public IActionResult Index()
        {
            
            var user = GetUser();
            if (user != null)
            {
                var address = _addressService.GetAddressLocationList(user.userId);
                
                return View(address);
            }
            return View();
        }



        public IActionResult EditAddress(int id)
        {
            var address = _addressService.GetAddressLocationById(id);
            
            //var district = _locationService.GetAllLocation().Where(d => d.parentLocationId == address.Location.parentLocationId).ToList();
            
            var model = new EditAddressViewModel
            {
                Address = address,
                Cities = _locationService.GetAllCities(),
                Districts = _locationService.GetAllDistrict(address.Location.parentLocationId),
            };
            return View(model);

        }
        [HttpPost]
        public IActionResult EditAddress(EditAddressViewModel model)
        {
            var user = GetUser();
            var address = _addressService.AddAddressModelToAddress(model);
            address.userId = user.userId;
            _addressService.UpdateAddress(address);
            TempData["Success"] = "true";

            return RedirectToAction("Index");
        }

        public IActionResult AddAddress()
        {
            
            var model = new EditAddressViewModel
            {

                Cities = _locationService.GetAllCities()
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult AddAddress(EditAddressViewModel model)
        {
            
            var user = GetUser();
            var newAddress = _addressService.AddAddressModelToAddress(model);
            newAddress.userId = user.userId;

            _addressService.CreateAddress(newAddress);

            TempData["Success"] = "true";


            
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult GetDistricts(int cityId)
        {
            var districts = _locationService.GetAllLocation()
                             .Where(l => l.parentLocationId == cityId)
                             .Select(c => new SelectListItem
                             {
                                 Value = c.locationId.ToString(),
                                 Text = c.name
                             }).ToList();
            return Json(districts);
        }
    }
}
