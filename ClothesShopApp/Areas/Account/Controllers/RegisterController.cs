using ClothesShopApp.Business;
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Generators;
using System.Web.WebPages.Html;

namespace ClothesShopApp.Areas.Account.Controllers
{
    [Area("Account")]
    public class RegisterController : Controller
    {
        private readonly UserService _userService;
        private readonly LocationService _locationService;
        private readonly AddressService _addressService;
        public RegisterController(UserService userService, LocationService locationService, AddressService addressService)
        {
            _userService = userService;
            _locationService = locationService;
            _addressService = addressService;
        }
        [HttpGet]
        
        public IActionResult Index()
        {
            var model = new RegisterViewModel
            {
                cities = _locationService.GetAllCities()


            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(RegisterViewModel model)
        {
            var newModel = new RegisterViewModel();
            if (model != null)
            {
                var emailExists = _userService.GetAllUser().Any(e => e.email == model.email);
                if (emailExists)
                {
                    ModelState.AddModelError("email", "Bu e-posta kullanılıyor.");

                    
                    newModel.cities = _locationService.GetAllCities();
                    return View(newModel);
                }
                if(model.intentityNumber != null)
                {
                    var intetntiy = _userService.GetAllUser().Any(i=> i.IdentityNumber == model.intentityNumber);
                    if(intetntiy)
                    {
                        ModelState.AddModelError("intentityNumber", "Geçersiz kimlik");
                        newModel.cities = _locationService.GetAllCities();
                        return View(newModel);
                    }
                }

                // Telefon kontrolü
                if (model.phone.Length == 10)
                {
                    var phoneExists = _userService.GetAllUser().Any(e => e.phone == model.phone);
                    if (phoneExists)
                    {
                        ModelState.AddModelError("phone", "Bu telefon numarası zaten kayıtlı.");
                        newModel.cities = _locationService.GetAllCities();
                        return View(newModel);
                    }
                }else
                {
                    ModelState.AddModelError("phone", "telefon numarısın eksik ya da fazla girdiniz");
                    newModel.cities = _locationService.GetAllCities();
                    return View(newModel);
                }
                

                if(model.password != model.password1)
                {
                    ModelState.AddModelError("password", "Şifreler aynı değil");
                    newModel.cities = _locationService.GetAllCities();
                    return View(newModel);
                }


                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.password);


                var user = _userService.ConvertModelToUser(model);
                user.password = hashedPassword;
                _userService.CreateUser(user);


                //var address = _addressService.ConverModelToAddress(model);
                //address.userId = user.userId;
                //_addressService.CreateAddress(address);

                TempData["Succes"] = "başarılı";
                newModel.cities = _locationService.GetAllCities();
                newModel.districts = _locationService.GetAllDistrict(newModel.selectedCityId);
                return View(newModel);
            }


            model.cities = _locationService.GetAllCities();
            model.districts = _locationService.GetAllDistrict(model.selectedCityId);
            

            return View(model);
        }
        [HttpGet]
        public IActionResult GetDistricts(int cityId)
        {
            var districts = _locationService.GetAllDistrict(cityId);
            return Json(districts);
        }
    }
}
