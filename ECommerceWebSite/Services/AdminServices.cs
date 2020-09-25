using ECommerceWebSite.Data;
using ECommerceWebSite.Models.DbModels;
using ECommerceWebSite.Models.ViewModels;
using ECommerceWebSite.Models.ViewModels.Admin;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Services
{
    public class AdminServices : IAdminServices
    {
        private readonly IApplicationDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        public AdminServices(IApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            this.db = db;
            this.webHostEnvironment = webHostEnvironment;
        }

        //return list of all products even mark as deleted 
        public async Task<List<Product>> GetProductAsync(bool include = false)
        {
            if (include)
            {
                return db.Products.Include(x => x.ProductTags).Include(x => x.ProductCategories).ToList();

            }
            return await db.Products.ToListAsync();
        }

        //return product and all categorys for adding product or editing it
        public async Task<AdminCreateProductViewModel> GetProduct(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var product = await db.Products.Where(x => x.Id == id).Include(x => x.ProductTags)
                .FirstOrDefaultAsync();
            var categories = await db.Categories.Where(x => !x.RemoveDate.HasValue).ToListAsync();

            if (product == null)
            {
                return null;
            }

            AdminCreateProductViewModel respons = new AdminCreateProductViewModel()
            {
                Product = product,
                categories = categories
            };
            return respons;
        }
        //Add product and tags and single category IMPORTANT need to fixing on client side to add multiple category 
        public async Task<AddProductViewModel> AddProduct(Product product, IFormFile file = null, int? CategoryId = null, ICollection<Tag> tags = null)
        {
            AddProductViewModel respons = new AddProductViewModel();
            if (product == null)
            {
                return new AddProductViewModel()
                {
                    Message = "Invalid Product",
                    Succeed = false
                };
            }
            if (file == null)
            {
                return new AddProductViewModel()
                {
                    Message = "Invalid Product",
                    Succeed = false
                };
            }
            else
            {
                try
                {
                    string imageAddress = await UploadImage(file);
                    product.PictureAddress = imageAddress;
                }
                catch
                {
                    respons.Message = "faild to upload image";
                    respons.Succeed = false;
                    return respons;
                }
                product.CreationDate = DateTime.Now;
                db.Products.Add(product);
                respons.Message = "Product Added";
                respons.Succeed = true;
            }
            if (CategoryId != null && respons.Succeed)
            {

                ProductCategory category = new ProductCategory()
                {
                    Product = product,
                    Cateory = db.Categories.SingleOrDefault(x => x.Id == CategoryId),
                    CreationDate = DateTime.Now,
                };
                db.ProductCategories.Add(category);
            }
            if (tags != null && respons.Succeed)
            {
                tags.Where(x => x.Product == null).Select(x => { x.Product = product; return x; }).ToList();
                db.Tags.AddRange(tags);
            }
            await db.SaveChangesAsync(true);
            return respons;

        }
        //edit product and tags and category IMPORTANT it does delete last tags and renew them 
        public async Task<AddProductViewModel> EditProduct(Product product, IFormFile file = null, int? CategoryId = null, ICollection<Tag> tags = null)
        {
            if (!db.Products.Any(x => x.Id == product.Id))
            {
                return new AddProductViewModel()
                {
                    Message = "Invalid Product",
                    Succeed = false
                };
            }
            AddProductViewModel respons = new AddProductViewModel();

            if (file != null)
            {
                try
                {
                    string imageAddress = await UploadImage(file);
                    product.PictureAddress = imageAddress;
                }
                catch
                {
                    respons.Message = "faild to upload image";
                    respons.Succeed = false;
                    return respons;
                }
            }
            if (CategoryId != null)
            {

                ProductCategory category = new ProductCategory()
                {
                    Product = product,
                    Cateory = db.Categories.SingleOrDefault(x => x.Id == CategoryId),
                    CreationDate = DateTime.Now,
                };
                db.ProductCategories.Add(category);
            }
            if (tags != null)
            {
                db.Tags.RemoveRange(db.Tags.Where(x => x.Product == product));
                db.Tags.AddRange(tags);
            }

            db.Products.Update(product);
            await db.SaveChangesAsync(true);
            respons.Message = "product updated";
            respons.Succeed = true;
            return respons;
        }
        //save image in wwwroot folder with uniqe id and return the file name 
        public async Task<string> UploadImage(IFormFile file)
        {
            string uniqueFileName = null;

            if (file != null)
            {

                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.ContentType;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return uniqueFileName;
        }
        //set remove date to product we dont delete the item bcoz of db issues 
        public AddProductViewModel DeleteProduct(Product product)
        {
            try
            {
                product.RemoveDate = DateTime.Now;
                db.Products.Update(product);
                db.SaveChanges();
                return new AddProductViewModel()
                {
                    Message = "product Deleted",
                    Succeed = true
                };
            }
            catch
            {
                return new AddProductViewModel()
                {
                    Message = "product  NOT Deleted",
                    Succeed = false
                };
            }


        }
        //add new category with picture picture is requeird 
        public async Task<AddProductViewModel> AddCategory(Category category, IFormFile file)
        {
            AddProductViewModel respons = new AddProductViewModel();
            if (category == null)
            {
                return new AddProductViewModel()
                {
                    Message = "Invalid Category",
                    Succeed = false
                };
            }
            if (file == null)
            {
                return new AddProductViewModel()
                {
                    Message = "Invalid Category picture",
                    Succeed = false
                };
            }
            else
            {
                try
                {
                    string imageAddress = await UploadImage(file);
                    category.PictureAddress = imageAddress;
                }
                catch
                {
                    respons.Message = "faild to upload image";
                    respons.Succeed = false;
                    return respons;
                }
                category.CreationDate = DateTime.Now;
                db.Categories.Add(category);
                respons.Message = "Category Added";
                respons.Succeed = true;
            }
            await db.SaveChangesAsync(true);
            return respons;
        }
        //edit category image is requeird 
        public async Task<AddProductViewModel> EditCategory(Category category, IFormFile file)
        {
            AddProductViewModel respons = new AddProductViewModel();
            if (category == null)
            {
                return new AddProductViewModel()
                {
                    Message = "Invalid Category",
                    Succeed = false
                };
            }
            if (file == null)
            {
                return new AddProductViewModel()
                {
                    Message = "Invalid Category picture",
                    Succeed = false
                };
            }
            else
            {
                try
                {
                    string imageAddress = await UploadImage(file);
                    category.PictureAddress = imageAddress;
                }
                catch
                {
                    respons.Message = "faild to upload image";
                    respons.Succeed = false;
                    return respons;
                }
                category.CreationDate = DateTime.Now;
                db.Categories.Update(category);
                respons.Message = "Category Added";
                respons.Succeed = true;
            }
            await db.SaveChangesAsync(true);
            return respons;
        }
        //set remove date for category it doesnt delete the category from db just add remove date 
        public async Task<AddProductViewModel> DeleteCategory(Category category)
        {
            AddProductViewModel respons = new AddProductViewModel();
            if (category == null)
            {
                respons.Message = "invalid Category";
                respons.Succeed = false;
                return respons;
            }
            try
            {
                category.RemoveDate = DateTime.Now;
                db.Categories.Update(category);
                await db.SaveChangesAsync(true);

                return new AddProductViewModel()
                {
                    Message = "Category Deleted",
                    Succeed = true
                };
            }
            catch
            {
                return new AddProductViewModel()
                {
                    Message = "Category NOT Deleted",
                    Succeed = false
                };
            }

        }
        //set remove date for category it doesnt delete the category from db just add remove date by id 
        public async Task<AddProductViewModel> DeleteCategory(int? id)
        {
            Category category = await GetCategoryAsync(id);
            return await DeleteCategory(category);

        }
        //get un deleted categories List
        public async Task<List<Category>> GetCategoriesAsync() => await db.Categories.Where(x => !x.RemoveDate.HasValue).ToListAsync();

        //get single category
        public async Task<Category> GetCategoryAsync(int? id)
        {
            if (id == null)
            {
                return null;
            }

            Category category = await db.Categories
                .FirstOrDefaultAsync(m => m.Id == id);

            return category;
        }
        public bool CategoryExists(int id)
        {
            return db.Categories.Any(e => e.Id == id);
        }

        //get orrder list 
        public async Task<List<Order>> GetOrdersAsync(OrderStatusTypes? orderStatus = null, DateTime? startDate = null, DateTime? endDate = null , bool include = false)
        {
            if (!startDate.HasValue)
            {
                startDate = DateTime.Today.AddDays(-7);
            }
            if (!endDate.HasValue)
            {
                endDate = DateTime.Now;
            }

            var orders = db.Orders.Where(x => x.OrderDate >= startDate &&
                                          x.OrderDate <= endDate);
            if (orderStatus != null)
            {
                orders.Where(x => x.OrderStatus == orderStatus);
            }
            if (include)
            {
                orders.Include(x => x.OrderDetails).ThenInclude(x => x.Product);
            }
            return await orders.ToListAsync();
        }

        public async Task<Order> GetOrderAsync(int? orderId, bool include = false)
        {
            var order = db.Orders.Where(x => x.Id == orderId);
            if (include)
            {
                order.Include(x => x.OrderDetails).ThenInclude(x => x.Product).ToList();
            }
            return await order.SingleOrDefaultAsync();

        }

        public ResponsViewModel UpdateOrderStatus(int? id,OrderStatusTypes orderStatus)
        {
            if(id == null)
            {
                return new ResponsViewModel()
                {
                    Message = "Invalid Order",
                    Succeed = false
                };
            }
            try
            {
                db.Orders.Where(x => x.Id == id).SingleOrDefault().OrderStatus = orderStatus;
                db.SaveChanges();

            }
            catch
            {
                return new ResponsViewModel()
                {
                    Message = "something wrong",
                    Succeed = false
                };
            }
            return new ResponsViewModel()
            {
                Message = "Order Updated",
                Succeed = true
            };
        }
    }
}
