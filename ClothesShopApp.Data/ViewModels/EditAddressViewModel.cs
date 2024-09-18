using ClothesShopApp.Data.Entity;
using System.Web.WebPages.Html;

namespace ClothesShopApp.Data.ViewModels
{
    public class EditAddressViewModel
    {
        public Address Address { get; set; }
        public List<SelectListItem> Cities { get; set; }
        public List<SelectListItem> Districts { get; set; }
        public int SelectedCityId { get; set; }
        public int SelectedDistrictId { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string Phone { get; set; }
        //public string AddressDetails { get; set; }
        //public string AddressHeader { get; set; }

    }
}
