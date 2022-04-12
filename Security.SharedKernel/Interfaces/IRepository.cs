using Ardalis.Specification;

namespace Security.SharedKernel.Interfaces;
public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
}

