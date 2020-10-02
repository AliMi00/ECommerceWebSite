using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceWebSite.Data
{
    public interface IUnitOfWork<TContext> :IDisposable where TContext:DbContext
    {
        void SaveChanges();
        Task<int> SaveChangesAsync();

    }
}
