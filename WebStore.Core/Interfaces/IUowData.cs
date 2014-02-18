using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Core.Models;

namespace WebStore.Core.Interfaces
{
    public interface IUowData : IDisposable
    {
        IRepository<Category> Categories { get; }

        IRepository<CategoryLanguage> CategoryLanguages { get; }

        IRepository<Comment> Comments { get; }

        IRepository<Language> Languages { get; }

        IRepository<Order> Orders { get; }

        IRepository<OrderDetail> OrderDetails { get; }

        IRepository<Product> Products { get; }

        IRepository<ProductLanguage> ProductLanguages { get; }

        IRepository<ApplicationUser> Admins { get; }

        IRepository<CategoryProduct> CategoryProducts { get; }

        IRepository<Customer> Customers { get; }

        void SaveChanges();
    }
}
