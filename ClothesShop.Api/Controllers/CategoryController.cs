using ClothesShop.Api.Response;
using ClothesShopApp.Business;
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web.WebPages.Html;



namespace ClothesShop.Api.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly CategoryService _categoryService;
		
		public CategoryController(CategoryService categoryService )
		{
			
			_categoryService = categoryService;
		}


		[HttpGet("categories")]
		public IActionResult Index()
		{


			
			var categories = _categoryService.GetCategoryHierarchy();


			var categoryViewModels = _categoryService.MapToViewModel(categories, 0);

			return Ok(categoryViewModels);


		}




		[HttpPost("delete/{categoryId}")]
		public IActionResult DeleteCategory(int categoryId)
		{
			try
			{
				_categoryService.DeleteCategory(categoryId);
				return Ok(new ApiResponse<string>(true, "silme işlemi başarılı", null));
			}
			catch (Exception ex) 
			{
				return StatusCode(500, new ApiResponse<string>(false,ex.Message,null));
			}
			
		}








		[HttpPost("create")]
		public IActionResult CreateCategory([FromForm] CreateCategoryViewModel model)
		{

			try
			{
                if (ModelState != null)
                {
                    string uniqueFileName = null;
                    if (model.pictureFile != null)
                    {
                        // Resmi kaydetmek için dosya adını oluştur
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + model.pictureFile.FileName;
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "images");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            model.pictureFile.CopyTo(fileStream);
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
                    return Ok(new ApiResponse<Category>(true,"başarılı",newCategory));
                }

                return Conflict(new ApiResponse<string>(false, "bir sorun oluştu tekrar deneyiniz",null));
            }
			catch (Exception ex) 
			{
				return StatusCode(500,new ApiResponse<string>(false,ex.Message,null) );
			}

			
		}





		[HttpPost("edit")]
		public IActionResult EditCategory([FromForm] EditCategoryViewModel model)
		{
			try
			{
                if (model.Picture != null)
                {
                    var oldImagePath = Path.Combine("App_Data", "images", model.Picture);

                    // Eski görseli sil
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                    // Yeni görseli yükle
                    var newImageName = $"{Guid.NewGuid()}_{model.UpdatedImages.FileName}";
                    var newImagePath = Path.Combine("App_Data", "images", newImageName);

                    using (var stream = new FileStream(newImagePath, FileMode.Create))
                    {
                        model.UpdatedImages.CopyTo(stream);
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
                    _categoryService.UpdateCategory(editedCategory);
                    return Ok(new ApiResponse<Category>(true, "+", editedCategory));
                }
                return BadRequest(new ApiResponse<string>(false, "eksik veri", null));
            }
			catch (Exception ex) 
			{
                return StatusCode(500, new ApiResponse<string>(false,ex.Message,null));
            }
			
			


		}







		[HttpGet("loadsubcategories/{parentId}")]
		public IActionResult LoadSubCategories(int parentId)
		{
			var subCategories = _categoryService.GetCategorySelectList(parentId);


			return Ok(new ApiResponse<List<SelectListItem>>(true,"+", subCategories));
		}
	}
}
