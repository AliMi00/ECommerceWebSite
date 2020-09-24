using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ECommerceWebSite.Models.ViewModels;
using ECommerceWebSite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWebSite.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductServices productServices;
        private readonly IOrderServices orderServices;
        private readonly ICartServices cartServices;
        private readonly IHttpContextAccessor httpContextAccessor;

        string UserName
        {
            get { return User.FindFirstValue(ClaimTypes.Name); }
        }
        public ProductsController(IProductServices productServices,IOrderServices orderServices,ICartServices cartServices, IHttpContextAccessor httpContextAccessor)
        {
            this.productServices = productServices;
            this.orderServices = orderServices;
            this.cartServices = cartServices;
            this.httpContextAccessor = httpContextAccessor;
        }

        //return all products
        public IActionResult Index()
        {
            var products =  productServices.GetProducts();
            return View("CategoryFilter",products);
        }
        //return category product 
        public IActionResult CategoryFilter(int categoryId)
        {
            var products =  productServices.GetProducts(categoryId);
            return View(products);
        }

        //add product to cart 
        [Produces("application/json")]
        public IActionResult AddProductToCart(int productId, int quantity = 1)
        {
            ProductAddToOrderViewModel result;
            if (UserName == null)
            {
                //here the code for managing the un authorize customer to store cart in temp table with uniqe id and store the id in user cookie to retrive the data 
                string tempCartId = httpContextAccessor.HttpContext.Request.Cookies["tempCartId"];
                if(tempCartId == null ||tempCartId == "")
                {
                    Guid g = Guid.NewGuid();
                    tempCartId = g.ToString();
                    CookieOptions option = new CookieOptions();
                    option.Expires = DateTime.Now.AddDays(7);
                    Response.Cookies.Append("tempCartId", tempCartId, option);
                }
                result = cartServices.addToTempCart(tempCartId, productId, quantity);
            }
            else
            {
                result = cartServices.addToCart(UserName, productId, quantity);

            }
            return Json(result);
        }

        //get cart list check for user if is unauthorize user get from his tempCartId 
        [Produces("application/json")]
        public IActionResult GetProductOfOpenCart()
        {
            CartListViewModel result = new CartListViewModel();
            if (UserName == null|| UserName == "")
            {
                string tempCartId = httpContextAccessor.HttpContext.Request.Cookies["tempCartId"];

                var cart = cartServices.GetTempCartList(tempCartId);
                if (cart != null)
                {
                    result.CartItems =
                        cart.Where(c => !c.DeletedDate.HasValue &&
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

                }
                else
                {

                    result = new CartListViewModel()
                    {
                        CartItems = new List<CartViewModel>(),
                        TotalPrice = 0
                    };


                }
            }
            else
            {
                var cart = cartServices.GetCartList(UserName);

                
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

                }
                else
                {

                    result = new CartListViewModel()
                    {
                        CartItems = new List<CartViewModel>(),
                        TotalPrice = 0
                    };


                }
            }
            return Json(result);
        }
    }
}
