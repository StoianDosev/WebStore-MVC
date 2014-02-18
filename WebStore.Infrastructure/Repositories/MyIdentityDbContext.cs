

using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Core.Models;

namespace WebStore.Infrastructure.Repositories
{
    public class MyIdentityDbContext<TUser> : IdentityDbContext<TUser> where TUser : Microsoft.AspNet.Identity.EntityFramework.IdentityUser
    {
        public MyIdentityDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }
        public virtual IDbSet<TUser> Admins { get; set; }
    }
}
