using System;
using System.Collections.Generic;
using System.Linq;
using GeekBurger.StoreCatalog.Infra.Interfaces;

namespace GeekBurger.StoreCatalog.Infra.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IDbContext _context;

        public Repository(IDbContext context)
        {
            this._context = context;
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public void Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }
    }
}
