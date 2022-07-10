
namespace DevBlogs.Shared.Kernel.Interfaces;

public interface IReadOnlyRepository<T> : IAggregateRoot
    where T : class
{
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
 
    Task<List<T>> ListAsync(CancellationToken cancellationToken = default);
 
    Task<int> CountAsync(CancellationToken cancellationToken = default);
 
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
}
