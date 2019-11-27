using Microsoft.AspNetCore.Identity;

namespace KTU_STPP_biudzetas.Models
{
    public class User : IdentityUser
    {
        public Member Member { get; set; }
    }
}
