using RentC.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.DataAccess.Contracts
{
    public interface IRepo<T> where T : BaseEntity
    {
        IQueryable<T> Collection();
        void Commit();
        void Delete(int id);
        T Find(int id);
        T FirstOrDefault(Func<T, bool> predicate);
        void Insert(T t);
        void Update(T t);
        ModelContext GetContext();
    }
}
