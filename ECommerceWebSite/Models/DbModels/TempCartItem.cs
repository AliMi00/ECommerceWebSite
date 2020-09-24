using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Models.DbModels
{
    public class TempCartItem
    {
        public int Id { get; set; }
        public string TempCartId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedTime { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
