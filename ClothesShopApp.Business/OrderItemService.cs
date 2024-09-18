using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using ClothesShopApp.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Business
{
    public class OrderItemService
    {
        private readonly IRepository<OrderItem> _repository;
        public OrderItemService(IRepository<OrderItem> repository)
        {
            _repository = repository;
        }

        
      

        public IQueryable<OrderItem> GetAllOrders()
        {
            return _repository.GetAll();
        }
        public OrderItem GetOrderById(int id)
        {
            return _repository.GetById(id);
        }
        public void DeleteOrder(int id)
        {
            _repository.Delete(id);
        }
        public void UpdateOrder(OrderItem orderItem)
        {

            _repository.Update(orderItem);
        }
        public void CreateOrder(OrderItem orderItem)
        {
            _repository.Create(orderItem);
        }
    }
}
