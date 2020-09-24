using ECommerceWebSite.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerceWebSite.Data
{
    public interface IApplicationDbContext:IDisposable
    {
        DbSet<Customer> Customers { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<OrderDetail> OrderDetails { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<ProductCategory> ProductCategories { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<CartItem> CartItems { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<TempCartItem> TempCartItems { get; set; }

        void Dispose();
        ValueTask DisposeAsync();
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

    }
}
