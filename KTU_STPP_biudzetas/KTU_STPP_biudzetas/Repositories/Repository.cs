using KTU_STPP_biudzetas.Models;
using KTU_STPP_biudzetas.Models.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KTU_STPP_biudzetas.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly BudgetContext Db;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(BudgetContext context)
        {
            Db = context;
            DbSet = Db.Set<TEntity>();
        }

        public virtual async Task<TEntity> AddAsync(TEntity obj)
        {
            obj.LastUpdated = DateTime.UtcNow;
            await DbSet.AddAsync(obj);
            await SaveChangesAsync();
            return obj;
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await Task.FromResult(DbSet.AsNoTracking().SingleOrDefault(x => x.Id == id));
        }

        public virtual async Task<IQueryable<TEntity>> GetAllAsync()
        {
            return await Task.FromResult(DbSet);
        }

        public virtual async Task UpdateAsync(TEntity obj)
        {
            obj.LastUpdated = DateTime.UtcNow;
            DbSet.Update(obj);
            await SaveChangesAsync();
        }

        public virtual async Task RemoveAsync(int id)
        {
            DbSet.Remove(await DbSet.FindAsync(id));
            await SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
