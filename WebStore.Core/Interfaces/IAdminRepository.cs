using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Core.Models;

namespace WebStore.Core.Interfaces
{
    public interface IAdminRepository : IRepository<ApplicationUser>
    {
        ApplicationUser GetById(string id);
    }
}
