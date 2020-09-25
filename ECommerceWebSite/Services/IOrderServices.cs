using ECommerceWebSite.Models.DbModels;
using ECommerceWebSite.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Services
{
    public interface IOrderServices
    {
        bool AddAuthorityToOrder(string userName, string authority, int? orderId = null);
        OrderViewModel CustomerOrderDetails(int OrderId, string Username);
        /// <summary>
        /// return list of orders of customer by filter of status and order id we could include the product and order details with it
        /// </summary>
        /// <param name="username"></param>
        /// <param name="orderId"></param>
        /// <param name="status"></param>
        /// <param name="withIncludes"></param>
        /// <returns></returns>
        List<Order> CustomerOrders(string username, int? orderId = null, OrderStatusTypes? status = OrderStatusTypes.Open, bool withIncludes = false);
        /// <summary>
        /// return open order from order of user this will use for return open order 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="orderId"></param>
        /// <param name="withIncludes"></param>
        /// <returns></returns>
        Order GetOrder(string username, int? orderId = null, bool withIncludes = false);
        /// <summary>
        /// add cart to order and delete from cart (mark as delete) or increase the quentity if exist
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        ProductAddToOrderViewModel AddCartToOrder(string Username, int productId, int quantity = 1);
        int ValidateOrders();
        bool PayForOrder(int amount, string userName);
        /// <summary>
        /// add all cart item to order and set delete from cart it use when user submit the order 
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public int AddProductToCart(string UserName);
        /// <summary>
        /// validate otem of cart and return valid cart item count
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        int ValidatingCartBeforAddingToCart(string UserName);
        /// <summary>
        /// canceling order use for errors
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool CancelingOpenOrder(string userName);
    }
}
