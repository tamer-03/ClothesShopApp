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
    public class VariantService
    {
        private readonly IRepository<Variant> _variantRepository;
        public VariantService(IRepository<Variant> repository) 
        {
            _variantRepository = repository;
        }
        public Variant ConvertToVariant(VariantViewModel model) 
        {
            var variant = new Variant();
            variant.Name = model.Name;
            return variant;
        }
        public List<VariantViewModel> ConvertToModel(List<Variant> variant)
        {
            var model = variant.Select(v => new  VariantViewModel
            {
                Id = v.VariantId,
                Name = v.Name,
            }).ToList();
            return model ;
        }
        public IQueryable<Variant> GetAllVariants()
        {
            return _variantRepository.GetAll();
        }
        public Variant GeVariantById(int id)
        {
            return _variantRepository.GetById(id);
        }
        public void DeleteVariant(int id)
        {
            _variantRepository.Delete(id);
        }
        public void UpdateVariant(Variant variant)
        {

            _variantRepository.Update(variant);
        }
        public void CreateVariant(Variant variant)
        {
            _variantRepository.Create(variant);
        }
    }
}
