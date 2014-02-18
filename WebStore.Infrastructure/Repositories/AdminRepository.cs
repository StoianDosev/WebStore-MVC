using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Core.Interfaces;
using WebStore.Core.Models;

namespace WebStore.Infrastructure.Repositories
{
    public class AdminRepository : GenericRepository<ApplicationUser>, IAdminRepository
    {
        public AdminRepository(DataContext data)
            : base(data)
        {

        }

        public ApplicationUser GetById(string id)
        {
            return this.DbSet.Find(id);
        }
    }
}
