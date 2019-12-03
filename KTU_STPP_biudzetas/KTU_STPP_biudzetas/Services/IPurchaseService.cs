using KTU_STPP_biudzetas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KTU_STPP_biudzetas.Services
{
    public interface IPurchaseService
    {
        Task<IEnumerable<Purchase>> GetAllAsync();
        Task<Purchase> GetAsync(int id);
        Task<Purchase> CreateAsync(Purchase user);
        Task<Purchase> UpdateAsync(Purchase user);
        Task DeleteAsync(int id);
    }
}
