using KTUSTPPBiudzetas.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KTUSTPPBiudzetas.Repositories
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        public MemberRepository(BudgetContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Member>> ResetLimits()
        {
            Db.Database.ExecuteSqlCommand("UPDATE Members SET Limit = 0, LimitState = 0");
            return await Task.FromResult(DbSet.ToList());
        }

        public async Task<Member> ConfirmLimit(int memberId)
        {
            Db.Database.ExecuteSqlCommand($"UPDATE Members SET LimitState = 2 WHERE id = {memberId}");
            return await GetByIdAsync(memberId);
        }

        public async Task<Member> SetLimit(int memberId, double newLimit)
        {
            Db.Database.ExecuteSqlCommand($"UPDATE Members SET Limit = {newLimit}, LimitState = 1 WHERE id = {memberId}");
            return await GetByIdAsync(memberId);
        }
    }
}
