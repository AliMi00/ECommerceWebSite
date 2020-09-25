using ECommerceWebSite.Models.ViewModels;
using System.Collections.Generic;

namespace ECommerceWebSite.Services
{
    public interface IProductServices
    {
        /// <summary>
        /// get all the available products by category filter
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="deleted"></param>
        /// <returns></returns>
        List<ProductViewModel> GetProducts(int categoryId,bool deleted = false);
        List<ProductViewModel> GetProducts(string categoryId, bool deleted = false);
        List<ProductViewModel> GetProducts(bool deleted = false);
        ProductDetailViewModel GetProduct(int? id);
        List<ProductViewModel> GetSearchedProducts(string searchString, bool deleted = false);

    }
}