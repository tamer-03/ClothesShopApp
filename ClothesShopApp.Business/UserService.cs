using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using ClothesShopApp.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ClothesShopApp.Business
{
    public class UserService
    {
        private readonly IRepository<User> _repository; 

        public UserService(IRepository<User> repository)
        {
            _repository = repository;
        }

        public User ValidateUser(string email)
        {
            var user = _repository.GetAll().Where(u => u.email == email);

            return user.FirstOrDefault();
        }
        
        public IQueryable<User> GetAllUser() 
        {
            return _repository.GetAll();
        }
        public User GetUserById(int id) 
        {
            return _repository.GetById(id);
        }
        public void CreateUser(User user) 
        {
            _repository.Create(user);
        }
        public void UpdateUser(User user) 
        {
            _repository.Update(user);
        }
        public void DeleteUser(int id) 
        {
            _repository.Delete(id);
        }

        public User UserInformationModelToUser(CustomerInformationViewModel model,User user)
        {
            user.name = model.name;
            user.email = model.email;
            user.lastName = model.lastName;
            user.phone = model.phone;
            user.IdentityNumber = model.identityNumber;
            return user;
        }

        public CustomerInformationViewModel UserInformationUserToModel(User user)
        {
            var model = new CustomerInformationViewModel
            {
                name = user.name,
                lastName = user.lastName,
                email = user.email,
                phone = user.phone,
                identityNumber = user.IdentityNumber
            };

            return model;
        }

        public User ConvertModelToUser(RegisterViewModel model) 
        {
            var user = new User
            {
                name = model.name,
                lastName = model.lastName,
                email = model.email,
                password = model.password,
                IdentityNumber = model.intentityNumber ?? "11111111111",
                phone = model.phone,
                RegistrationDate = DateTime.Now,

            };

            return user;
        }

        
    }
}
