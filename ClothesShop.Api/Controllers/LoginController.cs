using ClothesShop.Api.Response;
using ClothesShopApp.Business;
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ClothesShop.Api.Controllers
{
	
	[ApiController]
	[Route("[controller]")]
	public class LoginController : ControllerBase
	{
		private readonly UserService _userService;
		private readonly ILogger<LoginController> _logger;	

		public LoginController(UserService userService,ILogger<LoginController> logger)
		{
			_userService = userService;
			_logger = logger;
		}

		// API üzerinden Login işlemi
		[HttpPost]
		public IActionResult Login([FromBody] LoginViewMdoel model)
		{
			try
			{
				_logger.LogInformation("login started");
				if (model == null)
				{
					_logger.LogError("geçersiz istek/boş veri");
					return BadRequest(new ApiResponse<string>(false,"geçersiz istek",null));
					
				}

				var user = _userService.ValidateUser(model.email);
				if (user == null)
				{
					_logger.LogError("e-posta veya şifre hatalı");
					return Unauthorized(new ApiResponse<string>(false, "E-posta veya şifre hatalı.",null));
				}

				// JWT token veya başka bir oturum yönetim mekanizması ekleyebilirsiniz
				

				// Son başarılı giriş tarihini güncelle
				user.LastLoginDate = DateTime.Now;
				_userService.UpdateUser(user);
				
				// Kullanıcı bilgilerini döndür
				return Ok(new ApiResponse<User>(true,"ok",user));
			}
			catch (Exception ex) 
			{
				return StatusCode(500, new ApiResponse<string>(false, ex.Message,null));
			}
			

		}
	}
}
