using ECommerceWebSite.Data;
using ECommerceWebSite.Models.DbModels;
using ECommerceWebSite.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IApplicationDbContext db;
        public ProductServices(IApplicationDbContext _db)
        {
            db = _db;
        }
        
        // get all the available products by category filter2
        public IQueryable<ProductViewModel> GetProducts(int categoryId, bool deleted = false)
        {
            return db.Products
                .Where(p => !p.RemoveDate.HasValue &&
                            !p.DisableDate.HasValue &&
                            p.Quantity > 0 &&
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
                });
        }
        public IQueryable<ProductViewModel> GetProducts(bool deleted = false)
        {
            return db.Products
                .Where(p => !p.RemoveDate.HasValue &&
                            !p.DisableDate.HasValue &&
                            p.Quantity > 0
                            )
                .Select(x => new ProductViewModel()
                {
                    ImageAddress = x.PictureAddress,
                    Title = x.Title,
                    Id = x.Id,
                    Price = x.Price
                });
        }
        //use when we need to access with category name 
        public IQueryable<ProductViewModel> GetProducts(string categoryName, bool deleted)
        {
            return GetProducts(GetCategoryId(categoryName), deleted);
        }
        private int GetCategoryId(string categoryName)
        {
            return db.Categories.Single(c => c.Title == categoryName).Id;
        }
        public ProductDetailViewModel GetProduct(int? id)
        {
            if(id == null)
            {
                return null;
            }
            ProductDetailViewModel respos = new ProductDetailViewModel();
            Product product = db.Products.
                Where(x => x.Id == id && !x.DisableDate.HasValue && !x.RemoveDate.HasValue).Include(x => x.ProductTags).SingleOrDefault();
            if (product == null)
            {
                return null;
            }

            string tags = "";
            foreach (Tag tag in product.ProductTags)
            {
                tags += " " + tag.tag;
            }
            respos.Id = product.Id;
            respos.PictureAddress = product.PictureAddress;
            respos.Price = product.Price;
            respos.Title = product.Title;
            respos.ProductTags = tags;

            return respos;
        }
        public IQueryable<ProductViewModel> GetSearchedProducts(string searchString, bool deleted = false)
        {
           var respons = db.Products
                .Where(p => !p.RemoveDate.HasValue &&
                            !p.DisableDate.HasValue &&
                            p.Quantity > 0 &&
                            p.Title.Contains(searchString))
                .Select(x => new ProductViewModel()
                {
                    ImageAddress = x.PictureAddress,
                    Title = x.Title,
                    Id = x.Id,
                    Price = x.Price
                });
            respons.Union( db.Tags
                .Where(t => t.tag.Contains(searchString))
                .Select(x => new ProductViewModel()
                {
                    ImageAddress = x.Product.PictureAddress,
                    Id = x.Product.Id,
                    Price = x.Product.Price,
                    Title = x.Product.Title
                }));
            return respons;
        }

    }
}
