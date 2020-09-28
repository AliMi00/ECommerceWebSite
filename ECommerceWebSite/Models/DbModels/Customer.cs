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
        public string Name { get; set; }
        public string FamilyName { get; set; }

        public string PostCode { get; set; }
        public string Address { get; set; }

        public  ICollection<Order> Orders { get; set; }
    }
}
