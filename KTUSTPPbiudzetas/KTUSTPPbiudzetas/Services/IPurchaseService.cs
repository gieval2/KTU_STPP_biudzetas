using KTUSTPPBiudzetas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KTUSTPPBiudzetas.Services
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
