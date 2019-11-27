using KTU_STPP_biudzetas.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KTU_STPP_biudzetas.Repositories
{
    public interface IMemberRepository : IRepository<Member>
    {
        Task<IEnumerable<Member>> ResetLimits();
        Task<Member> ConfirmLimit(int memberId);
        Task<Member> SetLimit(int memberId, double newLimit);
    }
}
