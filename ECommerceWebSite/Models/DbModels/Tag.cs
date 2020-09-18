using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Models.DbModels
{
    public class Tag
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public string tag { get; set; }

    }
}
