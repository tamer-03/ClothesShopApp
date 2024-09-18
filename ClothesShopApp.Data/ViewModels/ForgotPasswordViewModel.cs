using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Data.ViewModels
{
    public class ForgotPasswordViewModel
    {
        public string Email { get; set; }
        public string Phone {  get; set; }
        public bool isUserValide { get; set; }
    }
}
