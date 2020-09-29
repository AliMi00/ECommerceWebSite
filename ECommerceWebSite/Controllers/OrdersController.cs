using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ECommerceWebSite.Models;
using ECommerceWebSite.Models.DbModels;
using ECommerceWebSite.Models.ViewModels;
using ECommerceWebSite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZarinpalSandbox;

namespace ECommerceWebSite.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderServices orderServices;
        private readonly IProductServices productServices;
        private readonly ICartServices cartServices;


        string UserName
        {
            get { return User.FindFirstValue(ClaimTypes.Name); }
        }

        public OrdersController(IOrderServices orderServices,IProductServices productServices,ICartServices cartServices)
        {
            this.orderServices = orderServices;
            this.productServices = productServices;
            this.cartServices = cartServices;
        }
        //use for future feature 
        public IActionResult GetUserOrdersReport()
        {
            var orders = orderServices.CustomerOrders(UserName);
            return Json(orders);
        }
        //use for future feature 
        public IActionResult GetUserDetailsReport(int OrderId)
        {
            var order = orderServices.CustomerOrderDetails(OrderId, UserName);
            return Json(order);
        }

        //show all orders of customer
        public IActionResult ShowOrders()
        {
            var order = orderServices.CustomerOrders(UserName,null,null,false); 
            
            List<ShowOrderViewModel> viewModel;
            if(order != null)
            {
                viewModel = order.Select(x => new ShowOrderViewModel()
                {
                    Id = x.Id,
                    TotalPrice = x.AmountBuy,
                    OrderDate = x.PaymentDate,
                    Status = x.OrderStatus.ToString()

                }).ToList();
            }
            else
            {
                viewModel = new List<ShowOrderViewModel>();
            }
            return View(viewModel);
        }

        //validate cart in case of invalid return to check cart again or pay
        //in case of error make order cancel 
        public async Task<IActionResult> CheckOut()
        {
            int initCount = cartServices.GetCartList(UserName).Count;
            int resCount = orderServices.ValidatingCartBeforAddingToCart(UserName);

            if (initCount != resCount)
            {
                return RedirectToAction(nameof(Index), "Cart");
            }

            var order = orderServices.GetOrder(UserName);
            int count = orderServices.AddProductToCart(UserName);
         

            var totalPrice = orderServices.GetOrder(UserName)?.AmountBuy;

            if (!totalPrice.HasValue)
                return View("Error");
            if(initCount != orderServices.GetOrder(UserName, null,true).OrderDetails.Count)
            {
                if (orderServices.CancelingOpenOrder(UserName))
                {
                    return View("Error", new ErrorViewModel() { RequestId = "Something wrong" });
                }

                return View("Error");
            }

            var payment = await new Payment(totalPrice.Value)
                .PaymentRequest("متن تست موقع خرید",
                    Url.Action(nameof(CheckOutCallback), "Orders", new { amount = totalPrice }, Request.Scheme));

            if(payment.Status == 100)
            {
                if (orderServices.AddAuthorityToOrder(UserName, payment.Authority))
                {
                     return (IActionResult)Redirect(payment.Link);
                }
                orderServices.CancelingOpenOrder(UserName);
                return BadRequest("خطا در پرداخت");
            }
            else
            {
                orderServices.CancelingOpenOrder(UserName);
                return BadRequest($"خطا در پرداخت. کد خطا:{payment.Status}");
            }
        }
        public async Task<IActionResult> CheckOutCallback(int amount, string Authority, string Status)
        {
            if (Status == "NOK")
            {
                cartServices.AddOrderDetailsToCart(UserName);
                return View("Error", new ErrorViewModel() { RequestId = "khkhkhkhkh" });
            }
            var verification = await new Payment(amount)
                .Verification(Authority);

            ViewResult response;
            if (verification.Status != 100 || !orderServices.PayForOrder(amount, UserName))
            {
                response = View("Error");
                orderServices.CancelingOpenOrder(UserName);
            }
            else
            {
                response = View("Success");
            }
            return response;
        }
        /// <summary>
        /// use to checkout after main checkout part mainly in order list 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> CheckAgainOut(int id)
        {
            var totalPrice = orderServices.GetOrder(UserName,id)?.AmountBuy;

            if (!totalPrice.HasValue)
                return View("Error");

            var payment = await new Payment(totalPrice.Value)
                .PaymentRequest("متن تست موقع خرید",
                    Url.Action(nameof(CheckOutCallback), "Orders", new { amount = totalPrice }, Request.Scheme));

            if (payment.Status == 100)
            {
                if (orderServices.AddAuthorityToOrder(UserName, payment.Authority))
                {
                    return (IActionResult)Redirect(payment.Link);
                }
                return BadRequest("خطا در پرداخت");
            }
            else
            {
                orderServices.CancelingOpenOrder(UserName);
                return BadRequest($"خطا در پرداخت. کد خطا:{payment.Status}");
            }
        }

    }
}
