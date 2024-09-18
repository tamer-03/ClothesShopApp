using ClothesShopApp.Business;
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;

namespace ClothesShopApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly ProductImageSerivce _productImageSerivce;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ProductVariantService _productVariantService;
        private readonly VariantService _variantService;
        private readonly VariantValueService _variantValueService;
        public ProductController(
            ProductService productService,
            CategoryService categoryService,
            ProductImageSerivce productImageSerivce,
            IWebHostEnvironment webHostEnvironment,
            ProductVariantService productVariantService,
            VariantService variantService,
            VariantValueService variantValueService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _productImageSerivce = productImageSerivce;
            _webHostEnvironment = webHostEnvironment;
            _productVariantService = productVariantService;
            _variantService = variantService;
            _variantValueService = variantValueService;

        }
        public IActionResult Index()
        {

            var model = _productService.ProductMapToViewModel(null);


            return View(model);
        }



        [HttpGet]
        public IActionResult CreateProduct()
        {
            var viewModel = _productService.GetCategories();


            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductViewModel model, IEnumerable<IFormFile> pictureFiles)
        {



            if (ModelState != null)
            {


                int? parentId = null;


                parentId = _productService.GetSelectedParentId(model);
                var newProduct = _productService.GetNewProduct(model, parentId);
                _productService.CreateProduct(newProduct);

                foreach (var pictureFile in pictureFiles)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + pictureFile.FileName;
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        pictureFile.CopyTo(fileStream);
                    }

                    // Yeni resim nesnesi oluşturma ve ürüne ekleme
                    var productImage = new ProductImage
                    {
                        url = uniqueFileName,
                        ProductId = newProduct.productId
                    };

                    _productImageSerivce.CreateImageProduct(productImage);
                }

                return RedirectToAction("AddVariantsToProduct", new { productId = newProduct.productId });


            }
            return View(model);
        }
        
        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditProduct(int id)
        {

            var model = _productService.GetSelectedProduct(id);


            return View(model);
        }
        [HttpPost]
        public IActionResult EditProduct(ProductViewModel model, List<IFormFile> NewImages, List<int> DeletedImages, List<IFormFile> UpdatedImages)
        {
            if (DeletedImages != null && DeletedImages.Count > 0)
            {
                foreach (var imageId in DeletedImages)
                {
                    var image = _productImageSerivce.GetAllProductImages().Where(c => c.productImageId == imageId).FirstOrDefault();
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", image.url);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                    // Veritabanından da silme işlemi
                    _productImageSerivce.DeleteImageProduct(imageId);
                }
            }
            if (UpdatedImages != null && UpdatedImages.Count > 0)
            {
                for (int i = 0; i < UpdatedImages.Count; i++)
                {
                    if (UpdatedImages[i] != null)
                    {
                        var imageId = model.PictureIds[i]; // ID ile eşleştirme
                        var image = _productImageSerivce.GetAllProductImages()
                            .FirstOrDefault(c => c.productImageId == imageId);

                        if (image != null)
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", image.url);

                            // Eski görseli sil
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }

                            // Yeni görseli yükle
                            var newImageName = $"{Guid.NewGuid()}_{UpdatedImages[i].FileName}";
                            var newImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", newImageName);

                            using (var stream = new FileStream(newImagePath, FileMode.Create))
                            {
                                UpdatedImages[i].CopyTo(stream);
                            }
                            image.url = newImageName;
                            image.ProductId = model.ProductId;

                            // Veritabanında da güncelleme işlemi
                            _productImageSerivce.UpdateImageProduct(image);
                        }
                    }
                }
            }
            if (NewImages != null && NewImages.Count > 0)
            {
                foreach (var newImage in NewImages)
                {
                    var newImageName = $"{Guid.NewGuid()}_{newImage.FileName}";
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", newImageName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        newImage.CopyTo(stream);
                    }
                    var image = new ProductImage
                    {
                        url = newImageName,
                        ProductId = model.ProductId,
                    };
                    // Yeni görseli veritabanına ekle
                    _productImageSerivce.CreateImageProduct(image);
                }
            }
            if (ModelState != null)
            {

                var updatedProduct = _productService.GetUpdatedProduct(model);
                _productService.UpdateProduct(updatedProduct);
                return RedirectToAction("Index");

            }
            model.Categories = _productService.GetSelectListCategory();
            return View(model);
        }

        [HttpGet]
        public IActionResult LoadSubCategories(int parentId)
        {
            var subCategories = _productService.GetCategorySelectList(parentId);


            return Json(subCategories);
        }
        [HttpGet]
        public IActionResult AddVariantsToProduct(int productId)
        {
            var productVariants = _productVariantService.GetAllProductVariants().Where(p => p.ProductId == productId).ToList();
            var variantDetails = (from pv in productVariants
                                  join vv in _variantValueService.GetAllVariantValues() on pv.VariantValueId equals vv.VariantValueId
                                  join vt in _variantService.GetAllVariants() on vv.VariantId equals vt.VariantId
                                  select new
                                  {
                                      VariantTypeName = vt.Name,
                                      VariantValueName = vv.Value,
                                      Stock = pv.Stock,
                                      ProductVariantId = pv.ProductVariantId
                                  }).ToList();

            var variantTypeGroups = variantDetails
             .GroupBy(v => v.VariantTypeName)
             .Select(group => new VariantTypeViewModel
             {
                 VariantTypeName = group.Key,
                 Values = group.Select(v => v.VariantValueName).ToList(),
                 Stock = group.Select(s => s.Stock).ToList(),
                 ProductVariantId = group.Select(p=> p.ProductVariantId).ToList()
             })
             .ToList();

            //var newModel = new ProductVariantsViewModel
            //{
            //    ProductId = productId,
            //    VariantTypes = variantTypeGroups
            //};
            
            var variatns = _variantService.GetAllVariants().ToList();
            var variantValues = _variantValueService.GetAllVariantValues().ToList();
            var model = new VariantViewModel
            {
                ProductId = productId,
                Variants = variatns,
                VariantValues = variantValues,
                VariantTypes = variantTypeGroups
            };
            return View(model);
        }

        [HttpPost]
        public JsonResult AddVariantsToProduct(int productId, int variantValueId, int stock)
        {
            var productVariantExists = _productVariantService.GetAllProductVariants().Any(v => v.VariantValueId == variantValueId && v.ProductId == productId);
            if (productVariantExists) 
            {

                return Json(new {success=false});
            }
            var productVariant = new ProductVariant
            {
                ProductId = productId,
                VariantValueId = variantValueId,
                Stock = stock
            };
            _productVariantService.CreateProductVariant(productVariant);


            // Eklenen varyant değeri
            var productVariants = _productVariantService.GetAllProductVariants().Where(p => p.ProductId == productId).ToList();
            var variantDetails = (from pv in productVariants
                                  join vv in _variantValueService.GetAllVariantValues() on pv.VariantValueId equals vv.VariantValueId
                                  join vt in _variantService.GetAllVariants() on vv.VariantId equals vt.VariantId
                                  select new
                                  {
                                      VariantTypeName = vt.Name,
                                      VariantValueName = vv.Value,
                                      Stock = pv.Stock,
                                      ProductVariantId = pv.ProductVariantId
                                  }).ToList();

            var variantTypeGroups = variantDetails
             .GroupBy(v => v.VariantTypeName)
             .Select(group => new VariantTypeViewModel
             {
                 VariantTypeName = group.Key,
                 Values = group.Select(v => v.VariantValueName).ToList(),
                 Stock = group.Select(s => s.Stock).ToList(),
                 ProductVariantId = group.Select(id=> id.ProductVariantId).ToList(),
             })
             .ToList();

           
            var result = new
            {
                VariantTypes = variantTypeGroups
            };
            

            return Json(result);
        }

        public void DeleteProductVariantValue(int valueId)
        {
            _productVariantService.DeleteProductVariant(valueId);
        }
        //public IActionResult ProductVariants(int productId)
        //{
        //    var productVariants = _productVariantService.GetAllProductVariants().Where(p => p.ProductId == productId).ToList();
        //    var variantDetails = (from pv in productVariants
        //                          join vv in _variantValueService.GetAllVariantValues() on pv.VariantValueId equals vv.VariantValueId
        //                          join vt in _variantService.GetAllVariants() on vv.VariantId equals vt.VariantId
        //                          select new
        //                          {
        //                              VariantTypeName = vt.Name,
        //                              VariantValueName = vv.Value,
        //                              Stock = pv.Stock
        //                          }).ToList();

        //    var variantTypeGroups = variantDetails
        //     .GroupBy(v => v.VariantTypeName)
        //     .Select(group => new VariantTypeViewModel
        //     {
        //         VariantTypeName = group.Key,
        //         Values = group.Select(v => v.VariantValueName).ToList(),
        //         Stock = group.Select(s => s.Stock).ToList(),
        //     })
        //     .ToList();

        //    var model = new ProductVariantsViewModel
        //    {
        //        ProductId = productId,
        //        VariantTypes = variantTypeGroups
        //    };

        //    return View(model);
        //}
        [HttpGet]
        public IActionResult GetVariantValue(int id)
        {
            var variantValue = _variantValueService.GetAllVariantValues().Where(v => v.VariantId == id).Select(v => new SelectListItem
            {
                Value = v.VariantValueId.ToString(),
                Text = v.Value
            });
            return Json(variantValue);
        }
    }
}
