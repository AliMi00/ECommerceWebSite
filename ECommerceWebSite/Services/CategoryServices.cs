using ECommerceWebSite.Data;
using ECommerceWebSite.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IApplicationDbContext db;
        public CategoryServices(IApplicationDbContext _db)
        {
            db = _db;
        }
        public List<CategoryViewModel> GetCategories()
        {
            return db.Categories
                .Where(c => !c.RemoveDate.HasValue &&
                            !c.DisableDate.HasValue)
                .Select(x => new CategoryViewModel()
                {
                    ImageAddress = x.PictureAddress,
                    Id = x.Id,
                    Title = x.Title,
                    ProductCount = x.ProductCategories.Count(c => !c.DisableDate.HasValue &&
                                                                  !c.RemoveDate.HasValue &&
                                                                  !c.Product.DisableDate.HasValue &&
                                                                  !c.Product.RemoveDate.HasValue)
                }).ToList();
        }

    }
}
