using ClothesShopApp.Data.Entity;
using ClothesShopApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages.Html;

namespace ClothesShopApp.Business
{
    public class LocationService
    {
        private readonly IRepository<Location> _repository;

        public LocationService(IRepository<Location> repository)
        {
            _repository = repository;
        }

        public IQueryable<Location> GetAllLocation() 
        {
            return _repository.GetAll();
        }

        public void CreateLocation(Location location)
        {
            _repository.Create(location);
        }
        public void UpdateLocation(Location location)
        {
            _repository.Update(location);
        }
        public void DeleteLocation(int id)
        {
            _repository.Delete(id);
        }

        public List<SelectListItem> GetAllCities()
        {
           return _repository.GetAll()
                            .Where(l => l.parentLocationId == null)
                            .Select(c => new SelectListItem
                            {
                                Value = c.locationId.ToString(),
                                Text = c.name
                            }).ToList();
        }

        public List<SelectListItem> GetAllDistrict(int? selectedId)
        {
            return _repository.GetAll()
                           .Where(l => l.parentLocationId == selectedId)
                           .Select(c => new SelectListItem
                           {
                               Value = c.locationId.ToString(),
                               Text = c.name
                           }).ToList();
        }
    }
}
