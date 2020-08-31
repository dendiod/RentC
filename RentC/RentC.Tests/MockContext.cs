using RentC.DataAccess;
using RentC.DataAccess.Contracts;
using RentC.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Tests
{
    public class MockContext<T> : IRepo<T> where T : BaseEntity
    {
        private List<T> items;

        public MockContext()
        {
            items = new List<T>();
        }

        public IQueryable<T> AsQuerable()
        {
            return items.AsQueryable();
        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }

        public void Commit()
        {
            return;
        }

        public void Delete(int id)
        {
            T tToDelete = items.Find(i => i.Id == id);

            if (tToDelete != null)
            {
                items.Remove(tToDelete);
            }
            throw new Exception(" Not found");
        }

        public T Find(int id)
        {
            T t = items.Find(i => i.Id == id);
            if (t != null)
            {
                return t;
            }
            throw new Exception(" Not found");
        }

        public T FirstOrDefault(Func<T, bool> predicate)
        {
            T t = items.FirstOrDefault(predicate);
            if (t != null)
            {
                return t;
            }
            throw new Exception(" Not found");
        }

        public ModelContext GetContext()
        {
            throw new NotImplementedException();
        }

        public void Insert(T t)
        {
            items.Add(t);
        }

        public void Update(T t)
        {
            T tToUpdate = items.Find(i => i.Id == t.Id);

            if (tToUpdate != null)
            {
                tToUpdate = t;
                return;
            }
            throw new Exception(" Not found");
        }

        public IEnumerable<T> Where(Func<T, bool> predicate)
        {
            return items.Where(predicate);
        }
    }
}
