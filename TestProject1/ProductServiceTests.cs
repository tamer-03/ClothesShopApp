using ClothesShopApp.Business;
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using ClothesShopApp.Repository;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShop.Business.Test
{
	public class ProductServiceTests
	{
		private readonly ProductService _productService;
		private readonly Mock<IRepository<Product>> _repositoryProductMock;
		private readonly Mock<IRepository<Category>> _repositoryCategoryMock;
		private readonly Mock<IRepository<ProductImage>> _repositoryProductImageMock;

		public ProductServiceTests()
		{

			_repositoryProductMock = new Mock<IRepository<Product>>();
			_repositoryCategoryMock = new Mock<IRepository<Category>>();
			_repositoryProductImageMock = new Mock<IRepository<ProductImage>>();
			_productService = new ProductService(_repositoryProductMock.Object, _repositoryCategoryMock.Object, _repositoryProductImageMock.Object);
		}

		[Fact]
		private void GetSelectedParentId_Test()
		{
			var selectedCategoryIds = new List<int?>
			{
				1,2,3,4,5
			};

			var model = new ProductViewModel
			{
				SelectedCategoryIds = selectedCategoryIds
			};

			var result = _productService.GetSelectedParentId(model);

			Assert.NotNull(result);
			Assert.Equal(5, result);

		}

		[Fact]
		public void GetNewProduct_Test()
		{
			var categoryId = 1;
			var model = new ProductViewModel
			{
				Name = "name",
				Description = "description",
				stock = 123,
				price = 10,

			};

			var result = _productService.GetNewProduct(model, categoryId);

			Assert.NotNull(result);
			Assert.Equal("name", result.name);
			Assert.Equal("description", result.description);
			Assert.Equal(categoryId, result.categoryId);
		}

		[Fact]
		private void ProductMapToViewModel_Test()
		{
			var products = GetSampleProducts();
			var productImage = GetSampleProductImages();

			_repositoryProductMock.Setup(repo=> repo.GetAll()).Returns(products.AsQueryable);
			_repositoryProductImageMock.Setup(repo=> repo.GetAll()).Returns(productImage.AsQueryable);

			var result = _productService.ProductMapToViewModel(products);
			Assert.NotNull(result);
			Assert.Equal(2, result.Count);
			Assert.Equal("Product 1", result[0].Name);
			Assert.Equal("Product 2", result[1].Name);
			Assert.Equal(100, result[0].price);
			Assert.Equal(2, result[0].PictureUrl.Count);
			Assert.Equal("image1.jpg", result[0].PictureUrl[0]);


		}


		private List<Product> GetSampleProducts()
		{
			return new List<Product>
		{
			new Product
			{
				productId = 1,
				name = "Product 1",
				description = "Description 1",
				price = 100,
				stock = 10,
				categoryId = 1,
				productImages = new List<ProductImage>
				{
					new ProductImage { productImageId = 1, url = "image1.jpg",ProductId =1 },
					new ProductImage { productImageId = 2, url = "image2.jpg" , ProductId = 1}
				}
			},
			new Product
			{
				productId = 2,
				name = "Product 2",
				description = "Description 2",
				price = 200,
				stock = 20,
				categoryId = 2,
				productImages = new List<ProductImage>
				{
					new ProductImage { productImageId = 3, url = "image3.jpg", ProductId =2 }
				}
			},

            new Product
            {
                productId = 3,
                name = "Product 3",
                description = "Description 2",
                price = 200,
                stock = 20,
                categoryId = 2,
                productImages = new List<ProductImage>
                {
                    new ProductImage { productImageId = 3, url = "image3.jpg", ProductId =2 }
                }
            }
        };
		}

		// Sahte ürün resimleri
		private List<ProductImage> GetSampleProductImages()
		{
			return new List<ProductImage>
			{
				new ProductImage { productImageId = 1, url = "image1.jpg", ProductId =1},
				new ProductImage { productImageId = 2, url = "image2.jpg", ProductId = 1},
				new ProductImage { productImageId = 3, url = "image3.jpg" , ProductId = 2}
			};
		}

		[Fact]
		private void GetSelectedProduct_Test()
		{
			
			var productImages = GetSampleProductImages();
			var products = GetSampleProducts();

			_repositoryProductImageMock.Setup(a=> a.GetAll()).Returns(productImages.AsQueryable);
			_repositoryProductMock.Setup(s=>s.GetAll()).Returns(products.AsQueryable);

			var result = _productService.GetSelectedProduct(1);
			Assert.NotNull(result);
			Assert.Equal("Product 1", result.Name);
			Assert.Equal(2,result.PictureUrl.Count);
			Assert.Equal("image1.jpg",result.PictureUrl[0]);
			Assert.Equal(2, result.PictureIds.Count);
			Assert.Equal(1, result.PictureIds[0]);
		}

		[Fact]
		private void GetCategoryPath_Test()
		{
			var categories = Categories();

			_repositoryCategoryMock.Setup(s => s.GetById(It.IsAny<int>()))
		.Returns<int>(id => categories.FirstOrDefault(c => c.categoryId == id)); ;

			var result =  _productService.GetCategoryPath(4);

			Assert.NotNull(result);
			Assert.Equal("Giyim > Erkek > Pantolon",result);


		}


		[Fact]
		private void GetProductsByCategory_Test() 
		{
			var productImages = GetSampleProductImages();
			var products = GetSampleProducts();
			var categories = Categories();

			_repositoryProductMock.Setup(s=>s.GetAll()).Returns(products.AsQueryable());
			_repositoryProductImageMock.Setup(s=>s.GetAll()).Returns(productImages.AsQueryable());
			_repositoryCategoryMock.Setup(s => s.GetAll()).Returns(categories.AsQueryable);

			var result = _productService.GetProductsByCategory(1);

			Assert.NotNull(result);
			Assert.Equal(3, result.Count);
			Assert.Equal(1, result[0].ProductId);
			Assert.Equal(1, result[0].CategoryId);
            Assert.Equal(100, result[0].price);
            Assert.Equal(2, result[1].ProductId);
            Assert.Equal(200, result[1].price);
			Assert.Equal(3,result[2].ProductId);
            Assert.Equal(2, result[2].CategoryId);
            Assert.Equal(2, result[1].CategoryId);

        }
		public List<Category> Categories()
		{
			return new List<Category>
			{
				new Category
				{
					categoryId = 1,
					name = "Giyim",
					parentCategoryId = null,
					parentIds = new List<Category>{new Category {categoryId =2,name="Erkek"}, new Category { categoryId=3,name="Kadın" } },

					picture="asdasd"
				},
				 new Category
				{
					categoryId = 2,
					name = "Erkek",
					parentCategoryId = 1,
					parentIds = new List<Category>{new Category { categoryId = 4, name = "Pantolon" }},
					picture="asdasd"
				},
				  new Category
				{
					categoryId = 3,
					name = "Kadın",
					parentCategoryId = 1,
					parentIds = new List<Category>(),
					picture="asdasd"
				},
					new Category
				{
					categoryId = 4,
					name = "Pantolon",
					parentCategoryId = 2,
					parentIds = new List<Category>(),
					picture="asdasd"
				},
			};

		}


	}
}

