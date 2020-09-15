using ECommerceWebSite.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Services
{
    interface ICategoryServices
    {
        List<CategoryViewModel> GetCategories();
    }
}
