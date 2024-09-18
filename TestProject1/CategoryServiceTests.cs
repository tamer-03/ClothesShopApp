using ClothesShopApp.Business;
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using ClothesShopApp.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShop.Business.Test
{
    public class CategoryServiceTests
    {
        private readonly Mock<IRepository<Category>> _repositoryMock;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _repositoryMock = new Mock<IRepository<Category>>();
            _categoryService = new CategoryService(_repositoryMock.Object);
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

        [Fact]
        public void BuildHierarchy_Test()
        {
            var categories = Categories();
            var parentId = 1;

           


            _repositoryMock.Setup(repo => repo.GetAll()).Returns(categories.AsQueryable());

            var result = _categoryService.BuildHierarchy(categories, parentId);
            var result2 = _categoryService.BuildHierarchy(categories, null);

            Assert.NotNull(result);
            

            Assert.Single(result2); //giyim kategorisinin altında erkek kategorisi var
            Assert.Equal("Giyim", result2[0].name);
			Assert.Equal("Erkek", result2[0].parentIds.First().name);

		}

        [Fact]
        public void GetCategoriesByParentId_Test()
        {
            var testParentId = 1;
            var categories = Categories();
            _repositoryMock.Setup(repo => repo.GetAll()).Returns(categories.AsQueryable());

            var resut = _categoryService.GetCategoriesByParentId(testParentId);

            Assert.NotNull(resut);
            Assert.Equal(2,resut.Count);

            Assert.Equal("Erkek", resut[0].name); 
            Assert.Equal("Kadın", resut[1].name);




        }

        [Fact]
        public void GetParentandSubCategoryViewModel_Test()
        {

            var categories = Categories();

            _repositoryMock.Setup(repo => repo.GetAll()).Returns(categories.AsQueryable());
            
            var result = _categoryService.GetParentandSubCategoryViewModel(1);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Erkek", result[0].Name);
            Assert.Equal("Kadın", result[1].Name);
			Assert.Equal("Pantolon", result[0].SubCategories[0].Name);

		}

        [Fact]
        public void GetParentCategory_Test()
        {
			var parentCategories = Categories();


            _repositoryMock.Setup(repo => repo.GetAll()).Returns(parentCategories.AsQueryable);


            var result = _categoryService.GetParentCategory(1);
            var result2 = _categoryService.GetParentCategory(2);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Erkek", result[0].name);
            Assert.Equal(4, result2[0].categoryId);
			Assert.Equal("Pantolon", result2[0].name);
		}

        [Fact]
        public void MapToViewModel_Test()
        {
            var categories = Categories();


            _repositoryMock.Setup(repo => repo.GetAll()).Returns(categories.AsQueryable());

            var result = _categoryService.MapToViewModel(categories, 1);

            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.Equal("Giyim", result[0].Name);
			Assert.Equal("Erkek", result[1].Name);
			Assert.Equal("Kadın", result[2].Name);
            Assert.Equal("Pantolon", result[1].SubCategories[0].Name);
			Assert.Equal("Erkek", result[0].SubCategories[0].Name);
			Assert.Equal("Kadın", result[0].SubCategories[1].Name);
			Assert.Equal("Pantolon", result[3].Name);
		}

        [Fact]
        public void GetCategorySelectList_Test()
        {
            var categories = Categories();

			_repositoryMock.Setup(repo => repo.GetAll()).Returns(categories.AsQueryable());

            var result = _categoryService.GetCategorySelectList(1);
            
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Erkek", result[0].Text);
			Assert.Equal("Kadın", result[1].Text);
		}
	} 
}
