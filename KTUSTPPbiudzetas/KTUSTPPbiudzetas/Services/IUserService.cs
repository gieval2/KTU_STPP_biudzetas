using KTUSTPPBiudzetas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KTUSTPPBiudzetas.Services
{
    public interface IUserService
    {
        Task<string> LoginAsync(Member user);
        Task<string> RegisterAsync(Member user);
        Task<bool> ChangePassword(User user, string userId);
    }
}
