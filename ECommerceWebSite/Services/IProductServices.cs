using ECommerceWebSite.Models.ViewModels;
using System.Collections.Generic;

namespace ECommerceWebSite.Services
{
    public interface IProductServices
    {
        List<ProductViewModel> GetProducts(int categoryId,bool deleted = false);
        List<ProductViewModel> GetProducts(string categoryId, bool deleted = false);
        int GetCategoryId(string categoryName);

    }
}