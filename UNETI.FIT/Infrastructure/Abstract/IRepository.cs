using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Web;

namespace UNETI.FIT.Infrastructure.Abstract
{
    public interface IRepository<T> where T : class
    {
        void Attach(T entity);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Save();
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void DeleteMany(Expression<Func<T, bool>> where);
        T GetByID(int ID);
        T GetByID(string ID);
        T Get(Expression<Func<T, bool>> where);
        IQueryable<T> GetAll();
        IQueryable<T> GetMany(Expression<Func<T, bool>> where);
    }
}