using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using ClothesShopApp.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;



namespace ClothesShopApp.Business
{

    public class ProductService
    {
        private readonly IRepository<Product> _repository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<ProductImage> _productImage;
        private readonly ReviewService _reviewService;
       
        public ProductService(IRepository<Product> repository, IRepository<Category> repository1, IRepository<ProductImage> productImage, ReviewService reviewService1)
        {
            _repository = repository;
            _categoryRepository = repository1;
            _productImage = productImage;
            _reviewService = reviewService1;
        }


        public ProductViewModel GetCategories()
        {
            var nemodel = new ProductViewModel();
            nemodel.CategoryLevels.Add(GetCategorySelectList(null));
            nemodel.SelectedCategoryIds.Add(null);
            return nemodel;
        }

        public List<SelectListItem> GetCategorySelectList(int? parentId)
        {
            var categories = GetCategoriesByParentId(parentId);
            return categories.Select(c => new SelectListItem
            {
                Value = c.categoryId.ToString(),
                Text = c.name
            }).ToList();
        }
        public List<Category> GetCategoriesByParentId(int? parentId)
        {
            return _categoryRepository.GetAll()
                .Where(c => c.parentCategoryId == parentId)
                .OrderBy(c => c.name)
                .ToList();
        }

        public int? GetSelectedParentId(ProductViewModel model)
        {

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


            return parentId;
        }

        public Product GetNewProduct(ProductViewModel model, int? id)
        {
            var newModel = new Product();
            newModel.name = model.Name;

            newModel.description = model.Description;
            newModel.price = model.price;
            newModel.stock = model.stock;

            newModel.categoryId = id;
            return newModel;
        }

        public List<ProductViewModel> ProductMapToViewModel(List<Product>? products)
        {
            if (products == null || products.Count == 0)
            {
                products = GetAllProducts().ToList();


                var productImage = _productImage.GetAll().ToDictionary(p => p.productImageId, p => p.url);

                return products.Select(p => new ProductViewModel
                {
                    ProductId = p.productId,
                    Name = p.name,
                    Description = p.description,
                    price = p.price,
                    stock = p.stock,
                    AverageRating = GetAverageRating(p.productId),
                    TotalReview = _reviewService.GetReviewCount(p.productId),
                    CategoryId = p.categoryId,
                    categoryPath = GetCategoryPath(p.categoryId),
                    PictureUrl = p.productImages != null
                    ? p.productImages.Select(pi => pi.url).ToList()
                    : new List<string>()
                }).ToList();
            }
            else
            {
                var productImage = _productImage.GetAll().ToDictionary(p => p.productImageId, p => p.url);

                return products.Select(p => new ProductViewModel
                {
                    ProductId = p.productId,
                    Name = p.name,
                    Description = p.description,
                    price = p.price,
                    stock = p.stock,
                    CategoryId = p.categoryId,
                    AverageRating = GetAverageRating(p.productId),
                    TotalReview = _reviewService.GetReviewCount(p.productId),
                    categoryPath = GetCategoryPath(p.categoryId),
                    PictureUrl = p.productImages != null
                    ? p.productImages.Select(pi => pi.url).ToList()
                    : new List<string>()
                }).ToList();
            }
            

        }
        public ProductDetailViewModel? GetSelectedProductDetails(int productId)
        {
            var productEntity = _repository.GetAll()
                .Include(p => p.ProductVariants)  // ProductSize ilişkisini dahil ediyoruz
                .ThenInclude(pv => pv.VariantValue)
                .ThenInclude(vv => vv.Variant)// Size tablosunu dahil ediyoruz
                .FirstOrDefault(p => p.productId == productId);

            var productImages = _productImage.GetAll()
                .Where(p => p.ProductId == productId)
                .Select(url => url.url)
                .ToList();

            if (productEntity == null)
            {
                return null;
            }
            var variantGroup = productEntity.ProductVariants
                .GroupBy(pv => pv.VariantValue.Variant.Name)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(pv => new SelectListItem
                    {
                        Value = pv.VariantValueId.ToString(),
                        Text = pv.VariantValue.Value
                    }).ToList()
                );
            // Size seçeneklerini alıyoruz
            var reviews = _reviewService.GetReviewsByProductId(productId);

            var product = new ProductDetailViewModel
            {
                ProductId = productEntity.productId,
                Name = productEntity.name,
                Description = productEntity.description,
                price = productEntity.price,
                PictureUrl = productImages,
                Variants = variantGroup,  // Size dropdown listesi
                Reviews = reviews,
                categoryPath = GetCategoryPath(productEntity.categoryId)  // Kategori yolunu alıyoruz
            };

