using ClothesShopApp.Business;
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace ClothesShopApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class VariantController : Controller
    {
        private readonly VariantService _variantService;
        private readonly VariantValueService _variantValueService;
        public VariantController(VariantService variantService, VariantValueService variantValueService)
        {
            _variantService = variantService;
            _variantValueService = variantValueService;
        }
        public IActionResult Index()
        {
            var variants = _variantService.GetAllVariants().ToList();
            var model = _variantService.ConvertToModel(variants);
            return View(model);
        }
        [HttpGet]
        public IActionResult CreateVariant(int? variantId)
        {
            if (variantId == null)
            {
                var model = new VariantViewModel();
                model.VariantValues = null;
                return View(model);
            }
            else
            {
                var model = new VariantViewModel();
                model.Id = variantId.Value;
                var variantValues = _variantValueService.GetAllVariantValues().Where(x => x.VariantId == variantId).ToList();
                model.VariantValues = variantValues;
                return View(model);
            }

        }

        [HttpPost]
        public JsonResult CreateVariant(VariantViewModel model)
        {
            if (model.Id == 0)
            {
                var variant = _variantService.ConvertToVariant(model);
                _variantService.CreateVariant(variant);
               
                model.Id = variant.VariantId;
            }
            else
            {
                var variantValue = _variantValueService.ConvertToVariantValue(model);
                _variantValueService.CreateVariantValue(variantValue);
            }


            var variantValues = _variantValueService.GetAllVariantValues().Where(x => x.VariantId == model.Id).ToList();

            var result = new
            {
                variantId = model.Id,
                variantValues = variantValues.Select(v => new
                {
                    variantValueId = v.VariantValueId,
                    variantId = v.VariantId,
                    value = v.Value
                }).ToList()
            };
            
            return Json(result);
        }

        public IActionResult EditVariant(int id)
        {
			var variant = _variantService.GetAllVariants()
										   .Where(c => c.VariantId == id)
										   .Select(c => new Variant
										   {
											  VariantId = c.VariantId,
                                              
                                              Name = c.Name

										   })
										   .FirstOrDefault();

			if (variant == null)
			{
				return NotFound();
			}
			return View(variant); 
        }
		[HttpPost]
		public IActionResult EditVariant(Variant variant)
		{
			

			if (ModelState.IsValid)
			{
				var editedCategory = new Variant
				{
					VariantId=variant.VariantId,
                    Name = variant.Name

				};
				TempData["SuccessMessage"] = "Varaynt başarıyla güncellendi.";
				_variantService.UpdateVariant(editedCategory);
				return RedirectToAction("Index");
			}
			return View(variant);


		}


		public IActionResult DeleteVariant(int id)
        {
            _variantService.DeleteVariant(id);
            return RedirectToAction("Index");
        }
        public void DeleteVariantValue(int valueId)
        {
            _variantValueService.DeleteVariantValue(valueId);


        }



    }
}
