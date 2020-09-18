using ECommerceWebSite.Data;
using ECommerceWebSite.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Services
{
    public class ProductServices :IProductServices
    {
        private readonly IApplicationDbContext db;
        public ProductServices(IApplicationDbContext _db)
        {
            db = _db;
        }
        /// <summary>
        /// get all the available products by category filter
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="deleted"></param>
        /// <returns></returns>
        public List<ProductViewModel> GetProducts(int categoryId, bool deleted = false)
        {
            return db.Products
                .Where(p => !p.RemoveDate.HasValue &&
                            !p.DisableDate.HasValue &&
                            p.ProductCategories
                            .Any(pc =>
                                 pc.Cateory.Id == categoryId &&
                                 !pc.DisableDate.HasValue &&
                                 !pc.RemoveDate.HasValue &&
                                 !pc.Cateory.RemoveDate.HasValue &&
                                 !pc.Cateory.DisableDate.HasValue)
                            )
                .Select(x => new ProductViewModel()
                {
                    ImageAddress = x.PictureAddress,
                    Title = x.Title,
                    Id = x.Id,
                    Price = x.Price
                }).ToList();
        }

        public List<ProductViewModel> GetProducts(string categoryName, bool deleted)
        {
            return GetProducts(GetCategoryId(categoryName));
        }
        public int GetCategoryId(string categoryName)
        {
            return db.Categories.Single(c => c.Title == categoryName).Id;
        }
     
    }
}
