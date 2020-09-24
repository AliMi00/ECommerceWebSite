using ECommerceWebSite.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Models.ViewModels
{
    public class ShowOrderViewModel
    {
        [Display(Name ="شماره سفارش")]
        public int Id { get; set; }
        [Display(Name = "مجموع قیمت")]
        public int TotalPrice { get; set; }
        [Display(Name = "وضعیت سفارش")]
        public string Status { get; set; }
        [Display(Name = "تاریخ سفارش")]
        public DateTime? OrderDate { get; set; }
    }
}