            return product;
        }


        public ProductViewModel? GetSelectedProduct(int productId)
        {



            var productEntity = _repository.GetAll().FirstOrDefault(p => p.productId == productId);
            var productImages = _productImage.GetAll().Where(p => p.ProductId == productId).Select(url => url.url).ToList();
            var ımages = _productImage.GetAll().Where(p => p.ProductId == productId).Select(id => id.productImageId).ToList();
            if (productEntity == null)
            {
                return null;
            }

            var product = new ProductViewModel
            {
                ProductId = productEntity.productId,
                Name = productEntity.name,
                Description = productEntity.description,
                stock = productEntity.stock,
                price = productEntity.price,
                CategoryId = productEntity.categoryId,
                Categories = GetSelectListCategory(),
                PictureUrl = productImages,
                PictureIds = ımages,
                CategoryLevels = new List<List<SelectListItem>>
                {
                    GetCategorySelectList(null) // Burada bir liste içinde tek bir kategori seviyesi ekliyoruz
                },
                SelectedCategoryIds = new List<int?> { productEntity.categoryId }
            };

            product.categoryPath = GetCategoryPath(product.CategoryId);

            return product;
        }

        public string GetCategoryPath(int? categoryId)
        {
            if (!categoryId.HasValue)
                return "Unknown";

            var category = _categoryRepository.GetById(categoryId.Value);
            if (category == null)
                return "Unknown";

            string path = category.name;
            while (category.parentCategoryId.HasValue)
            {
                category = _categoryRepository.GetById(category.parentCategoryId.Value);
                path = category.name + " > " + path;
            }

            return path;
        }

        public List<SelectListItem> GetSelectListCategory()
        {
            return _categoryRepository.GetAll().Select(c => new SelectListItem
            {
                Text = c.name,
                Value = c.categoryId.ToString()
            }).ToList();
        }
        public Product GetUpdatedProduct(ProductViewModel model)
        {
            var newModel = new Product();
            newModel.productId = model.ProductId;
            newModel.name = model.Name;

            newModel.description = model.Description;
            newModel.price = model.price;
            newModel.stock = model.stock;
            if (model.SelectedCategoryIds.Count != 1)
            {
                newModel.categoryId = GetSelectedParentId(model);
            }
            else
            {
                newModel.categoryId = model.CategoryId;
            }

            return newModel;
        }

        public List<ProductViewModel> GetProductsByCategory(int categoryId)
        {
            var allCategoryIds = GetAllSubCategoryIds(categoryId);
            var productImage = _productImage.GetAll().ToDictionary(p => p.productImageId, p => p.url);

            // Ürünleri filtrele
            var products = _repository.GetAll()
                .Where(p => p.categoryId.HasValue && allCategoryIds.Contains(p.categoryId.Value))
                .Select(p => new ProductViewModel
                {
                    ProductId = p.productId,
                    Name = p.name,
                    Description = p.description,
                    price = p.price,

                    CategoryId = p.categoryId.Value,

                    PictureUrl = p.productImages != null
                    ? p.productImages.Select(pi => pi.url).ToList()
                    : new List<string>()
                    // Diğer gerekli alanları eşle
                })
                .ToList();

            return products;
        }

        public List<int> GetAllSubCategoryIds(int parentId)
        {
            var categoryIds = new List<int> { parentId };

            var subCategories = _categoryRepository.GetAll()
                .Where(c => c.parentCategoryId == parentId)
                .ToList(); ;

            foreach (var subCategory in subCategories)
            {
                categoryIds.AddRange(GetAllSubCategoryIds(subCategory.categoryId));
            }

            return categoryIds;
        }

        public double GetAverageRating(int productId)
        {
            var reviews = _reviewService.GetAllReview().Where(p => p.productId == productId).ToList();
            if (reviews == null || !reviews.Any())
            {
                return 0;
            }

            return reviews.Average(r => r.rating); // Yorumların ortalama puanını hesaplar
        }


        public IQueryable<Product> GetAllProducts()
        {
            return _repository.GetAll();
        }
        public Product GetProductById(int id)
        {
            return _repository.GetById(id);
        }
        public void DeleteProduct(int id)
        {
            _repository.Delete(id);
        }
        public void UpdateProduct(Product product)
        {

            _repository.Update(product);
        }
        public void CreateProduct(Product product)
        {
            _repository.Create(product);
        }

    }
}
