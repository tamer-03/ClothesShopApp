using ClothesShopApp.Data.Entity;
using ClothesShopApp.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Business
{
    public class ReviewService
    {
        private readonly IRepository<Review> _reviewRepository;
        public ReviewService(IRepository<Review> repository) 
        {
            _reviewRepository = repository;
        }
        public IQueryable<Review> GetAllReview()
        {
            return _reviewRepository.GetAll();
        }


        public void UpdateReview(Review review)
        {
            _reviewRepository.Update(review);
        }
        public void DeleteReview(int id)
        {
            _reviewRepository.Delete(id);
        }
        public void CreateReview(Review review)
        {
            _reviewRepository.Create(review);
        }
        public IEnumerable<Review> GetReviewsByProductId(int productId)
        {
            return _reviewRepository.GetAll()
                .Where(r => r.productId == productId)
                .Include(r => r.user) // Yorumu yazan kullanıcının bilgilerini de getir
                .ToList();
        }
        public int GetReviewCount(int productId)
        {
            return _reviewRepository.GetAll().Where(p=>p.productId == productId).Count();
        }

    }
}
