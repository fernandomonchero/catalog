using Catalog.Domain.Interfaces;
using Catalog.Domain.Models;
using Catalog.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Catalog.Infrastructure.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : Entity, new()
    {
        protected readonly CatalogContext Context;
        protected readonly DbSet<T> EntitySet;

        protected Repository(CatalogContext context)
        {
            Context = context;
            EntitySet = Context.Set<T>();
        }

        public async Task<List<T>> All(params Expression<Func<T, object>>[] includes)
        {
            var query = EntitySet.AsQueryable();

            foreach (var include in includes)
                query = query.Include(include);

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<T?> Get(int id, params Expression<Func<T, object>>[] includes)
        {
            var query = EntitySet.AsQueryable();

            foreach (var include in includes)
                query = query.Include(include);

            return await query.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task Add(T entity)
        {
            EntitySet.Add(entity);

            await SaveChanges();
        }

        public async Task<int> SaveChanges()
        {
            return await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}