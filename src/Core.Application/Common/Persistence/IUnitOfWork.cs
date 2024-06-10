namespace Core.Application.Common.Persistence;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}