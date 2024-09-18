
using System.ComponentModel.DataAnnotations;
using System.Web.WebPages.Html;

namespace ClothesShopApp.Data.ViewModels
{
    public class RegisterViewModel
    {
        
        public string name { get; set; }

        
        public string lastName { get; set; }

       
        public string email { get; set; }

       
        public string password { get; set; }
        public string password1 { get; set; }

        public string phone { get; set; }
        public string intentityNumber { get; set; }
        public int selectedCityId { get; set; }

        
        public int selectedDistrictId { get; set; }


        public List<SelectListItem> cities { get; set; }
        public List<SelectListItem> districts { get; set; }

    }
}
