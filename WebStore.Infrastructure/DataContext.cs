using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Core.Models;
using WebStore.Infrastructure.Repositories;

namespace WebStore.Infrastructure
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<CategoryLanguage> CategoryLanguages { get; set; }

        public DbSet<CategoryProduct> CategoryProducts { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductLanguage> ProductLanguages { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DataContext()
            : base("dbContext")
        {

        }
    }
}
