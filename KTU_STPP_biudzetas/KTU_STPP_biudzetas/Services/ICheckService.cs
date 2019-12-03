using KTU_STPP_biudzetas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KTU_STPP_biudzetas.Services
{
    public interface ICheckService
    {
        Task<IEnumerable<Check>> GetAllAsync();
        Task<Check> GetAsync(int id);
        Task<Check> CreateAsync(Check user);
        Task<Check> UpdateAsync(Check user);
        Task DeleteAsync(int id);
    }
}
