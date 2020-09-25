using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerceWebSite.Data;
using ECommerceWebSite.Models.DbModels;
using Microsoft.AspNetCore.Authorization;
using ECommerceWebSite.Services;
using ECommerceWebSite.Models.ViewModels;
using ECommerceWebSite.Models.ViewModels.Admin;

namespace ECommerceWebSite.Controllers.Admin
{
    [Authorize(Roles ="Admin")]
    [Route("/Admin/{controller}/{action=Index}/{id?}")]
    public class AdminOrdersController : Controller
    {
        private readonly IAdminServices adminServices;

        public AdminOrdersController(IAdminServices adminServices)
        {
            this.adminServices = adminServices;
        }

        public async Task<IActionResult> Index(OrderStatusTypes? statusType)
        {
            if (statusType.HasValue)
            {
                return View(await adminServices.GetOrdersAsync(statusType));
            }
            return View(await adminServices.GetOrdersAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await adminServices.GetOrderAsync(id,true);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await adminServices.GetOrderAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            EditOrderAdminViewModel respons = new EditOrderAdminViewModel();
            respons.Order = order;
            respons.ResponsViewModel = new ResponsViewModel();
            return View(respons);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderStatusTypes orderStatus)
        {
            EditOrderAdminViewModel respons = new EditOrderAdminViewModel();
            Order order = await adminServices.GetOrderAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            respons.ResponsViewModel = adminServices.UpdateOrderStatus(id, orderStatus);
            respons.Order = order;

            return View(respons);
        }
    }
}
