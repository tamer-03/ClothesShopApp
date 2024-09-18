using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using ClothesShopApp.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Business
{
    public class ProductImageSerivce
    {
        private readonly IRepository<ProductImage> _productImageRepository;
        public ProductImageSerivce(IRepository<ProductImage> repository)
        {
            _productImageRepository = repository;
        }

        public ProductImage MapToViewModel(string url)
        {
            var ımage = new ProductImage();
            ımage.url = url;
            return ımage;
        }

        
        public IQueryable<ProductImage> GetAllProductImages()
        {

            return _productImageRepository.GetAll(); 
        }

        public void DeleteImageProduct(int id)
        {
            _productImageRepository.Delete(id);
        }
        public void UpdateImageProduct(ProductImage product)
        {

            _productImageRepository.Update(product);
        }
        public void CreateImageProduct(ProductImage product)
        {
            _productImageRepository.Create(product);
        }
    }
}
