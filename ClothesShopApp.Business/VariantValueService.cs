using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using ClothesShopApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Business
{
    public class VariantValueService
    {
        private readonly IRepository<VariantValue> _repository;
        public VariantValueService(IRepository<VariantValue> repository)
        {
            _repository = repository;
        }
        public VariantValue ConvertToVariantValue(VariantViewModel model)
        {
            var variantValue = new VariantValue();
            variantValue.Value = model.Name;
            variantValue.VariantId = model.Id;
            return variantValue;
        }
        public List<VariantViewModel> ConvertToModel(List<Variant> variant)
        {
            var model = variant.Select(v => new VariantViewModel
            {
                Id = v.VariantId,
                Name = v.Name,
            }).ToList();
            return model;
        }
        public IQueryable<VariantValue> GetAllVariantValues()
        {
            return _repository.GetAll();
        }
        public VariantValue GeVariantValueById(int id)
        {
            return _repository.GetById(id);
        }
        public void DeleteVariantValue(int id)
        {
            _repository.Delete(id);
        }
        public void UpdateVariantValue(VariantValue variantValue)
        {

            _repository.Update(variantValue);
        }
        public void CreateVariantValue(VariantValue variantValue)
        {
            _repository.Create(variantValue);
        }
    }
}
