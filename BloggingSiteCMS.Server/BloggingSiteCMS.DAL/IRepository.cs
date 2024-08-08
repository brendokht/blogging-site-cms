using System;
using System.Linq.Expressions;

using BloggingSiteCMS.DAL.Enums;

namespace BloggingSiteCMS.DAL
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAll();
        Task<List<T>> GetSome(Expression<Func<T, bool>> match);
        Task<T?> GetOne(Expression<Func<T, bool>> match);
        Task<T> Add(T entity);
        Task<UpdateStatus> Update(T entity);
        Task<int> Delete(string i);
    }
}