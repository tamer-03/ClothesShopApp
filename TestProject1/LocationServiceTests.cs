using ClothesShopApp.Business;
using ClothesShopApp.Data.Entity;
using ClothesShopApp.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ClothesShop.Business.Test
{
	public class LocationServiceTests
	{
		private readonly LocationService _locationService;
		private readonly Mock<IRepository<Location>> _locationMock;

        public LocationServiceTests()
        {
            _locationMock = new Mock<IRepository<Location>>();
            _locationService = new LocationService(_locationMock.Object);

        }

		public List<Location> CreateLocation()
		{
			return new List<Location>
			{
				new Location
				{
					name = "istabnul",
					parentLocationId = null,
				},
				new Location
				{
					name = "ankara",
					parentLocationId = null,
				},
				new Location
				{
					name = "üsküdar",
					parentLocationId = 1,
				},new Location
				{
					name = "kadıköy",
					parentLocationId = 1,
				},
			};

		}

		[Fact]
        public void GetAllCities_Test()
        {
			var location = CreateLocation();

			_locationMock.Setup(repo=> repo.GetAll()).Returns(location.AsQueryable);

			var result = _locationService.GetAllCities();
			Assert.NotNull(result);
			Assert.Equal(2,result.Count);
			Assert.Equal("istabnul", result[0].Text);

			
        }
		[Fact]
		public void GetLocation_Test() 
		{
			var location = CreateLocation();
			
			_locationMock.Setup(a=>a.GetAll()).Returns(location.AsQueryable);

			var result = _locationService.GetAllDistrict(1);

			Assert.NotNull(result);
			Assert.Equal(2,result.Count);
			Assert.Equal("üsküdar", result[0].Text);

		}

	}
}
