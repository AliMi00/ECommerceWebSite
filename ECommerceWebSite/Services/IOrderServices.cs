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
        List<Order> CustomerOrders(string username, int? orderId = null, OrderStatusTypes? status = OrderStatusTypes.Open, bool withIncludes = false);
        Customer GetCustomer(string Username);
        Order GetOrder(string username, int? orderId = null, bool withIncludes = false);
        OrderViewModel GetOrderDetails(int OrderId, string Username);
        Product GetProduct(int productId);
        ProductAddToOrderViewModel AddCartToOrder(string Username, int productId, int quantity = 1);
        int ValidateOrders();
        bool PayForOrder(int amount, string userName);
        public int AddProductToCart(string UserName);
    }
}
