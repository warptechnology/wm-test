using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WMTest.Models
{
    public abstract class Repository<T, I> : IRepository<T, I> where T : class
    {
        protected DbSet<T> Repo { set; get; }
        protected Context db { set; get; }


        public void Add(T t)
        {
            Repo.Add(t);
            db.SaveChanges();
        }

        public void Delete(T t)
        {
            Repo.Remove(t);
            db.SaveChanges();
        }

        public T Find(I ID)
        {
            return Repo.Find(ID);
        }

        public T FirstOrDefault(Func<T, bool> predicate)
        {
            return Repo.FirstOrDefault(predicate);
        }

        public IEnumerable<T> Get()
        {
            return Repo.AsNoTracking().ToList();
        }

        public IEnumerable<T> Get(Func<T, bool> predicate)
        {
            return Repo.AsNoTracking().Where(predicate).ToList();
        }

        public void Update(T t)
        {
            db.Entry(t).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
    }
}