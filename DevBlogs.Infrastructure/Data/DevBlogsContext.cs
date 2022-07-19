namespace DevBlogs.Infrastructure.Data;

public class DevBlogsContext : DbContext, IUnitOfWork
{
    public const string DEFAULT_SCHEMA = "blogging";

    private readonly IMediator _mediator;
    public DbSet<Post> Posts => Set<Post>();

    public DevBlogsContext(DbContextOptions<DevBlogsContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch Domain Events collection. 
        await _mediator.DispatchDomainEventsAsync(this);

        // performed through the DbContext will be committed
        await base.SaveChangesAsync(cancellationToken);

        return true;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
     
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new PostEntityTypeConfiguration());
    }
     
    #region Transactions
    private IDbContextTransaction? _currentTransaction;

    public IDbContextTransaction? GetCurrentTransaction()
        => _currentTransaction;

    public bool HasActiveTransaction
        => _currentTransaction != null;

    private void TransactionDispose()
    {
        if (_currentTransaction != null)
        {
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }
    }

    public async Task<IDbContextTransaction?> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null) return null;

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

        return _currentTransaction;
    }

    public async Task<IDbContextTransaction?> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
    {
        if (_currentTransaction is not null) return null;

        _currentTransaction = await Database.BeginTransactionAsync(isolationLevel, cancellationToken);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        if (transaction == null)
        {
            ThrowHelper.ThrowArgumentNullException(nameof(transaction));
        }
        else if (transaction != _currentTransaction)
        {
            ThrowHelper.ThrowInvalidOperationException($"Transaction {transaction.TransactionId} is not current");
        }

        try
        {
            await SaveChangesAsync(cancellationToken);
            if (transaction != null)
            {
                await transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            ThrowHelper.ThrowRollbackTransactionException(transaction?.TransactionId);
        }
        finally
        {
            TransactionDispose();
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
            }
        }
        finally
        {
            TransactionDispose();
        }
    }
    #endregion
}
