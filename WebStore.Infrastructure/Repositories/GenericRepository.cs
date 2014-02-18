using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Core.Interfaces;

namespace WebStore.Infrastructure.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected DbContext Context { get; set; }
        protected DbSet<T> DbSet { get; set; }

        public GenericRepository() :
            this(new DataContext())
        {

        }

        public GenericRepository(DbContext data)
        {
            if (data == null)
            {
                throw new ArgumentException("An instance of DbContext is required to use this repository.", "context");
            }
            this.Context = data;
            this.DbSet = data.Set<T>();
        }

        public IQueryable<T> All()
        {
            return this.DbSet.AsQueryable<T>();
        }

        public T GetById(int id)
        {
            return this.DbSet.Find(id);
        }

        public void Update(T entity)
        {
            DbEntityEntry entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var item = this.DbSet.Find(id);

            if (item != null)
            {
                Delete(item);
            }
        }

        public void Delete(T entity)
        {
            DbEntityEntry entry = this.Context.Entry(entity);

            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                this.DbSet.Attach(entity);
                this.DbSet.Remove(entity);
            }
        }

        public void Add(T entity)
        {
            this.DbSet.Add(entity);
        }

        public virtual void Detach(T entity)
        {
            DbEntityEntry entry = this.Context.Entry(entity);

            entry.State = EntityState.Detached;
        }
    }
}
