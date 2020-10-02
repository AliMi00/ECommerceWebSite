using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ECommerceWebSite.Data
{
    public abstract class Repository<TEntity> : IRepository<TEntity> ,IDisposable where TEntity : class
    {


        #region Add
        public TEntity Add(TEntity model)
        {
            throw new NotImplementedException();
        }

        public TEntity Add(TEntity model, string UserId)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> AddAsync(TEntity model)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> AddAsync(TEntity model, string UserId)
        {
            throw new NotImplementedException();
        }

        public ICollection<TEntity> AddRange(ICollection<TEntity> model)
        {
            throw new NotImplementedException();
        }

        public ICollection<TEntity> AddRange(ICollection<TEntity> model, string UserId)
        {
            throw new NotImplementedException();
        }

        public ICollection<TEntity> AddRange(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public ICollection<TEntity> AddRange(Expression<Func<TEntity, bool>> predicate, string UserId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> model)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> model, string UserId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TEntity>> AddRangeAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TEntity>> AddRangeAsync(Expression<Func<TEntity, bool>> predicate, string UserId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Delete

        public bool Delete(TEntity model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public int DeleteAll(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public int DeleteAll(Expression<Func<TEntity, bool>> predicate, string UserId)
        {
            throw new NotImplementedException();
        }

        public int DeleteAll(Expression<Func<TEntity, bool>> predicate, string UserId, bool GetDeleted = false)
        {
            throw new NotImplementedException();
        }

        public int DeleteAll(Expression<Func<TEntity, bool>> predicate, bool GetDeleted = false)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAllAsync(Expression<Func<TEntity, bool>> predicate, string UserId)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAllAsync(Expression<Func<TEntity, bool>> predicate, string UserId, bool GetDeleted = false)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAllAsync(Expression<Func<TEntity, bool>> predicate, bool GetDeleted = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(TEntity model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Get


        public TEntity Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate, string UserId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate, string UserId, bool GetDeleted = false)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate, bool GetDeleted = false)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate, string UserId)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate, string UserId, bool GetDeleted = false)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate, bool GetDeleted = false)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Update
        public TEntity Update(TEntity model)
        {
            throw new NotImplementedException();
        }

        public TEntity Update(TEntity model, string UserId)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> UpdateAsync(TEntity model)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> UpdateAsync(TEntity model, string UserId)
        {
            throw new NotImplementedException();
        }

        public ICollection<TEntity> UpdateRange(ICollection<TEntity> model)
        {
            throw new NotImplementedException();
        }

        public ICollection<TEntity> UpdateRange(ICollection<TEntity> model, string UserId)
        {
            throw new NotImplementedException();
        }

        public ICollection<TEntity> UpdateRange(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public ICollection<TEntity> UpdateRange(Expression<Func<TEntity, bool>> predicate, string UserId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> model)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> model, string UserId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TEntity>> UpdateRangeAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TEntity>> UpdateRangeAsync(Expression<Func<TEntity, bool>> predicate, string UserId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
