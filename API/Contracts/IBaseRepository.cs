using API.Models;

namespace API.Contracts
{
    public interface IBaseRepository<TEntity>
    {
        TEntity? Create(TEntity entity);
        bool Update(TEntity entity);
        bool Delete(Guid guid);
        IEnumerable<TEntity> GetAll();
        TEntity? GetByGuid(Guid guid);
    }
}
