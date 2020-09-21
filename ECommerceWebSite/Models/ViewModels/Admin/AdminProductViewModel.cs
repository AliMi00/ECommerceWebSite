using ECommerceWebSite.Models.DbModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Models.ViewModels.Admin
{
    public class AdminProductViewModel
    {
        public int Id { get; set; }
        [MaxLength(30)]
        [MinLength(5)]
        [Required]
        [Display(Name = "عنوان محصول")]
        public string Title { get; set; }
        public int Price { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreationDate { get; set; }
        public DateTime? DisableDate { get; set; }
        public DateTime? RemoveDate { get; set; }
        public IFormFile Picture { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; }
        public ICollection<Tag> ProductTags { get; set; }
        public int Quantity { get; set; }
    }
}
