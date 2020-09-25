using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerceWebSite.Data;
using ECommerceWebSite.Models.DbModels;
using ECommerceWebSite.Services;
using Microsoft.AspNetCore.Http;
using ECommerceWebSite.Models.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceWebSite.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("/Admin/{controller}/{action=Index}/{id?}")]
    public class AdminCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdminServices adminServices;

        public AdminCategoriesController(ApplicationDbContext context, IAdminServices adminServices)
        {
            _context = context;
            this.adminServices = adminServices;
        }

        // GET: AdminCategories
        public async Task<IActionResult> Index()
        {
            return View(await adminServices.GetCategoriesAsync());
        }

        // GET: AdminCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await adminServices.GetCategoryAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: AdminCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,CreationDate,DisableDate,RemoveDate,PictureAddress")] Category category, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                AddProductViewModel respons = new AddProductViewModel();
                respons = await adminServices.AddCategory(category, file);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: AdminCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await adminServices.GetCategoryAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: AdminCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,CreationDate,DisableDate,RemoveDate,PictureAddress")] Category category, IFormFile file)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await adminServices.EditCategory(category, file);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!adminServices.CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: AdminCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await adminServices.GetCategoryAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: AdminCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            await adminServices.DeleteCategory(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
