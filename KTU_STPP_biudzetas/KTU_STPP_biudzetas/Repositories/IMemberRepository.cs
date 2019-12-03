using KTUSTPPBiudzetas.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KTUSTPPBiudzetas.Repositories
{
    public interface IMemberRepository : IRepository<Member>
    {
        Task<IEnumerable<Member>> ResetLimits();
        Task<Member> ConfirmLimit(int memberId);
        Task<Member> SetLimit(int memberId, double newLimit);
    }
}
