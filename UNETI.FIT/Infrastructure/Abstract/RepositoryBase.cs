using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data;
using System.Linq.Expressions;
using UNETI.FIT.Infrastructure.Concrete;

namespace UNETI.FIT.Infrastructure.Abstract
{
    public abstract class RepositoryBase<T> where T : class
    {
        private ApplicationDBContext context;
        protected readonly IDbSet<T> dbset;
        protected RepositoryBase()
        {
            context = new ApplicationDBContext();
            dbset = context.Set<T>();
        }

        public virtual void Attach(T entity)
        {
            dbset.Attach(entity);
        }

        public virtual void Add(T entity)
        {
            dbset.Add(entity);
            context.SaveChanges();
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
                Add(entity);
        }

        public virtual void Save()
        {
            context.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            var entry = context.Entry<T>(entity);
            if (entry.State == EntityState.Detached)
            {
                var set = context.Set<T>();
                var pkey = dbset.Create().GetType().GetProperty("ID").GetValue(entity);
                T attachedEntity = set.Find(pkey);
                if (attachedEntity == null)
                {
                    entry.State = EntityState.Modified;
                }
                else
                {
                    var attachedEntry = context.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entity);
                }
                context.SaveChanges();
            }
            else if (entry.State == EntityState.Modified)
            {
                context.SaveChanges();
            }
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
                Update(entity);
        }

        public virtual void Delete(T entity)
        {
            //var entry = context.Entry<T>(entity);
            //if (entry.State == EntityState.Detached)
            //{
            //    dbset.Attach(entity);
            //    dbset.Remove(entity);
            //}
            //else
            //{
            //    dbset.Remove(entity);
            //}
            dbset.Remove(entity);
            context.SaveChanges();
        }

        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
                Delete(entity);
        }

        public virtual void DeleteMany(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbset.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                dbset.Remove(obj);
            context.SaveChanges();
        }

        public virtual T GetByID(int id)
        {
            return dbset.Find(id);
        }

        public virtual T GetByID(string id)
        {
            return dbset.Find(id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return dbset;
        }

        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where).FirstOrDefault();
        }

        public virtual IQueryable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where);
        }
    }
}