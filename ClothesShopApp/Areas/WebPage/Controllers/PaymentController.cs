using ClothesShopApp.Business;
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.Security.Claims;
using System.Web.WebPages.Html;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClothesShopApp.Areas.WebPage.Controllers
{
	[Area("WebPage")]
	public class PaymentController : Controller
	{
		private readonly IMemoryCache _memoryCache;
		private readonly UserService _userService;
		private readonly OrderService _orderService;
		private readonly ProductService _productService;
		private readonly CategoryService _categoryService;
		private readonly AddressService _addressService;
		private readonly LocationService _locationService;
		public PaymentController(
			UserService userService,
			OrderService orderService,
			ProductService productService,
			CategoryService categoryService,
			AddressService addressService,
			LocationService locationService,
			IMemoryCache memoryCache)
		{
			_userService = userService;
			_orderService = orderService;
			_productService = productService;
			_categoryService = categoryService;
			_addressService = addressService;
			_locationService = locationService;
			_memoryCache = memoryCache;
		}
		[HttpGet]
		public IActionResult Index()
		{
			var user = GetUser();
			ViewBag.CheckoutFormInitialize = TempData["CheckoutFormInitialize"] as string;
			var jsonAddress = TempData["Address"] as string;
			var address = !string.IsNullOrEmpty(jsonAddress) ? JsonConvert.DeserializeObject<Data.Entity.Address>(jsonAddress) : null;
			

			var paymentItemJson = TempData["PaymentItem"] as string;
			var paymentItems =!string.IsNullOrEmpty(paymentItemJson)?JsonConvert.DeserializeObject<PaymentItemsViewModel>(paymentItemJson):null;

			address.user = user;
			var location = _locationService.GetAllLocation().Where(l => l.locationId == address.LocationId).Include(pl => pl.parentLocation).First();
			address.Location = location;
			paymentItems.Address = address;

			_memoryCache.TryGetValue("CartData", out PaymentRequestViewModel? cart);

			if(cart != null)
			{
				foreach (var item in cart.Cart)
				{
					var paymentItem = new PaymentItemViewModel
					{
						name = item.Name,
						img = item.Img,
					};
					paymentItems.Item.Add(paymentItem);
				}


				return View(paymentItems);
			}
			
			return View();
		}

		[HttpPost]
		public IActionResult Index([FromBody] PaymentRequestViewModel cart)
		{
			_memoryCache.Set("CartData", cart);
			var selectedAdressId = cart.SelectedAddressId;
			var address = _addressService.GetAllAddress().Where(a => a.addressId == Convert.ToInt32(selectedAdressId)).First();
			var location = _locationService.GetAllLocation().Where(l => l.locationId == address.LocationId).Include(pl => pl.parentLocation).First();

			decimal totalPrice = cart.Cart.Sum(item=> item.Price * item.Quantity);
			var paymentItem = new PaymentItemsViewModel();
			
			
			TempData["PaymentItem"] = JsonConvert.SerializeObject(paymentItem);
			address.Location = null;
			TempData["Address"] = JsonConvert.SerializeObject(address);

			var user = GetUser();
			_memoryCache.Set("userId", user.userId);
			

			Options options = new Options();
			options.ApiKey = "sandbox-6Q0dAyqFZXuT2Kd8dXwM4RJ0eX72cPdn";
			options.SecretKey = "sandbox-Dyfab7cgkPWWJTgceJnjQkkp3Zmv86zu";
			options.BaseUrl = "https://sandbox-api.iyzipay.com";

			CreateCheckoutFormInitializeRequest request = new CreateCheckoutFormInitializeRequest();
			request.Locale = Locale.TR.ToString();
			request.ConversationId = "123456789";
			//price sepetteki ürünlerin toplam fiyatı
			request.Price = totalPrice.ToString("F2", CultureInfo.InvariantCulture);
			//paidprice ürün fiyatları dışında ekstra ödeme öcretleri ile toplam fiyat
			request.PaidPrice = (totalPrice).ToString("F2", CultureInfo.InvariantCulture);
			request.Currency = Currency.TRY.ToString();
			request.BasketId = "B67832";
			request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
			request.CallbackUrl = "https://localhost:7255/WebPage/Payment/Callback";

			// Alıcı ve adres bilgileri eklemeye devam edin


			Buyer buyer = new Buyer();
			buyer.Id = user.userId.ToString();
			buyer.Name = user.name;
			buyer.Surname = user.lastName;
			buyer.GsmNumber = address.phone;
			buyer.Email = user.email;
			buyer.IdentityNumber = user.IdentityNumber;
			buyer.LastLoginDate = user.LastLoginDate.ToString("yyyy-mm-dd HH:mm:ss");
			buyer.RegistrationDate = user.RegistrationDate.ToString("yyyy-mm-dd HH:mm:ssv");
			buyer.RegistrationAddress = address.addresses;
			buyer.Ip = "85.34.78.112";
			buyer.City = location.parentLocation.name;
			buyer.Country = "Turkey";
			buyer.ZipCode = "34732";
			request.Buyer = buyer;

			Iyzipay.Model.Address shippingAddress = new Iyzipay.Model.Address();
			shippingAddress.ContactName = user.name + user.lastName;
			shippingAddress.City = location.parentLocation.name;
			shippingAddress.Country = "Turkey";
			shippingAddress.Description = address.addresses;
			shippingAddress.ZipCode = "34742";
			request.ShippingAddress = shippingAddress;

			Iyzipay.Model.Address billingAddress = new Iyzipay.Model.Address();
			billingAddress.ContactName = user.name+user.lastName;
			billingAddress.City = location.parentLocation.name;
			billingAddress.Country = "Turkey";
			billingAddress.Description = address.addresses;
			billingAddress.ZipCode = "34742";
			request.BillingAddress = billingAddress;

			
			List<BasketItem> basketItems = cart.Cart.Select(item=> new BasketItem
			{
				Id = item.ProductId.ToString(),
				Name = item.Name,
				Price = (item.Price * item.Quantity).ToString("F2", CultureInfo.InvariantCulture),
				Category1 =_categoryService.GetCategoryById(_productService.GetProductById(item.ProductId).categoryId).name,
				ItemType = BasketItemType.PHYSICAL.ToString()
				 

			}).ToList();


			request.BasketItems = basketItems;
		
			
			
		


			CheckoutFormInitialize checkoutFormInitialize = CheckoutFormInitialize.Create(request, options);
			TempData["CheckoutFormInitialize"] = checkoutFormInitialize.CheckoutFormContent;
			ViewBag.CheckoutFormInitialize = checkoutFormInitialize.CheckoutFormContent;
			return Json(new { redirectUrl = Url.Action("Index") });
		}

		[HttpPost]
		public IActionResult CallBack(RetrieveCheckoutFormRequest model)
		{
			
			Options options = new Options();
			options.ApiKey = "sandbox-6Q0dAyqFZXuT2Kd8dXwM4RJ0eX72cPdn";
			options.SecretKey = "sandbox-Dyfab7cgkPWWJTgceJnjQkkp3Zmv86zu";
			options.BaseUrl = "https://sandbox-api.iyzipay.com";

			string data = "";
			data = model.Token;
			RetrieveCheckoutFormRequest request = new RetrieveCheckoutFormRequest();
			request.Token = data;
			CheckoutForm checkoutForm = CheckoutForm.Retrieve(request, options);
			
			if (checkoutForm.PaymentStatus == "SUCCESS")
			{
                //başarılı olduğunda order tablosu dolduralacak
                if (_memoryCache.TryGetValue("CartData", out PaymentRequestViewModel? cart))
                {
                    if (cart != null && cart.Cart != null)
                    {
                        var selectedAdressId = cart.SelectedAddressId;
                        var address = _addressService.GetAllAddress().Where(a => a.addressId == Convert.ToInt32(selectedAdressId)).First();

						var order = new Order
						{
							userId = Convert.ToInt32(_memoryCache.Get("userId")),
							totalPrice = cart.Cart.Sum(item => item.Price * item.Quantity),
							OrderDateTime = DateTime.Now,
							OrderStatus = "sipariş alındı",
							
							address = address,

							orderItems = cart.Cart.Select(item => new Data.Entity.OrderItem
							{
								productId = item.ProductId,
								quantity = item.Quantity,
								price = item.Price,
								ImageUrl = item.Img,

							}).ToList()
						};
                        _orderService.CreateOrder(order);

						_memoryCache.Set("OrderCompleted", true);
						_memoryCache.Remove("CartData");
						_memoryCache.Remove("userId");
						return RedirectToAction("Index", "Cart", new { area = "WebPage" });
					}
                }

				
				//localstorageda tutulan sepet silinecek
			}

			return View();
		}

		[HttpGet]
		public JsonResult CheckCache ()
		{
			bool isOrderCompleted = _memoryCache.TryGetValue("OrderCompleted", out bool result) && result;

			
			if (isOrderCompleted)
			{
				_memoryCache.Remove("OrderCompleted");
			}

			return Json(new { isOrderCompleted });
		}

		public User GetUser()
		{
			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = _userService.GetUserById(Convert.ToInt32(currentUserId));
			return user;
		}

		public IActionResult Confirm()
		{
			return View();
		}
	}
}
