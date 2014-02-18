using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Core.Interfaces
{
    public interface IRepository<T> 
    {
        IQueryable<T> All();

        T GetById(int id);

        void Update(T entity);

        void Delete(int id);

        void Delete(T entity);

        void Add(T entity);
    }
}
