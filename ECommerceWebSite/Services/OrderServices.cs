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
        public OrderServices(IApplicationDbContext db )
        {
            this.db = db;
        }


        //add cart to order and delete from cart (mark as delete) or increase the quentity if exist 
        public ProductAddToOrderViewModel AddCartToOrder(string Username, int productId, int quantity = 1)
        {
            var responseModel = new ProductAddToOrderViewModel();

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
            if(product.Quantity <= quantity)
            {
                responseModel.Message = "Product is not available";
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
                UnitPriceBuy = product.Price,
                Tax = 0,
                Discount = 0,
                Quantity = quantity,
                CreationDate = DateTime.Now
            };
            if(db.OrderDetails.Any(x=> x.Order.Id == order.Id && x.Product.Id == product.Id ))
            {
                int deletedQuantity = db.OrderDetails.Where(x => x.Order.Id == order.Id && x.Product.Id == product.Id && !x.DeleteDate.HasValue).FirstOrDefault().Quantity;
                db.OrderDetails.Where(x => x.Order.Id == order.Id && x.Product.Id == product.Id && !x.DeleteDate.HasValue).FirstOrDefault().DeleteDate = DateTime.Now;
                db.Products.Where(x => x.Id == product.Id).FirstOrDefault().Quantity += deletedQuantity;
            }
            db.OrderDetails.Add(orderDetail);
            db.Products.Where(x => x.Id == product.Id).FirstOrDefault().Quantity -= quantity;

            order.AmountBuy += orderDetail.UnitPriceBuy * orderDetail.Quantity;
            db.SaveChanges();


            responseModel.Message = "Product added to order";
            responseModel.Succeed = true;

            return responseModel;


        }

        //return order include orderdetails list 
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
        //for checkout part add authority to order 
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
        //return open order from order of user this will use for return open order 
        public Order GetOrder(string username, int? orderId = null, bool withIncludes = false)
        {
            OrderStatusTypes status = OrderStatusTypes.Open;
            Order order = null;
            IQueryable<Order> orders = db.Orders.Where(x => x.Customer.UserName == username && x.OrderStatus == status);
            if (orderId.HasValue)
            {
                orders = orders.Where(x => x.Id == orderId.Value);
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
        //return customer by username 
        private Customer GetCustomer(string Username)
        {
            return db.Customers.SingleOrDefault(c => c.UserName == Username);

        }
        //return product by id
        private Product GetProduct(int productId)
        {
            return db.Products.SingleOrDefault(p => p.Id == productId && !p.DisableDate.HasValue && !p.RemoveDate.HasValue);
        }
        //return list of orders of customer by filter of status and order id we could include the product and order details with it 
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
                orders = orders.Where(x => x.OrderStatus == status.Value);
            }
            if (withIncludes)
            {
                orders = orders.Include(x => x.OrderDetails)
                               .ThenInclude(x => x.Product);
            }
            order = orders.ToList();
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
        //add all cart item to order and set delete from cart it use when user submit the order 
        public int AddProductToCart(string UserName)
        {
            Customer customer = GetCustomer(UserName);

            if (customer == null)
                return 0;
            List<CartItem> cartItems = GetCartList(customer.UserName);
            List<ProductAddToOrderViewModel> respose = new List<ProductAddToOrderViewModel>();
            foreach(CartItem item in cartItems)
            {
                ProductAddToOrderViewModel res = AddCartToOrder(item.Customer.UserName, item.Product.Id, item.Quantity);
                db.CartItems.Where(x => x.Id == item.Id).SingleOrDefault().RemoveDate = DateTime.Now;

                respose.Add(res);
            }

            db.SaveChanges();
            return respose.Count(x => x.Succeed);
        }
        //validate otem of cart and return valid cart item count
        public int ValidatingCartBeforAddingToCart(string UserName)
        {
            Customer customer = GetCustomer(UserName);

            if (customer == null)
                return 0;
            List<CartItem> cartItems = GetCartList(customer.UserName);
            List<ProductAddToOrderViewModel> respose = new List<ProductAddToOrderViewModel>();
            foreach (CartItem item in cartItems)
            {
                ProductAddToOrderViewModel res = ValidateCartItem(item.Customer.UserName, item.Product.Id, item.Quantity);
                if(res.Succeed == false)
                {
                    db.CartItems.Where(x => x.Id == item.Id).SingleOrDefault().RemoveDate = DateTime.Now;
                }
                respose.Add(res);
            }

            db.SaveChanges();
            return respose.Count(x => x.Succeed);
        }
        private ProductAddToOrderViewModel ValidateCartItem(string Username, int productId, int quantity = 1)
        {
            var responseModel = new ProductAddToOrderViewModel();

            var customer = this.GetCustomer(Username);
            var product = this.GetProduct(productId);
            if (product == null)
            {
                responseModel.Message = "product Invalid";
                responseModel.Succeed = false;

                return responseModel;
            }
            if (product.Quantity <= quantity)
            {
                responseModel.Message = "Product is not available";
                responseModel.Succeed = false;

                return responseModel;
            }

            responseModel.Message = "item is Valid";
            responseModel.Succeed = true;

            return responseModel;
        }
        //canceling order use for errors
        public bool CancelingOpenOrder(string userName)
        {
            Order order = GetOrder(userName,null,true);
            order.OrderStatus = OrderStatusTypes.Canseled;
                        

            foreach (OrderDetail od in order.OrderDetails)
            {
                var pro = od.Product;
                pro.Quantity += od.Quantity;
                try
                {
                    db.Products.Update(pro);
                }
                catch
                {
                    return false;
                }
            }
            try
            {
                db.Orders.Update(order);
                db.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        // return list of itme in cart and totla price of the user 
        public List<CartItem> GetCartList(string userName)
        {
            List<CartItem> cartItems;
            Customer customer = this.GetCustomer(userName);
            if (customer == null)
            {
                return cartItems = new List<CartItem>();
            }
            else
            {
                cartItems = db.CartItems.Where(c => c.Customer == customer &&
                             !c.DeletedDate.HasValue &&
                             !c.RemoveDate.HasValue &&
                             !c.Product.DisableDate.HasValue &&
                             !c.Product.RemoveDate.HasValue).Include(x => x.Product).ToList();

                return cartItems;
            }

        }




    }
}
