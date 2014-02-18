using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Core.Interfaces;
using WebStore.Core.Models;

namespace WebStore.Infrastructure.Repositories
{
    public class UowData :IUowData
    {
        private readonly DbContext context;

        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();


        public UowData()
            : this(new DataContext())
        {

        }

        public UowData(DbContext context)
        {
            this.context = context;
        }

        public IRepository<Category> Categories
        {
            get { return this.GetRepository<Category>(); }
        }

        public IRepository<CategoryLanguage> CategoryLanguages
        {
            get { return this.GetRepository<CategoryLanguage>(); }
        }

        public IRepository<Language> Languages
        {
            get { return this.GetRepository<Language>(); }
        }

        public IRepository<Comment> Comments
        {
            get { return this.GetRepository<Comment>(); }
        }

        public IRepository<Product> Products
        {
            get { return this.GetRepository<Product>(); }
        }

        public IRepository<ProductLanguage> ProductLanguages
        {
            get { return this.GetRepository<ProductLanguage>(); }
        }

        public IRepository<Order> Orders
        {
            get { return this.GetRepository<Order>(); }
        }

        public IRepository<OrderDetail> OrderDetails
        {
            get { return this.GetRepository<OrderDetail>(); }
        }

        public IRepository<CategoryProduct> CategoryProducts
        {
            get { return this.GetRepository<CategoryProduct>(); }
        }

        public IRepository<ApplicationUser> Admins
        {
            get { return this.GetRepository<ApplicationUser>(); }
        }

        public IRepository<Customer> Customers
        {
            get { return this.GetRepository<Customer>(); }
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericRepository<T>);

                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}
