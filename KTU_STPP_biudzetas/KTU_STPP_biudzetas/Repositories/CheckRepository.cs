using KTU_STPP_biudzetas.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KTU_STPP_biudzetas.Repositories
{
    public class CheckRepository : Repository<Check>, ICheckRepository
    {
        public CheckRepository(BudgetContext context) : base(context)
        {

        }

        public override Task<Check> GetByIdAsync(int id)
        {
            return Db.Checks.Include(b => b.Purchases).FirstOrDefaultAsync(c => c.Id == id);
            //return base.GetByIdAsync(id);
        }
    }
}
