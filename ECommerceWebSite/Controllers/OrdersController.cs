using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ECommerceWebSite.Models;
using ECommerceWebSite.Models.DbModels;
using ECommerceWebSite.Models.ViewModels;
using ECommerceWebSite.Services;
using Microsoft.AspNetCore.Mvc;
using ZarinpalSandbox;

namespace ECommerceWebSite.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderServices orderServices;
        private readonly IProductServices productServices;

        string UserName
        {
            get { return User.FindFirstValue(ClaimTypes.Name); }
        }

        public OrdersController(IOrderServices orderServices,IProductServices productServices)
        {
            this.orderServices = orderServices;
            this.productServices = productServices;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetUserOrdersReport()
        {
            var orders = orderServices.CustomerOrders(UserName);
            return Json(orders);
        }

        public IActionResult GetUserDetailsReport(int OrderId)
        {
            var order = orderServices.CustomerOrderDetails(OrderId, UserName);
            return Json(order);
        }

        //show sent orders 
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

        
        public IActionResult AddToOrder()
        {
            int count = orderServices.AddProductToCart(UserName);

            var order = orderServices.GetOrder(UserName);

            OrderViewModel model;
            if (order != null)
            {
                model = orderServices.GetOrderDetails(order.Id, UserName);
            }
            else
            {
                model = new OrderViewModel()
                {
                    Details = new List<OrderDetailViewModel>(),
                    TotalPrice = 0
                };
            }
            

            return View(model);
        }

        //
        public async Task<IActionResult> CheckOut()
        {
            
            

            var totalPrice = orderServices.GetOrder(UserName)?.AmountBuy;

            if (!totalPrice.HasValue)
                return View("Error");

            var payment = await new Payment(totalPrice.Value)
                .PaymentRequest("متن تست موقع خرید",
                    Url.Action(nameof(CheckOutCallback), "Orders", new { amount = totalPrice }, Request.Scheme));

            return payment.Status == 100 ? (orderServices.AddAuthorityToOrder(UserName, payment.Authority) ? (IActionResult)Redirect(payment.Link) : BadRequest("خطا در پرداخت")) : BadRequest($"خطا در پرداخت. کد خطا:{payment.Status}");
        }
        public async Task<IActionResult> CheckOutCallback(int amount, string Authority, string Status)
        {
            if (Status == "NOK") return View("Error", new ErrorViewModel() { RequestId = "khkhkhkhkh" });
            var verification = await new Payment(amount)
                .Verification(Authority);

            ViewResult response;
            if (verification.Status != 100 || !orderServices.PayForOrder(amount, UserName))
            {
                response = View("Error");
            }
            else
            {
                response = View("Success");
            }
            return response;
        }
    }
}
