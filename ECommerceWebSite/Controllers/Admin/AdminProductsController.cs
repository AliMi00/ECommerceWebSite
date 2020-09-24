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
            AdminCreateProductViewModel product = await adminServices.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product.Product);
        }

        // GET: AdminProducts/Create
        public async Task<IActionResult> Create()
        {
            AdminCreateProductViewModel adminProductViewModel = new AdminCreateProductViewModel()
            {
                Product = new Product(),
                categories = await adminServices.GetCategoriesAsync()
            };
            return View(adminProductViewModel);
        }

        // POST: AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Price,CreationDate,DisableDate,RemoveDate,PictureAddress,Quantity")] Product product ,
            IFormFile picture ,string Tags,int category)
        {
            List<string> tagString = Tags.Split(" ").ToList();
            List<Tag> tags = tagString.Select(x => new Tag(){ tag = x.Trim()}).ToList();

            if (ModelState.IsValid)
            {
                AddProductViewModel respos =  await adminServices.AddProduct(product, picture, category, tags);
                //TODO do this on ajax later to show respons to create action 

                return RedirectToAction(nameof(Create));
            }
            return View(product);
        }
        public async Task<IActionResult> AddCategoryToProduct(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            AdminCreateProductViewModel vm = await adminServices.GetProduct(id);
            return View(vm);
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
            IFormFile picture, string Tags,int category)
        {
            List<Tag> tags = new List<Tag>();
            if (id != product.Id)
            {
                return NotFound();
            }
            if(Tags != null)
            {
                List<string> tagString = Tags.Trim().Split(" ").ToList();
                tags = tagString.Select(x => new Tag() { tag = x.Trim() }).ToList();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    AddProductViewModel respos = await adminServices.EditProduct(product, picture, category, tags);
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
            var respons = adminServices.DeleteProduct(product.Product);

            //TODO do this on ajax later to show respons to delete action 
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
