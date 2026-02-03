using System.Collections;
using MedLink.Application.Interfaces.Persistence;
using MedLink.Domain.Common;
using MedLink.Infrastructure.Persistence.Context;
using MedLink.Infrastructure.Persistence.Repositories;

namespace MedLink.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _dbContext;
        private Hashtable _repositories;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            if (_repositories is null)
                _repositories = new Hashtable();
            var type = typeof(T).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<T>(_dbContext);
                _repositories.Add(type, repository);

            }

            return _repositories[type] as GenericRepository<T>;

        }
        public async Task<int> Complete()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }


    }
}

