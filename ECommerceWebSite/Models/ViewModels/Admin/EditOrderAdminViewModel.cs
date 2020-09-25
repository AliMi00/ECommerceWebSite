using ECommerceWebSite.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Models.ViewModels.Admin
{
    public class EditOrderAdminViewModel
    {
        public Order Order { get; set; }
        public ResponsViewModel ResponsViewModel { get; set; }
    }
}
