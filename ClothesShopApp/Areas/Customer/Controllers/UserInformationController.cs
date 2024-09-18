using ClothesShopApp.Business;
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClothesShopApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class UserInformationController : Controller
    {
        private readonly UserService _userService;
        public UserInformationController(UserService userService) 
        {
         _userService = userService;
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

            var model = _userService.UserInformationUserToModel(user);
            return View(model);
            
        }
        [HttpPost]
        public IActionResult UpdateUserInformation(CustomerInformationViewModel model)
        {

            var currentUser = GetUser();
            
            if (model.phone != currentUser.phone)
            {
                var phoneExist = _userService.GetAllUser().Any(e => e.phone == model.phone);
                if (phoneExist)
                {
                    return Json(new { success = false, errors = new[] { "Telefon numarası başka bir kullanıcıya ait" } });
                }

            }
            else if (model.email != currentUser.email)
            {
                var emailExist = _userService.GetAllUser().Any(e => e.email == model.email);
                if (emailExist)
                {
                    return Json(new { success = false, errors = new[] { "Bu email kullanılıyor" } });
                }
            }
            else if (model.identityNumber != currentUser.IdentityNumber) 
            {
                var identityExist = _userService.GetAllUser().Any(e => e.IdentityNumber == model.identityNumber);
                if (identityExist)
                {
                    return Json(new { success = false, errors = new[] { "Kimlik numarası başkasına ait" } });
                }
            }
            
            
            currentUser  = _userService.UserInformationModelToUser(model,currentUser);
           
            _userService.UpdateUser(currentUser);
            return Json(new {success = true});
        }
        [HttpPost]
            public IActionResult UpdateUserPassword(CustomerInformationViewModel model)
            {
                var user = GetUser();
                
                if (!BCrypt.Net.BCrypt.Verify(model.password,user.password) /*user.password != model.password*/)
                {
                    return Json(new { success = false, errors = new[] { "Mevcut şifreniz yanlış." } });
                    //kullanıcı şifresi ile girilen şifre eşit değilse hata mesajı
                }
                if (model.password1 != model.password2)
                {
                    return Json(new { success = false, errors = new[] { "Yeni şifreler eşleşmiyor." } });
                    //iki şifre değilse hata mesajı
                }
                var hashPassword = BCrypt.Net.BCrypt.HashPassword(model.password1);

                user.password = hashPassword;
                _userService.UpdateUser(user);

                return Json(new { success = true });
            }
    }
}
