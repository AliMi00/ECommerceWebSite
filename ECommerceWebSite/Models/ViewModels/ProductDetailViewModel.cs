using ECommerceWebSite.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Models.ViewModels
{
    public class ProductDetailViewModel
    {
        public int Id { get; set; }
        [MaxLength(30)]
        [MinLength(5)]
        [Required]
        [Display(Name = "عنوان محصول")]
        public string Title { get; set; }
        public int Price { get; set; }
        public string PictureAddress { get; set; }
        public string ProductTags { get; set; }

    }
}
