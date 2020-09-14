using System;
using System.Collections.Generic;

namespace ECommerceWebSite.Models.DbModels
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int AmountBuy { get; set; }
        public Customer User { get; set; }
        public OrderStatusTypes OrderStatus { get; set; }
#nullable enable
        public string? Authority { get; set; }
#nullable restore
        public DateTime? PaymentDate { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}