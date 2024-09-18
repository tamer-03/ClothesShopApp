
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using ClothesShopApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages.Html;

namespace ClothesShopApp.Business
{
    public class CategoryService
    {
        private readonly IRepository<Category> _repository;
        public CategoryService(IRepository<Category> repository) 
        {
            _repository = repository;
        }

        public IQueryable<Category> GetAllCategory() 
        {
            return _repository.GetAll();
        }

        public Category GetCategoryById(int? categoryId) 
        {
            return _repository.GetById(categoryId);
        }

        public List<Category> BuildHierarchy(List<Category> categories, int? parentId)
        {
            return categories
                .Where(c => c.parentCategoryId == parentId)
                .Select(c => new Category
                {
                    categoryId = c.categoryId,
                    name = c.name,
                    parentCategoryId = c.parentCategoryId,
                    picture = c.picture,
                    parentIds = BuildHierarchy(categories, c.categoryId)
                })
                .ToList();
        }
        public List<Category> GetCategoryHierarchy()
        {
            var categories = _repository.GetAll().ToList();
            return BuildHierarchy(categories, null);
        }

        public List<Category> GetCategoriesByParentId(int? parentId)
        {
            return _repository.GetAll()
                .Where(c => c.parentCategoryId == parentId)
                .OrderBy(c => c.name)
                .ToList();
        }
        public List<CategoryViewModel>? GetParentandSubCategoryViewModel(int? parentId = null) 
        {
			var parentCateogyr = GetParentCategory(parentId);
			var viewModel = parentCateogyr.Select(c => new CategoryViewModel
			{
				CategoryId = c.categoryId,
				Name = c.name,
				SubCategories = GetParentCategory(c.categoryId).Select(c => new CategoryViewModel
				{
					CategoryId = c.categoryId,
					Name = c.name,
					Picture = c.picture,
				}).ToList()
			}).ToList();
            return viewModel;
		}
		//public List<int> GetAllSubCategoryIds(int parentId)
		//{
		//	var categoryIds = new List<int> { parentId };

		//	var subCategories = GetSubCategories(parentId);

		//	foreach (var subCategory in subCategories)
		//	{
		//		categoryIds.AddRange(GetAllSubCategoryIds(subCategory.categoryId));
		//	}

		//	return categoryIds;
		//}
		public List<CategoryViewModel> MapToViewModel(List<Category> categories, int level)
		{


			return categories
				.Select(c => new CategoryViewModel
				{
					CategoryId = c.categoryId,
					Name = c.name,
					ParentId = c.parentCategoryId,
					Level = level,
					SubCategories = c.parentIds != null ? MapToViewModel(c.parentIds.ToList(), level + 1) : new List<CategoryViewModel>(),
					Picture = c.picture

				})
				.ToList();

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


		public List<Category> GetParentCategory(int? parentId = null)
        {

            return _repository.GetAll().Where(c=> c.parentCategoryId == parentId).ToList(); 
        }
        //public List<Category> GetSubCategories(int parentCategoryId)
        //{
        //    return _repository.GetAll()
        //        .Where(c => c.parentCategoryId == parentCategoryId)
        //        .ToList();
        //}

        public void DeleteCategory(int id) 
        {
            _repository.Delete(id);
        }
        public void UpdateCategory(Category category) 
        {
            
            _repository.Update(category);
        }
        public void CreateCategory(Category category)
        {
            _repository.Create(category);
        }
    }
}
