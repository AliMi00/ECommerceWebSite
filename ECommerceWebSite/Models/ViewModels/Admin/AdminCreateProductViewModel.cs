using ECommerceWebSite.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Models.ViewModels.Admin
{
    public class AdminCreateProductViewModel
    {
        public Product Product { get; set; }
        public List<Category> categories { get; set; }
        public string Tags { get; set; }

    }
}
