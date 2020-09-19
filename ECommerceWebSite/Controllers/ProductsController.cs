using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ECommerceWebSite.Models.ViewModels;
using ECommerceWebSite.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWebSite.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductServices productServices;
        private readonly IOrderServices orderServices;
        private readonly ICartServices cartServices;

        string UserName
        {
            get { return User.FindFirstValue(ClaimTypes.Name); }
        }
        public ProductsController(IProductServices productServices,IOrderServices orderServices,ICartServices cartServices)
        {
            this.productServices = productServices;
            this.orderServices = orderServices;
            this.cartServices = cartServices;
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

        [Produces("application/json")]
        public IActionResult AddProductToCart(int productId, int quantity = 1)
        {
            ProductAddResponseViewModel result = cartServices.addToCart(UserName, productId, quantity);
            return Json(result);
        }

        [Produces("application/json")]
        public IActionResult GetProductOfOpenCart()
        {
            //CartListViewModel respon = new CartListViewModel();
            //Customer customer = this.GetCustomer(userName);
            //if (customer == null)
            //{
            //    respon = new CartListViewModel()
            //    {
            //        CartItems = new List<CartViewModel>(),
            //        TotalPrice = 0
            //    };
            //    return respon;
            //}
            //respon.CartItems =
            //    db.CartItems.Where(c => c.Customer == customer &&
            //                            !c.DeletedDate.HasValue &&
            //                            !c.RemoveDate.HasValue &&
            //                            !c.Product.DisableDate.HasValue &&
            //                            !c.Product.RemoveDate.HasValue)
            //                 .Select(x => new CartViewModel()
            //                 {
            //                     Id = x.Id,
            //                     ImageAddress = x.Product.PictureAddress,
            //                     Price = x.Product.Price,
            //                     Quantity = x.Quantity,
            //                     Title = x.Product.Title
            //                 }).ToList();
            //respon.TotalPrice = respon.CartItems.Sum(x => x.Price * x.Quantity);

            //return respon;



            var cart = cartServices.GetCartList(UserName);
            CartListViewModel result =new CartListViewModel();
            if (cart != null)
            {

                result.CartItems =
                    cart.Where(c => !c.DeletedDate.HasValue &&
                                    !c.RemoveDate.HasValue &&
                                    !c.Product.DisableDate.HasValue &&
                                    !c.Product.RemoveDate.HasValue)
                        .Select(x => new CartViewModel()
                        {
                            Id = x.Id,
                            ImageAddress = x.Product.PictureAddress,
                            Price = x.Product.Price,
                            Quantity = x.Quantity,
                            Title = x.Product.Title
                        }).ToList();
                result.TotalPrice = result.CartItems.Sum(x => x.Price * x.Quantity);

                //result.CartItems = cart.Select(x => new CartViewModel() 
                //{
                //    Id = x.Id,
                //    ImageAddress = x.Product.PictureAddress,
                //    Price =x.Product.Price,
                //    Quantity = x.Quantity,
                //    Title = x.Product.Title
                //}).ToList();
                //result.TotalPrice = cart.Sum(x => x.Product.Price * x.Quantity);
                
            }
            else
            {

                result = new CartListViewModel()
                {
                    CartItems = new List<CartViewModel>(),
                    TotalPrice = 0
                };
                //result = new CartListViewModel()
                //{
                //    CartItems = new List<CartViewModel>(),
                //    TotalPrice = 0
                //};

            }
            return Json(result);
        }
    }
}
