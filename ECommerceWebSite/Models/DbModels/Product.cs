using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceWebSite.Models.DbModels
{
    public class Product
    {
        public int Id { get; set; }
        [MaxLength(30)]
        [MinLength(5)]
        [Required]
        [Display(Name = "عنوان محصول")]
        public string Title { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public int Price { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? DisableDate { get; set; }
        public DateTime? RemoveDate { get; set; }
        public string PictureAddress { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; }
        public ICollection<Tag> ProductTags { get; set; }
        public int Quantity { get; set; }


    }
}