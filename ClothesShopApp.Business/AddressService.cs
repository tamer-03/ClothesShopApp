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
    public class AddressService
    {
        private readonly IRepository<Address> _repository;
        public AddressService(IRepository<Address> repository)
        {
            _repository = repository;
        }
        public IQueryable<Address> GetAllAddress()
        {
            return _repository.GetAll();
        }

      
        public void UpdateAddress(Address address)
        {
            _repository.Update(address);
        }
        public void DeleteAddress(int id)
        {
            _repository.Delete(id);
        }
        public void CreateAddress(Address address)
        {
            _repository.Create(address);
        }

        
        public Address AddAddressModelToAddress(EditAddressViewModel model)
        {
            var newAddress = new Address
            {
                addressId = model.Address.addressId,
                addressHeader = model.Address.addressHeader,
                firstName = model.Address.firstName,
                lastName = model.Address.lastName,
                phone = model.Address.phone,
                
                addresses = model.Address.addresses,
                LocationId = model.SelectedDistrictId == 0 ? model.Address.Location.locationId : model.SelectedDistrictId
            };

            return newAddress;
        }

        public List<Address> GetAddressLocationList(int userId) 
        {
            var address = GetAllAddress()
                    .Where(u => u.userId == userId)
                    .Include(l => l.Location)
                    .ThenInclude(p => p.parentLocation).ToList();

            return address;
        }

        public Address GetAddressLocationById(int id) 
        {
            var address = GetAllAddress().Include(l => l.Location).ThenInclude(p=>p.parentLocation).FirstOrDefault(a=> a.addressId == id);
            return address;
        
        }
    }
    
}
