using ECommerceWebSite.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

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
        IQueryable<ProductViewModel> GetProducts(int categoryId,bool deleted = false);
        IQueryable<ProductViewModel> GetProducts(string categorNamed, bool deleted = false);
        IQueryable<ProductViewModel> GetProducts(bool deleted = false);
        ProductDetailViewModel GetProduct(int? id);
        IQueryable<ProductViewModel> GetSearchedProducts(string searchString, bool deleted = false);

    }
}