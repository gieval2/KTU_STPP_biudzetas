using KTU_STPP_biudzetas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KTU_STPP_biudzetas.Services
{
    public interface IMemberService
    {
        Task<IEnumerable<Member>> GetAllAsync();
        Task<Member> GetAsync(int id);
        Task<Member> CreateAsync(Member user);
        Task<Member> UpdateAsync(Member user);
        Task DeleteAsync(int id);
        Task<IEnumerable<Member>> ResetLimits();
        Task<Member> SetLimit(int memberId, double newLimit);
        Task<Member> ConfirmLimit(int memberId);
    }
}
