
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using ClothesShopApp.Repository;
using Microsoft.Identity.Client;
using Moq;
using Xunit;

namespace ClothesShopApp.Business.Test
{
    public class AddressServiceTest
    {
        private readonly AddressService _addressService;
        private readonly Mock<IRepository<Address>> _mockRepository;

        public AddressServiceTest()
        {
            _mockRepository = new Mock<IRepository<Address>>();
            _addressService = new AddressService(_mockRepository.Object);
        }

        [Fact]
        public void AddAddressModelToAddress_Test()
        {
            var model = new EditAddressViewModel
            {
                Address = new Address
                {
                    addressId = 1,
                    addresses = "asdasdasd",
                    firstName = "tamer",
                    lastName = "kandil",
                    addressHeader = "ev",
                    phone = "5505555555"
                },
                SelectedDistrictId = 1,
            };

            var result = _addressService.AddAddressModelToAddress(model);

            UserAssertion.Equals(1, result.addressId);
            UserAssertion.Equals("tamer", result.firstName);
        }
    }
}
