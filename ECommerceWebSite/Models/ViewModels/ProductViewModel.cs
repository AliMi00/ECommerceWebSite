using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Models.ViewModels
{
    public class ProductViewModel
    {
        public string ImageAddress { get; set; }
        public string Title { get; set; }
        public int Id { get; set; }
        public int Price { get; set; }

    }
}
