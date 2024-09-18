using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesShopApp.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context) 
        {
            _context = context;
        }

        public IQueryable<T> GetAll() 
        {
            return _context.Set<T>();
        }

        public T? GetById(int? id) 
        {
            return _context.Set<T>().Find(id);
        }

        public void Create(T entity) 
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }
        public void Update(T entity) 
        {
            
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }
        public void Delete(int id) 
        {
            var entity = GetById(id);
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity not found");
            }
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }
    }
}
