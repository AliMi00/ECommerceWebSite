using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerceWebSite.Data;
using ECommerceWebSite.Models.DbModels;
using Microsoft.AspNetCore.Http;
using ECommerceWebSite.Models.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using ECommerceWebSite.Services;

namespace ECommerceWebSite.Controllers.Admin
{
    [Authorize(Roles ="Admin")]
    public class AdminProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdminServices adminServices;

        public AdminProductsController(ApplicationDbContext context,IAdminServices adminServices)
        {
            _context = context;
            this.adminServices = adminServices;
        }

        // GET: AdminProducts

        public async Task<IActionResult> Index()
        {
            return View(await adminServices.GetProductAsync());
        }

        // GET: AdminProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Product product = await adminServices.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: AdminProducts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Price,CreationDate,DisableDate,RemoveDate,PictureAddress,Quantity")] Product product ,
            IFormFile picture ,string Tags)
        {
            List<string> tagString = Tags.Split(" ").ToList();
            List<Tag> tags = tagString.Select(x => new Tag(){ tag = x.Trim()}).ToList();

            if (ModelState.IsValid)
            {
                AddProductViewModel respos =  await adminServices.AddProduct(product, picture, null, tags);
                //TODO do this on ajax later to show respons to create action 

                return RedirectToAction(nameof(Create));
            }
            return View(product);
        }


        // GET: AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await adminServices.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Price,CreationDate,DisableDate,RemoveDate,PictureAddress,Quantity")] Product product,
            IFormFile picture, string Tags)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            List<string> tagString = Tags.Split(" ").ToList();
            List<Tag> tags = tagString.Select(x => new Tag() { tag = x.Trim() }).ToList();

            if (ModelState.IsValid)
            {
                try
                {
                    AddProductViewModel respos = await adminServices.EditProduct(product, picture, null, tags);
                    //TODO do this on ajax later to show respons to edit action 

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: AdminProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {


            var product = await adminServices.GetProduct(id);
            var respons = adminServices.DeleteProduct(product);

            //TODO do this on ajax later to show respons to delete action 
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
