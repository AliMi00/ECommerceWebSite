using ECommerceWebSite.Models.DbModels;
using ECommerceWebSite.Models.ViewModels;
using ECommerceWebSite.Models.ViewModels.Admin;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Services
{
    /// <summary>
    /// This Service Use just for Admin Panel 
    /// </summary>
    public interface IAdminServices
    {
        /// <summary>
        /// save file in images folder and return unique file path of the file 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<string> UploadImage(IFormFile file);
        Task<AddProductViewModel> AddProduct(Product product, IFormFile file, ICollection<ProductCategory> productCategories = null,ICollection<Tag> tags = null);
        Task<AddProductViewModel> EditProduct(Product product, IFormFile file, ICollection<ProductCategory> productCategories = null, ICollection<Tag> tags = null);
        AddProductViewModel DeleteProduct(Product product);
        Task<Product> GetProduct(int? id);
        Task<List<Product>> GetProductAsync(bool include = false);


    }
}
