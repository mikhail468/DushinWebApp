using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DushinWebApp.Services
{
    public class DataService<T>:IDataService<T> where T:class
    {
        private MyDbContext _context;
        private DbSet<T> _dBSet;
        public DataService(MyDbContext context)
        {
            _context = context;
            _dBSet = _context.Set<T>();
        }

        public void Create(T entity)
        {
            _dBSet.Add(entity);
            _context.SaveChanges();
        }

        public IEnumerable<T> GetAll()
        {
            return _dBSet.ToList();
        }

        public T GetSingle(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public IEnumerable<T> Query(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public void Update(T entity)
        {
            _dBSet.Update(entity);
            _context.SaveChanges();
        }
    }
}
