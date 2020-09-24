using ECommerceWebSite.Models.DbModels;
using ECommerceWebSite.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Services
{
    public interface ICartServices
    {
        ProductAddToOrderViewModel addToCart(string Username, int productId, int quantity = 1);
        List<CartItem> GetCartList(string userName);
        Customer GetCustomer(string Username);
        Product GetProduct(int productId);
        ProductAddToOrderViewModel addToTempCart(string tempCartId, int productId, int quantity = 1);
        List<TempCartItem> GetTempCartList(string tempCartId);
        Task DeleteTempCart(string tempCartId);
        Task<ResponsViewModel> DeleteCartItem(int ItemId);

    }
}
