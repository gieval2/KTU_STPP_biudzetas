using KTUSTPPBiudzetas.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KTUSTPPBiudzetas.Repositories
{
    public class CheckRepository : Repository<Check>, ICheckRepository
    {
        public CheckRepository(BudgetContext context) : base(context)
        {

        }

        public async Task<IQueryable<Check>> GetCheckByIdAsync(int id)
        {
            //return await Task.FromResult(DbSet.Where(x => x.EmployeeId == employeeId));
            return await Task.FromResult(DbSet.Where(c => c.Id == id));
            //return await Task.FromResult(DbSet.Include(b => b.Purchases).Include(a => a.Member).Where(c => c.Id == id));
            //return await Task.FromResult(DbSet.Include(b => b.Purchases).Include(a => a.Member).FirstOrDefaultAsync(c => c.Id == id));
            //return base.Db.Checks.Include(b => b.Purchases).Include(a => a.Member).FirstOrDefaultAsync(c => c.Id == id);
            //return base.GetByIdAsync(id);
        }

        //public override Task<Check> AddAsync(Check obj)
        //{
        //    Db.Members.Update(b => b.Checks).FirstOrDefaultAsync(c => c.Id == id);
        //    return base.AddAsync(obj);
        //}
    }
}
