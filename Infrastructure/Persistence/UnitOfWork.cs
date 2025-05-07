using Domain.Contracts;
using Domain.Models;
using Persistence.Data;
using Persistence.Repositories;
using System.Collections.Concurrent;


namespace Persistence
{
    public class UnitOfWork(StoreDbContext context,ConcurrentDictionary<string,object> repositories) : IUnitOfWork
    {
        private readonly StoreDbContext _context = context;
        private readonly ConcurrentDictionary<string, object> _repositories = repositories;

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            return (IGenericRepository<TEntity, TKey>)_repositories.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TEntity, TKey>(_context));
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
