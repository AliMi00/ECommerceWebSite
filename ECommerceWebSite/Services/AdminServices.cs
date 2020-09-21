﻿using ECommerceWebSite.Data;
using ECommerceWebSite.Models.DbModels;
using ECommerceWebSite.Models.ViewModels.Admin;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        public AdminServices(IApplicationDbContext db , IWebHostEnvironment webHostEnvironment)
        {
            this.db = db;
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<AddProductViewModel> AddProduct(Product product,IFormFile file = null, ICollection<ProductCategory> productCategories = null, ICollection<Tag> tags = null)
        {
            AddProductViewModel respons = new AddProductViewModel();
            if(product == null)
            {
                return new AddProductViewModel()
                {
                    Message = "Invalid Product",
                    Succeed = false
                };
            }
            if(file == null)
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
            if(productCategories != null && respons.Succeed)
            {
                productCategories.Where(x => x.Product == null).Select(x => { x.Product = product; return x; }).ToList();
                db.ProductCategories.AddRange(productCategories);
            }
            if(tags != null && respons.Succeed)
            {
                tags.Where(x => x.Product == null).Select(x => { x.Product = product; return x; }).ToList();
                db.Tags.AddRange(tags);
            }
            await db.SaveChangesAsync(true);
            return respons;

        }



        public async Task<AddProductViewModel> EditProduct(Product product, IFormFile file = null, ICollection<ProductCategory> productCategories = null, ICollection<Tag> tags = null)
        {
            if(!db.Products.Any(x => x.Id == product.Id))
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
            if(productCategories != null)
            {
                db.ProductCategories.RemoveRange(db.ProductCategories.Where(x => x.Product == product));
                db.ProductCategories.AddRange(productCategories);

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

        public async Task<string> UploadImage(IFormFile file)
        {
            string uniqueFileName = null;

            if (file != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                uniqueFileName = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(uniqueFileName, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return uniqueFileName;
        }

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


    }
}
