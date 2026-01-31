using MedLink.Domain.Common;

namespace MedLink_Application.Interfaces.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> Complete();
    }
}
