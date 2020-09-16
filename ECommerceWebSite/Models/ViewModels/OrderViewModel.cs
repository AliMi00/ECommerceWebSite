using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Models.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public ICollection<OrderDetailViewModel> Details { get; set; }
        [Display(Name = "مجموع قیمت")]
        public int TotalPrice { get; set; }
    }
}
