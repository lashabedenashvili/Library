using Library.Data.Domein.Data;
using Library.Data.Domein.Domein;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataBase.GeneralRepository
{
    public class GeneralRepository<TSource> : IGeneralRepository<TSource> where TSource:class,IGlobald
    {
        private readonly Context _context;
        private readonly DbSet<TSource> _entities;
        public GeneralRepository(Context context)
        {
            _context = context;
            _entities = _context.Set<TSource>();
        }
        public async Task<TSource> AddAsync(TSource entity)
        {
            var Tresult = await _entities.AddAsync(entity);
            return Tresult.Entity;
        }

        public async Task<bool> AnyAsync(Expression<Func<TSource, bool>> predicate)
        {
            return await _entities.AsNoTracking().AnyAsync(predicate);
        }
        public async Task Delete(TSource entity)
        {
            _entities.Remove(entity);
        }

        public async Task<TSource> GetAsync(Expression<Func<TSource, bool>> predicate)
        {
            return await _entities.AsNoTracking().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync();
        }

        public async Task Update(TSource entity)
        {
            _entities.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public IQueryable<TSource> Where(Expression<Func<TSource, bool>> predicate)
        {
            return _entities.AsNoTracking().Where(predicate);
        }
        public IQueryable<TSource> Include(params Expression<Func<TSource, object>>[] includes)
        {
            IQueryable<TSource> query = _entities;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query;
        }     
        public IQueryable<TSource> IncludeMultiple(IEnumerable<Expression<Func<TSource, object>>> includes)
        {
            IQueryable<TSource> query = _entities;
            return includes.Aggregate(query, (current, include) => current.Include(include));
        }

        public IQueryable<TSource> AsQuareble()
        {
            return _context.Set<TSource>().AsQueryable();
        }
    }
}
