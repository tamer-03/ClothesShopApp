using ClothesShopApp.Business;
using Microsoft.AspNetCore.Mvc;

namespace ClothesShopApp.Areas.WebPage.Controllers
{
	[Area("WebPage")]
	public class HomePageController : Controller
	{
		private readonly CategoryService _categoryService;
		public HomePageController(CategoryService categoryService)
		{
			_categoryService = categoryService;
		}
		public IActionResult Index()
		{
			var viewModel = _categoryService.GetParentandSubCategoryViewModel();
			return View(viewModel);
		}
	}
}