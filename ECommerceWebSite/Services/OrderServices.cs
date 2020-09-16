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
    public class OrderServices:IOrderServices
    {
        private readonly IApplicationDbContext db;
        public OrderServices(IApplicationDbContext _db)
        {
            db = _db;
        }

        public ProductAddResponseViewModel SaveOrder(string Username, int productId, int quantity = 1)
        {
            var responseModel = new ProductAddResponseViewModel();

            var customer = this.GetCustomer(Username);
            var product = this.GetProduct(productId);
            Order order;
            if (customer == null)
            {
                responseModel.Message = "Customer is wrong";
                responseModel.Succeed = false;

                return responseModel;
            }


            if (product == null)
            {
                responseModel.Message = "Product is wrong";
                responseModel.Succeed = false;

                return responseModel;
            }

            order = GetOrder(Username);
            if (order == null)
            {
                order = new Order()
                {
                    OrderDate = DateTime.Now,
                    Customer = customer,

                };
                db.Orders.Add(order);
            }

            var orderDetail = new OrderDetail()
            {
                Order = order,
                Product = product,
                UnitPriceBuy = quantity * product.Price,
                Tax = 0,
                Discount = 0,
                Quantity = quantity,
                CreationDate = DateTime.Now
            };
            db.OrderDetails.Add(orderDetail);
            order.AmountBuy += orderDetail.UnitPriceBuy;
            db.SaveChanges();


            responseModel.Message = "Product added to order";
            responseModel.Succeed = true;
            responseModel.orderId = order.Id;

            return responseModel;


        }
        //return order include orderdetails list  
        public OrderViewModel GetOrderDetails(int OrderId, string Username)
        {
            OrderViewModel responseModel = new OrderViewModel();


            var order = GetOrder(Username, OrderId, withIncludes: true);
            if (order == null)
                return responseModel;


            var details =
                order.OrderDetails
                     .Where(x => !x.DeleteDate.HasValue)
                     .Select(x => new OrderDetailViewModel()
                     {
                         Id = x.Id,
                         ImageAddress = x.Product.PictureAddress,
                         Price = x.UnitPriceBuy,
                         Title = x.Product.Title
                     });
            responseModel.Details = details.ToList();
            responseModel.TotalPrice = details.Sum(x => x.Price);
            responseModel.Id = OrderId;

            return responseModel;



        }

        //same as getOrderDetails but in difrent aproch 
        public OrderViewModel CustomerOrderDetails(int OrderId, string Username)
        {
            OrderViewModel responseModel = new OrderViewModel();

            //var order = CustomerOrders(Username, OrderId,0,true).Single<Order>();
            //if (order == null)
            //    return responseModel;
            //responseModel.TotalPrice = order.OrderDetails.Sum(x => x.UnitPriceBuy);
            //return responseModel;

            var order = CustomerOrders(Username).Where(x => x.Id == OrderId).Single<Order>();
            if (order == null)
                return responseModel;

            var details =
                order.OrderDetails
                     .Where(x => !x.DeleteDate.HasValue)
                     .Select(x => new OrderDetailViewModel()
                     {
                         Id = x.Id,
                         ImageAddress = x.Product.PictureAddress,
                         Price = x.UnitPriceBuy,
                         Title = x.Product.Title
                     });
            responseModel.Details = details.ToList();
            responseModel.TotalPrice = details.Sum(x => x.Price);
            responseModel.Id = OrderId;

            return responseModel;

        }



        //this will be use in hosted service to validate orders
        public int ValidateOrders()
        {
            var orders = db.Orders.Include(o => o.OrderDetails).ToList();
            //In OrderStatusType Enum, there is a 1000 step distance between temp values and
            //real values, so they can simply turn into each other
            //for more info,
            ///<seealso cref="OrderStatusTypes"/>
            const int Status_Temp_Distance = 1000;
            orders.AsParallel()
                .ForAll(o =>
                {
                    var detailsPriceSum = o.OrderDetails.Where(od => !od.DeleteDate.HasValue).Sum(odp => odp.UnitPriceBuy);
                    if (o.AmountBuy != detailsPriceSum)
                    {
                        o.AmountBuy = detailsPriceSum;
                        o.OrderStatus = o.OrderStatus + Status_Temp_Distance;

                    }
                });
            var totalChanged = 0;
            orders.ForEach(o =>
            {
                if ((int)o.OrderStatus >= Status_Temp_Distance)
                {
                    totalChanged++;
                    o.OrderStatus = o.OrderStatus - Status_Temp_Distance;
                }
            });

            db.SaveChanges();
            return totalChanged;

        }

        public bool AddAuthorityToOrder(string username, string authority, int? orderId = null)
        {
            var order = GetOrder(username, orderId);
            if (order == null)
            {
                return false;
            }
            order.Authority = authority;
            db.SaveChanges();
            return true;
        }

        public Order GetOrder(string username, int? orderId = null, OrderStatusTypes? status = OrderStatusTypes.Open, bool withIncludes = false)
        {
            Order order = null;
            IQueryable<Order> orders = db.Orders.Where(x => x.Customer.UserName == username);
            if (orderId.HasValue)
            {
                orders = orders.Where(x => x.Id == orderId.Value);
            }
            if (status.HasValue)
            {
                orders = orders.Where(x => x.OrderStatus == status.Value);
            }
            if (withIncludes)
            {
                orders = orders.Include(x => x.OrderDetails)
                               .ThenInclude(x => x.Product);
            }
            var ordersList = orders.ToList();
            switch (ordersList.Count())
            {
                case 0:
                    return order;
                case 1:
                    return orders.SingleOrDefault();
                default:
                    foreach (var item in orders)
                    {
                        item.OrderStatus = OrderStatusTypes.NeedReview;
                    }
                    db.SaveChanges();
                    return order;
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
        public List<Order> CustomerOrders(string username, int? orderId = null, OrderStatusTypes? status = OrderStatusTypes.Open, bool withIncludes = false)
        {
            List<Order> order = null;
            IQueryable<Order> orders = db.Orders.Where(x => x.Customer.UserName == username);
            if (orderId.HasValue)
            {
                orders = orders.Where(x => x.Id == orderId.Value);
            }
            if (status.HasValue)
            {
                orders = orders.Where(x => x.OrderStatus != status.Value);
            }
            if (withIncludes)
            {
                orders = orders.Include(x => x.OrderDetails)
                               .ThenInclude(x => x.Product);
            }
            return order;
        }
        public bool PayForOrder(int Amount, string Username)
        {

            var Order = GetOrder(Username);
            if (Amount >= Order.AmountBuy)
            {
                Order.OrderStatus = OrderStatusTypes.Boxing;
                Order.PaymentDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            else
                return false;


        }

        //

    }
}
