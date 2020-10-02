using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ECommerceWebSite.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        #region GET
        TEntity Get(int id);
        IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate, string UserId);
        IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate, string UserId, bool GetDeleted = false);
        IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate, bool GetDeleted = false);
        #endregion

        #region DELETE
        bool Delete(TEntity model);
        bool Delete(int id);
        int DeleteAll(Expression<Func<TEntity, bool>> predicate);
        int DeleteAll(Expression<Func<TEntity, bool>> predicate, string UserId);
        int DeleteAll(Expression<Func<TEntity, bool>> predicate, string UserId, bool GetDeleted = false);
        int DeleteAll(Expression<Func<TEntity, bool>> predicate, bool GetDeleted = false);
        #endregion

        #region CREATE

        TEntity Add(TEntity model);
        TEntity Add(TEntity model, string UserId);
        ICollection<TEntity> AddRange(ICollection<TEntity> model);
        ICollection<TEntity> AddRange(ICollection<TEntity> model, string UserId);
        ICollection<TEntity> AddRange(Expression<Func<TEntity, bool>> predicate);
        ICollection<TEntity> AddRange(Expression<Func<TEntity, bool>> predicate, string UserId);

        #endregion

        #region Update

        TEntity Update(TEntity model);
        TEntity Update(TEntity model, string UserId);
        ICollection<TEntity> UpdateRange(ICollection<TEntity> model);
        ICollection<TEntity> UpdateRange(ICollection<TEntity> model, string UserId);
        ICollection<TEntity> UpdateRange(Expression<Func<TEntity, bool>> predicate);
        ICollection<TEntity> UpdateRange(Expression<Func<TEntity, bool>> predicate, string UserId);

        #endregion


        #region GetASYNC
        Task<TEntity> GetAsync(int id);
        Task<IQueryable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IQueryable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate, string UserId);
        Task<IQueryable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate, string UserId, bool GetDeleted = false);
        Task<IQueryable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate, bool GetDeleted = false);
        #endregion

        #region DeleteASYNC
        Task<bool> DeleteAsync(TEntity model);
        Task<bool> DeleteAsync(int id);
        Task<int> DeleteAllAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> DeleteAllAsync(Expression<Func<TEntity, bool>> predicate, string UserId);
        Task<int> DeleteAllAsync(Expression<Func<TEntity, bool>> predicate, string UserId, bool GetDeleted = false);
        Task<int> DeleteAllAsync(Expression<Func<TEntity, bool>> predicate, bool GetDeleted = false);
        #endregion

        #region CreateASYNC

        Task<TEntity> AddAsync(TEntity model);
        Task<TEntity> AddAsync(TEntity model, string UserId);
        Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> model);
        Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> model, string UserId);
        Task<ICollection<TEntity>> AddRangeAsync(Expression<Func<TEntity, bool>> predicate);
        Task<ICollection<TEntity>> AddRangeAsync(Expression<Func<TEntity, bool>> predicate, string UserId);

        #endregion

        #region UpdateASYNC

        Task<TEntity> UpdateAsync(TEntity model);
        Task<TEntity> UpdateAsync(TEntity model, string UserId);
        Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> model);
        Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> model, string UserId);
        Task<ICollection<TEntity>> UpdateRangeAsync(Expression<Func<TEntity, bool>> predicate);
        Task<ICollection<TEntity>> UpdateRangeAsync(Expression<Func<TEntity, bool>> predicate, string UserId);

        #endregion
    }
}
