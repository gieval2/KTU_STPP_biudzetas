using Microsoft.AspNetCore.Identity;

namespace KTUSTPPBiudzetas.Models
{
    public class User : IdentityUser
    {
        public Member Member { get; set; }
    }
}
