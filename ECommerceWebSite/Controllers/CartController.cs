using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ECommerceWebSite.Models.DbModels;
using ECommerceWebSite.Models.ViewModels;
using ECommerceWebSite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWebSite.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartServices cartServices;
        private readonly IHttpContextAccessor httpContextAccessor;
        string UserName
        {
            get
            {
                return User.FindFirstValue(ClaimTypes.Name);
            }
        }

        public CartController(ICartServices cartServices, IHttpContextAccessor httpContextAccessor)
        {
            this.cartServices = cartServices;
            this.httpContextAccessor = httpContextAccessor;

        }


        public async Task<IActionResult> Index()
        {
            string tempCartId = httpContextAccessor.HttpContext.Request.Cookies["tempCartId"];
            List<TempCartItem> tempCarts = cartServices.GetTempCartList(tempCartId);
            if(tempCarts != null)
            {
                foreach (TempCartItem item in tempCarts)
                {
                    cartServices.addToCart(UserName, item.Product.Id, item.Quantity);
                }
                await cartServices.DeleteTempCart(tempCartId);
                
            }
            List<CartItem> cart = cartServices.GetCartList(UserName);
            CartListViewModel viewModel = new CartListViewModel();
            if (cart != null)
            {
                viewModel.CartItems = cart
                    .Select(x => new CartViewModel()
                    {
                        Id = x.Id,
                        ImageAddress = x.Product.PictureAddress,
                        Price =x.Product.Price,
                        Quantity =x.Quantity,
                        Title = x.Product.Title

                    }).ToList();
                viewModel.TotalPrice = cart.Sum(x => x.Quantity * x.Product.Price);
            }
            else
            {
                viewModel = new CartListViewModel()
                {
                    CartItems = new List<CartViewModel>(),
                    TotalPrice = 0


                };
            }
            return View(viewModel);
        }

        //TODO late
        //we can use this on ajax to prevent page reloading TODO late
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            ResponsViewModel respons = new ResponsViewModel();
            respons = await cartServices.DeleteCartItem(id);


            return RedirectToAction(nameof(Index));
        }

        //get data by ajax to prevent reloading page
        [Produces("application/json")]
        public IActionResult GetCartList()
        {
            List<CartItem> cart = cartServices.GetCartList(UserName);
            CartListViewModel viewModel = new CartListViewModel();
            if (cart != null)
            {
                viewModel.CartItems = cart
                    .Select(x => new CartViewModel()
                    {
                        Id = x.Id,
                        ImageAddress = x.Product.PictureAddress,
                        Price = x.Product.Price,
                        Quantity = x.Quantity,
                        Title = x.Product.Title

                    }).ToList();
                viewModel.TotalPrice = cart.Sum(x => x.Quantity * x.Product.Price);
            }
            else
            {
                viewModel = new CartListViewModel()
                {
                    CartItems = new List<CartViewModel>(),
                    TotalPrice = 0


                };
            }
            return Json(viewModel);
        }

    }
}
