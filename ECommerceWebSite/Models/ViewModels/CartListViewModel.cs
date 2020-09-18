using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Models.ViewModels
{
    public class CartListViewModel
    {
        public ICollection<CartViewModel> CartItems { get; set; }
        [Display(Name = "مجموع قیمت")]
        public int TotalPrice { get; set; }
    }
}
