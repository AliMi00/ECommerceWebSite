using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Models.DbModels
{
    public class Customer : IdentityUser
    {
        [Required]
        public string PostCode { get; set; }
        public string Address { get; set; }

        public  ICollection<Order> Orders { get; set; }
    }
}
