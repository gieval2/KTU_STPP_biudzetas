using Microsoft.AspNetCore.Identity;

namespace KTUSTPPBiudzetas.Models
{
    public class User : IdentityUser
    {
        [PersonalData]
        public Member Member { get; set; }
        //public override string UserName { get => Member.Username; set => Member.Username = value; }
        //public override string NormalizedUserName { get => base.NormalizedUserName; set => base.NormalizedUserName = value; }
        //public override string Email { get => Member.Email; set => Member.Email = value;  }
        //public override string NormalizedEmail { get => base.NormalizedEmail; set => base.NormalizedEmail = value; }
        //public override bool EmailConfirmed { get => base.EmailConfirmed; set => base.EmailConfirmed = value; }
        //public override string PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }
        //public override string SecurityStamp { get => base.SecurityStamp; set => base.SecurityStamp = value; }
        //public override string ConcurrencyStamp { get => base.ConcurrencyStamp; set => base.ConcurrencyStamp = value; }
        //public override bool LockoutEnabled { get => base.LockoutEnabled; set => base.LockoutEnabled = value; }
    }
}
