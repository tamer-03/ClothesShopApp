using ClothesShopApp.Data.Entity;
using ClothesShopApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Business
{
    public class ProductVariantService
    {
        private readonly IRepository<ProductVariant> _repository;
        public ProductVariantService(IRepository<ProductVariant> repository) 
        {
            _repository = repository;
        }
        public IQueryable<ProductVariant> GetAllProductVariants()
        {
            return _repository.GetAll();
        }
        public ProductVariant GeProductVariantById(int id)
        {
            return _repository.GetById(id);
        }
        public void DeleteProductVariant(int id)
        {
            _repository.Delete(id);
        }
        public void UpdateProductVariant(ProductVariant productVariant)
        {

            _repository.Update(productVariant);
        }
        public void CreateProductVariant(ProductVariant productVariant)
        {
            _repository.Create(productVariant);
        }
    }
}
