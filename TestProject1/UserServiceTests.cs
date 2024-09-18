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
	public class UserServiceTests
	{
		private readonly UserService _userService;
		private readonly Mock<IRepository<User>> _mockRepository;

        public UserServiceTests()
        {
            _mockRepository = new Mock<IRepository<User>>();
            _userService = new UserService(_mockRepository.Object);
        }

        public User GetUser()
        {
			return new User
			{
				name = "Test",
				lastName = "Test1",
				email = "test@123",
				phone = "12345",
				IdentityNumber = "1234567890",
			};
		}

        public CustomerInformationViewModel GetCustomerInformationViewModel() 
        {
			return new CustomerInformationViewModel
			{
				name = "tamer",
				lastName = "asdaw",
				email = "test@123.com",
				phone = "1234444333",
				identityNumber = "12345678901",
				
			};
		}

        [Fact]
        public void UserInformationModelToUser_Test()
        {
            var user = GetUser();

            var model = GetCustomerInformationViewModel();

            
            var result = _userService.UserInformationModelToUser(model, user);

            Assert.NotNull(result);
            Assert.Equal("tamer", result.name);
            Assert.Equal("1234444333",result.phone);

        }

        [Fact]
        public void UserInformationUserToModel_Test() 
        {
            var user = GetUser();

			var model = GetCustomerInformationViewModel();


            var result = _userService.UserInformationUserToModel(user);

			Assert.NotNull(result);
			Assert.Equal("Test", result.name);
			Assert.Equal("12345", result.phone);
		}

		[Fact]
		public void ConvertModelToUser_Test()
		{
			var user = GetUser();
			var model = new RegisterViewModel
			{
				name = "tamer",
				lastName = "asdaw",
				email = "test@123.com",
				phone = "1234444333",
				intentityNumber = "12345678901",

			};


			var result = _userService.ConvertModelToUser(model);


			Assert.NotNull(result);
			Assert.Equal("tamer", result.name);

		}
	}
}
