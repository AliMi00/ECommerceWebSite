using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ECommerceWebSite.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWebSite.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductServices productServices;
        private readonly IOrderServices orderServices;

        string UserName
        {
            get { return User.FindFirstValue(ClaimTypes.Name); }
        }
        public ProductsController(IProductServices productServices,IOrderServices orderServices)
        {
            this.productServices = productServices;
            this.orderServices = orderServices;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CategoryFilter(int categoryId)
        {
            var products =  productServices.GetProducts(categoryId);
            return View(products);
        }
    }
}
