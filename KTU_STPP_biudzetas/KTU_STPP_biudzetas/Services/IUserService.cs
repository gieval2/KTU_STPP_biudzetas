using KTU_STPP_biudzetas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KTU_STPP_biudzetas.Services
{
    public interface IUserService
    {
        Task<string> LoginAsync(Member user);
        Task<string> RegisterAsync(Member user);
        Task<bool> ChangePassword(User user, string userId);
    }
}
