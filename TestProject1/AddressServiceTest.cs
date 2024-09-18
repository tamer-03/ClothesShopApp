
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Data.ViewModels;
using ClothesShopApp.Repository;
using Microsoft.Identity.Client;
using Moq;


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


        [Fact]
        public void GetAddressLocationList_Test() 
        {
            int testUserId = 1;

            var testLocation = new Location { locationId = 1, name = "istanbul", parentLocation = new Location {locationId=2, name = "Üsküdar", parentLocationId = 1 } };

            var addresses = new List<Address>
            {
                new Address {addressId = 1, userId = testUserId, Location = testLocation},
                new Address { addressId =2, userId =testUserId, Location = testLocation }
            }.AsQueryable();

            _mockRepository.Setup(repo=> repo.GetAll()).Returns(addresses);




            var result = _addressService.GetAddressLocationList(testUserId);

            Assert.Equal(2,result.Count);
            Assert.All(result, address => Assert.Equal(address.userId,testUserId));
            Assert.All(result, address => Assert.NotNull(address.Location));
        }

        [Fact]
        public void GetAddressLocationById_Test() 
        {
            int testAddressId = 1;

            var testLocation = new Location { locationId = 1, name = "istanbul", parentLocation = new Location { locationId = 2, name = "Üsküdar", parentLocationId = 1 } };

            var addresses = new List<Address>
            {
                new Address {addressId = 1, userId = 1, Location = testLocation},
                new Address { addressId =2, userId =1, Location = testLocation }
            }.AsQueryable();

            _mockRepository.Setup(repo => repo.GetAll()).Returns(addresses);

            var result = _addressService.GetAddressLocationById(testAddressId);

            Assert.NotNull(result);
            Assert.Equal(testAddressId, result.addressId);
        
        }

    }
}
