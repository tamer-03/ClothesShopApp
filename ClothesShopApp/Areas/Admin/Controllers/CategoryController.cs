using ClothesShopApp.Business;
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web.WebPages.Html;

namespace ClothesShopApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CategoryController(CategoryService categoryService, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _categoryService = categoryService;
        }
        

        [HttpGet]
        public IActionResult Index()
        {



            var categories = _categoryService.GetCategoryHierarchy();
            var categoryViewModels = _categoryService.MapToViewModel(categories, 0);

            return View(categoryViewModels);


        }




        [HttpPost]
        public IActionResult DeleteCategory(int CategoryId)
        {
            _categoryService.DeleteCategory(CategoryId);
            return RedirectToAction("Index");
        }

        //private List<CategoryViewModel> MapToViewModel(List<Category> categories, int level)
        //{


        //    return categories
        //        .Select(c => new CategoryViewModel
        //        {
        //            CategoryId = c.categoryId,
        //            Name = c.name,
        //            ParentId = c.parentCategoryId,
        //            Level = level,
        //            SubCategories = MapToViewModel(c.parentIds.ToList(), level + 1),
        //            Picture = c.picture

        //        })
        //        .ToList();

        //}




        [HttpGet]
        public IActionResult CreateCategory()
        {




            var viewModel = new CreateCategoryViewModel();
            viewModel.CategoryLevels.Add(_categoryService.GetCategorySelectList(null));
            viewModel.SelectedCategoryIds.Add(null);

            return View(viewModel);

        }

        //public List<SelectListItem> GetCategorySelectList(int? parentId)
        //{
        //    var categories = _categoryService.GetCategoriesByParentId(parentId);
        //    return categories.Select(c => new SelectListItem
        //    {
        //        Value = c.categoryId.ToString(),
        //        Text = c.name
        //    }).ToList();
        //}

        [HttpPost]
        public IActionResult CreateCategory(CreateCategoryViewModel model, IFormFile pictureFile)
        {



            if (ModelState != null)
            {
                string uniqueFileName = null;
                if (pictureFile != null)
                {
                    // Resmi kaydetmek için dosya adını oluştur
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + pictureFile.FileName;
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        pictureFile.CopyTo(fileStream);
                    }
                }
                int? parentId = null;

                // En son seçilen kategoriyi parent olarak al
                for (int i = model.SelectedCategoryIds.Count - 1; i >= 0; i--)
                {
                    if (model.SelectedCategoryIds[i].HasValue)
                    {
                        parentId = model.SelectedCategoryIds[i];
                        break;
                    }
                }

                var newCategory = new Category
                {
                    name = model.NewCategoryName,
                    parentCategoryId = parentId,
                    picture = uniqueFileName,
                };
                _categoryService.CreateCategory(newCategory);
                TempData["SuccessMessage"] = "Kategori başarıyla eklendi.";
                return RedirectToAction("CreateCategory");
            }

            return View(model);
        }




        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            var category = _categoryService.GetAllCategory()
                                           .Where(c => c.categoryId == id)
                                           .Select(c => new CategoryViewModel
                                           {
                                               CategoryId = c.categoryId,
                                               Name = c.name,
                                               ParentId = c.parentCategoryId,
                                               Picture = c.picture

                                           })
                                           .FirstOrDefault();

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult EditCategory(EditCategoryViewModel model, IFormFile UpdatedImages)
        {
            if (model.Picture != null)
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", model.Picture);

                // Eski görseli sil
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                // Yeni görseli yükle
                var newImageName = $"{Guid.NewGuid()}_{UpdatedImages.FileName}";
                var newImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", newImageName);

                using (var stream = new FileStream(newImagePath, FileMode.Create))
                {
                    UpdatedImages.CopyTo(stream);
                }
                model.Picture = newImageName;


            }


            if (ModelState.IsValid)
            {
                var editedCategory = new Category
                {
                    categoryId = model.CategoryId,
                    name = model.Name,
                    parentCategoryId = model.ParentId,
                    picture = model.Picture,

                };
                TempData["SuccessMessage"] = "Kategori başarıyla güncellendi.";
                _categoryService.UpdateCategory(editedCategory);
                return RedirectToAction("Index");
            }
            return View(model);


        }





        public IActionResult DeleteCategory(CategoryViewModel model)
        {
            _categoryService.DeleteCategory(model.CategoryId);
            return RedirectToAction("Index");

        }


        [HttpGet]
        public IActionResult LoadSubCategories(int parentId)
        {
            var subCategories =_categoryService.GetCategorySelectList(parentId);


            return Json(subCategories);
        }
    }
}
