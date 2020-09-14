using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerceWebSite.Models.DbModels
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Category Cateory { get; set; }
        public int ProdctId { get; set; }
        public Product Prodct { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreationDate { get; set; }
        public DateTime? DisableDate { get; set; }
        public DateTime? RemoveDate { get; set; }
    }
}