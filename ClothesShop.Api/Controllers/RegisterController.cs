using ClothesShop.Api.Response;
using ClothesShopApp.Business;
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ClothesShop.Api.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class RegisterController : ControllerBase
	{
		private readonly UserService _userService;
		private readonly LocationService _locationService;
		private readonly AddressService _addressService;
		private readonly ILogger<RegisterController> _logger;
		public RegisterController(UserService userService, LocationService locationService, AddressService addressService,ILogger<RegisterController> logger)
		{
			_userService = userService;
			_locationService = locationService;
			_addressService = addressService;
			_logger = logger;
		}
		[HttpGet("cities")]
		public IActionResult GetCities()
		{
			var cities = _locationService.GetAllCities();
			return Ok(cities);
		}

		// GET api/register/districts/{cityId}
		[HttpGet("districts/{cityId}")]
		public IActionResult GetDistricts(int cityId)
		{
			var districts = _locationService.GetAllDistrict(cityId);
			return Ok(districts);
		}

		[HttpPost]
		public IActionResult Index([FromBody] RegisterViewModel model)
		{
			var newModel = new RegisterViewModel();
			try
			{
				_logger.LogInformation("registering started");
                if (model != null)
                {
                    var emailExists = _userService.GetAllUser().Any(e => e.email == model.email);
                    if (emailExists)
                    {
						_logger.LogError("Bu e-posta kullanılıyor.");
                        return Conflict(new ApiResponse<string>(false, "Bu e-posta kullanılıyor.",null));

                    }
                    if (model.intentityNumber != null)
                    {
                        var intetntiy = _userService.GetAllUser().Any(i => i.IdentityNumber == model.intentityNumber);
                        if (intetntiy)
                        {
                            _logger.LogError("Bu kimlik numarası kayıtlı.");
                            return Conflict(new { message = "Bu kimlik numarası kayıtlı." });

                        }
                    }

                    // Telefon kontrolü
                    if (model.phone.Length == 10)
                    {
                        var phoneExists = _userService.GetAllUser().Any(e => e.phone == model.phone);
                        if (phoneExists)
                        {
                            _logger.LogError("Bu telefon numarası zaten kayıtlı.");
                            return Conflict(new { message = "Bu telefon numarası zaten kayıtlı." });

                        }
                    }
                    else
                    {
                        _logger.LogError("Telefon numarası eksik veya hatalı.");
                        return BadRequest(new { message = "Telefon numarası eksik veya hatalı." });
                    }


                    if (model.password != model.password1)
                    {
                        _logger.LogError("Şifreler aynı değil.");
                        return BadRequest(new { message = "Şifreler aynı değil." });
                    }





                    var user = _userService.ConvertModelToUser(model);
                    _userService.CreateUser(user);


                    //var address = _addressService.ConverModelToAddress(model);
                    //address.userId = user.userId;
                    //_addressService.CreateAddress(address);

                    return Ok(new ApiResponse<User>(true,"ok",user));

                }
            }
			catch (Exception ex) 
			{
				return StatusCode(500, new ApiResponse<string>(false, ex.Message, null));
			}
			


				


			return Ok(model);
		}
		
	}
}
