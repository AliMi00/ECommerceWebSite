using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerceWebSite.Models.DbModels
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public Category Cateory { get; set; }
        public Product Product { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreationDate { get; set; }
        public DateTime? DisableDate { get; set; }
        public DateTime? RemoveDate { get; set; }
    }
}