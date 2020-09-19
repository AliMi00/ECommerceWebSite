using ECommerceWebSite.Data;
using ECommerceWebSite.Models.DbModels;
using ECommerceWebSite.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Services
{
    public class CartServices : ICartServices
    {
        private readonly IApplicationDbContext db;
        public CartServices(IApplicationDbContext db)
        {
            this.db = db;
        }


        public ProductAddResponseViewModel addToCart(string Username, int productId, int quantity = 1)
        {
            ProductAddResponseViewModel respons = new ProductAddResponseViewModel();
            var customer = this.GetCustomer(Username);
            var product = this.GetProduct(productId);
            if (customer == null)
            {
                respons.Message = "Customer is wrong";
                respons.Succeed = false;

                return respons;
            }


            if (product == null)
            {
                respons.Message = "Product is wrong";
                respons.Succeed = false;

                return respons;
            }
            var cart = new CartItem()
            {
                Product = product,
                AddedTime = DateTime.Now,
                Customer = customer,
                Quantity = quantity,
            };
            if(!db.CartItems.Any(c => c.Customer.Id == customer.Id && c.Product.Id == product.Id && !c.RemoveDate.HasValue && !c.DeletedDate.HasValue))
            {
                db.CartItems.Add(cart);
            }
            else
            {
                db.CartItems.Where(c => c.Customer.Id == customer.Id && c.Product.Id == product.Id && !c.RemoveDate.HasValue && !c.DeletedDate.HasValue)
                            .FirstOrDefault().Quantity += quantity;
            }
            db.SaveChanges();


            respons.Message = "Product added to cart";
            respons.Succeed = true;

            return respons;

        }
        /// <summary>
        /// return list of itme in cart and totla price of the user 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<CartItem> GetCartList(string userName)
        {
            List<CartItem> cartItems;
            Customer customer = this.GetCustomer(userName);
            if (customer == null)
            {
                return cartItems =new List<CartItem>();
            }
            else
            {
                cartItems =db.CartItems.Where(c => c.Customer == customer &&
                            !c.DeletedDate.HasValue &&
                            !c.RemoveDate.HasValue &&
                            !c.Product.DisableDate.HasValue &&
                            !c.Product.RemoveDate.HasValue).Include(x => x.Product).ToList();

                return cartItems;
            }

        }


        public Customer GetCustomer(string Username)
        {
            return db.Customers.SingleOrDefault(c => c.UserName == Username);

        }
        public Product GetProduct(int productId)
        {
            return db.Products.SingleOrDefault(p => p.Id == productId && !p.DisableDate.HasValue && !p.RemoveDate.HasValue);
        }
    }
}
