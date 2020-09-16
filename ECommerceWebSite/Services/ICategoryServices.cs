using ECommerceWebSite.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Services
{
    public interface ICategoryServices
    {
        List<CategoryViewModel> GetCategories();
    }
}
