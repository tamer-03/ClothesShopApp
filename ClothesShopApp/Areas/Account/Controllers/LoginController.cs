using ClothesShopApp.Business;
using ClothesShopApp.Data.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using ClothesShopApp.Areas.Account.Models;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Memory;


namespace ClothesShopApp.Areas.Account.Controllers
{
    [Area("Account")]
    public class LoginController : Controller
    {
        private readonly UserService _userService;
        private readonly JwtTokenService _jwtTokenService;
        private readonly EmailSender _emailSender;
        private readonly SmsService _smsService;
        private readonly IMemoryCache _memoryCache;

        public LoginController(UserService userService, EmailSender emailSender, JwtTokenService jwtTokenService, SmsService smsService, IMemoryCache memoryCache)
        {
            _emailSender = emailSender;
            _userService = userService;
            _jwtTokenService = jwtTokenService;
            _smsService = smsService;
            _memoryCache = memoryCache;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewMdoel model)
        {
            if (model != null)
            {

                var user = _userService.ValidateUser(model.email);


                if (user == null || !BCrypt.Net.BCrypt.Verify(model.password, user.password))
                {
                    ModelState.AddModelError("email", "E-post veya şifre hatalı");
                }
                else
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.userId.ToString()),
                        new Claim(ClaimTypes.Role, user.role ? "Admin" : "Customer")
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.rememberMe,
                    };
                    // Oturum açma işlemi
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    if (user.role == false)
                    {
                        user.LastLoginDate = DateTime.Now;
                        _userService.UpdateUser(user);
                        return RedirectToAction("Index", "HomePage", new { area = "WebPage" });

                    }
                    else
                    {
                        user.LastLoginDate = DateTime.Now;
                        _userService.UpdateUser(user);
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }

                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {


            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}
            if (!string.IsNullOrEmpty(model.Email))
            {
                var user = await _userService.GetAllUser().FirstOrDefaultAsync(a => a.email == model.Email);

                if (user == null)
                {
                    return RedirectToAction("ForgotPasswordConfirmation", new { inputEmail = model.Email });
                }
                var token = _jwtTokenService.GeneratePasswordResetToken(user.email);

                var resetLink = Url.Action("ResetPassword", "Login", new { token, email = user.email }, Request.Scheme);

                await _emailSender.SendPasswordResetLinkAsync(user, model.Email, resetLink);

                return RedirectToAction("ForgotPasswordConfirmation", new { email = user.email });
            }
            else if (!string.IsNullOrEmpty(model.Phone))
            {
                var user = await _userService.GetAllUser().FirstOrDefaultAsync(p => p.phone == model.Phone);
                if (user != null)
                {

                    var resetCode = _smsService.GenerateResetCode(); // 6 haneli bir kod oluşturun
                    var message = $"yenileme kodu:{resetCode}........";
                    await _smsService.SendSms(user.phone, message);
                    _memoryCache.Set("userPhone", user.phone);

                    TempData["code"] = resetCode;
                    return RedirectToAction("SmsConfirmation");
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult SmsConfirmation()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult SmsConfirmation(SmsViewModel model)
        {
            if (model != null)
            {

                if(model.Code != TempData["code"]?.ToString())
                {
                    ModelState.AddModelError("code", "6 haneli olan kodu yanlış girdiniz");
                }

                return RedirectToAction("ResetPassword");
            }
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation(string? email, string inputEmail)
        {
            var model = new ForgotPasswordViewModel();
            if (email != null)
            {
                model.Email = email;
                model.isUserValide = true;
                return View(model);
            }
            

            model.Email = inputEmail;
            model.isUserValide = false;
            return View(model);



        }

        [HttpGet]
        public IActionResult ResetPasswordConfirm()
        {

            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if(email !=null )
            {
                var model = new ResetPasswordViewModel
                {
                    Token = token,
                    Email = email
                };
                return View(model);
            }

            return View();
            
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var hashedPassword = "";
            var user = new Data.Entity.User();
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            if (model.Token != null) 
            {
                var claimsPrincipal = _jwtTokenService.ValidateToken(model.Token);
                if (claimsPrincipal == null)
                {
                    ModelState.AddModelError(string.Empty, "Geçersiz veya süresi dolmuş token.");
                    return View(model);

                }
                var email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
                user = await _userService.GetAllUser().FirstOrDefaultAsync(a => a.email == model.Email);
                if (user == null)
                {
                    return RedirectToAction("ResetPasswordConfirm");
                }
                
            }
            
            if (model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "şifreler uyuşmuyor");
                return View(model);
            }
            var userPhone = _memoryCache.Get("userPhone").ToString();
            user = await _userService.GetAllUser().FirstOrDefaultAsync(a => a.phone == userPhone);
            hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            user.password = hashedPassword;
            _userService.UpdateUser(user);
            TempData["Success"] = "true";
            return RedirectToAction("Index");
        }
    }
}
