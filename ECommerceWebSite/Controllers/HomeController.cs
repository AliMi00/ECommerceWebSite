using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ECommerceWebSite.Models;
using ECommerceWebSite.Data;
using ECommerceWebSite.Services;
using ECommerceWebSite.Models.ViewModels;
using ECommerceWebSite.Models.DbModels;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceWebSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IApplicationDbContext db;
        private readonly ICategoryServices categoryServices;


        public HomeController(ILogger<HomeController> logger, IApplicationDbContext db, ICategoryServices categoryServices)
        {
            _logger = logger;
            this.db = db;
            this.categoryServices = categoryServices;
        }
        public IActionResult Index()
        {
            var model = new IndexViewModel()
            {
                categoryViewModels = categoryServices.GetCategories(),
                PageTitle = "دسته بندی ها "
            };
            return View(model);
        }
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //use to add dummy data to db for test
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult seeddb()
        {
            if (true)
            {
                List<Category> categories = new List<Category>()
                {
                    new Category()
                    {
                        Title = "خانگی",
                        PictureAddress ="034cc792-2c2e-4626-9a0f-8332139a214d_computer-wallpaper-hd-1440x900-409771.jpg",
                        CreationDate = DateTime.Now,
                    },
                    new Category()
                    {
                        Title = "ورزشی",
                        PictureAddress ="034cc792-2c2e-4626-9a0f-8332139a214d_computer-wallpaper-hd-1440x900-409771.jpg",
                        CreationDate = DateTime.Now,
                    },
                    new Category()
                    {
                        Title = "اداری",
                        CreationDate = DateTime.Now,
                        PictureAddress ="034cc792-2c2e-4626-9a0f-8332139a214d_computer-wallpaper-hd-1440x900-409771.jpg",

                    },
                     new Category()
                    {
                        Title = "اسباب‌بازی",
                        CreationDate = DateTime.Now,
                        PictureAddress ="034cc792-2c2e-4626-9a0f-8332139a214d_computer-wallpaper-hd-1440x900-409771.jpg",
                    }
                };

                if (db.Categories.Count() < 4)
                {
                    db.Categories.AddRange(categories);
                }
                db.SaveChanges();

                for (int i = 0; i < 50; i++)
                {
                    Product prod = new Product()
                    {
                        CreationDate = DateTime.Now,
                        Title = $"Product{i}",
                        PictureAddress = "034cc792-2c2e-4626-9a0f-8332139a214d_computer-wallpaper-hd-1440x900-409771.jpg",
                        Price = 1000,
                        Quantity = 100

                    };
                    db.Products.Add(prod);
                    db.SaveChanges();

                    ProductCategory productCategory = new ProductCategory()
                    {
                        Product = prod,
                        CreationDate = DateTime.Now,
                        Cateory = categories[new Random().Next(0, 3)],

                    };
                    db.ProductCategories.Add(productCategory);
                    db.SaveChanges();
                }



            }
            return Ok();
        }

    }
}
