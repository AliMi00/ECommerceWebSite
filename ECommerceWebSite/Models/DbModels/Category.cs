using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceWebSite.Models.DbModels
{
    public class Category
    {
        public int Id { get; set; }
        [StringLength(30, MinimumLength = 5)]
        [Required]
        [Display(Name = "عنوان دسته")]
        public string Title { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreationDate { get; set; }
        public DateTime? DisableDate { get; set; }
        public DateTime? RemoveDate { get; set; }
        [MaxLength(100)]
        public string PictureAddress { get; set; }
    }
}