using KTUSTPPBiudzetas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KTUSTPPBiudzetas.Repositories
{
    public class PurchaseRepository : Repository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(BudgetContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Purchase>> GetByCheckIdAsync(int id)
        {
            return await Task.FromResult(DbSet.Where(c => c.CheckId == id));
        }
    }
}
