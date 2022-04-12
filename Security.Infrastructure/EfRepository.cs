using Ardalis.Specification.EntityFrameworkCore;

using Security.SharedKernel.Interfaces;

namespace Security.Infrastructure;
public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
    public EfRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}