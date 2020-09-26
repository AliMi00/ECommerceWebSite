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

        //add product to cart with quantity it check if exist increase the quantity 
        public ProductAddToOrderViewModel addToCart(string Username, int productId, int quantity = 1)
        {
            ProductAddToOrderViewModel respons = new ProductAddToOrderViewModel();
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

        //add cart item to temp cart for un authorize users
        public ProductAddToOrderViewModel addToTempCart(string tempCartId, int productId, int quantity = 1)
        {
            ProductAddToOrderViewModel respons = new ProductAddToOrderViewModel();         
            var product = this.GetProduct(productId);
            if (tempCartId == null)
            {
                respons.Message = "id is wrong";
                respons.Succeed = false;

                return respons;
            }


            if (product == null)
            {
                respons.Message = "Product is wrong";
                respons.Succeed = false;

                return respons;
            }
            var cart = new TempCartItem()
            {
                Product = product,
                AddedTime = DateTime.Now,
                TempCartId = tempCartId,
                Quantity = quantity,
            };
            if (!db.TempCartItems.Any(c => c.TempCartId == tempCartId && c.Product.Id == product.Id && !c.DeletedDate.HasValue ))
            {
                db.TempCartItems.Add(cart);
            }
            else
            {
                db.TempCartItems.Where(c => c.TempCartId == tempCartId && c.Product.Id == product.Id && !c.DeletedDate.HasValue)
                            .FirstOrDefault().Quantity += quantity;
            }
            db.SaveChanges();


            respons.Message = "Product added to Tempcart";
            respons.Succeed = true;

            return respons;

        }

        // return list of itme in cart and totla price of the user 
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
        //get temp cart by tempCartID
        public List<TempCartItem> GetTempCartList(string tempCartId)
        {
            List<TempCartItem> cartItems;
            if (tempCartId == null)
            {
                return cartItems = new List<TempCartItem>();
            }
            else
            {
                cartItems = db.TempCartItems.Where(c => c.TempCartId == tempCartId &&
                             !c.DeletedDate.HasValue &&
                             !c.Product.DisableDate.HasValue &&
                             !c.Product.RemoveDate.HasValue).Include(x => x.Product).ToList();

                return cartItems;
            }

        }

        private Customer GetCustomer(string Username)
        {
            return db.Customers.SingleOrDefault(c => c.UserName == Username);

        }
        public Product GetProduct(int productId)
        {
            return db.Products.SingleOrDefault(p => p.Id == productId && !p.DisableDate.HasValue && !p.RemoveDate.HasValue);
        }
        // Set Delete date for all the cart item of the TempCartId
        public async Task DeleteTempCart(string tempCartId)
        {
            await db.TempCartItems.Where(x => x.TempCartId == tempCartId).ForEachAsync(x => x.DeletedDate = DateTime.Now);
            await db.SaveChangesAsync(true);
        }
        // delete single cart item from Cart by cartItem Id
        public async Task<ResponsViewModel> DeleteCartItem(int ItemId)
        {
            ResponsViewModel respons = new ResponsViewModel();
            if(db.CartItems.Any(x => x.Id == ItemId))
            {
                await db.CartItems.Where(x => x.Id == ItemId).ForEachAsync(x => x.DeletedDate = DateTime.Now);
                await db.SaveChangesAsync(true);

            }
            else
            {
                respons.Succeed = false;
                respons.Message = "Item Not Found";
            }
            return respons;

        }

        public bool CleanUpTempCart()
        {
            try
            {
                db.TempCartItems.RemoveRange(db.TempCartItems.Where(x => x.AddedTime < DateTime.Today.AddDays(-8)));
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
