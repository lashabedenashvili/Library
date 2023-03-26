using Library.Data.Domein.Data;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataBase.GeneralRepository
{
    public interface IGeneralRepository<TSource> where TSource : class,IGlobald
    {
        
        IQueryable<TSource> AsQuareble();

        
        Task<TSource> AddAsync(TSource entity);
        Task<bool> AnyAsync(Expression<Func<TSource, bool>> predicate);
        Task Update(TSource entity);
        Task Delete(TSource entity);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<TSource> GetAsync(Expression<Func<TSource, bool>> predicate);
        IQueryable<TSource> Where(Expression<Func<TSource, bool>> predicate);
    }
}
