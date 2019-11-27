using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KTU_STPP_biudzetas.Repositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity obj);
        Task<TEntity> GetByIdAsync(int id);
        Task<IQueryable<TEntity>> GetAllAsync();
        Task UpdateAsync(TEntity obj);
        Task RemoveAsync(int id);
        Task<int> SaveChangesAsync();
    }
}
