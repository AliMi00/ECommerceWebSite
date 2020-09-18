using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Models.DbModels
{
    public class CartItem
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public DateTime? OrderedDate { get; set; }
        public DateTime AddedTime { get; set; }
        public DateTime? RemoveDate { get; set; }
        public DateTime? DeletedDate { get; set; }



    }
}
