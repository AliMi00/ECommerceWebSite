using System;

namespace ECommerceWebSite.Models.DbModels
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
        //Product Price When Product was Sold
        public int UnitPriceBuy { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public double Tax { get; set; }
        //the time that product added to order
        public DateTime CreationDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}