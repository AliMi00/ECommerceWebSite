using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ECommerceWebSite.Models.DbModels;
using ECommerceWebSite.Models.ViewModels;
using ECommerceWebSite.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWebSite.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartServices cartServices;
        string UserName
        {
            get
            {
                return User.FindFirstValue(ClaimTypes.Name);
            }
        }

        public CartController(ICartServices cartServices)
        {
            this.cartServices = cartServices;

        }


        public IActionResult Index()
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
    }
}
