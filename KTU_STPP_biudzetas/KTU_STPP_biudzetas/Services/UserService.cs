using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using KTUSTPPBiudzetas.Helpers;
using KTUSTPPBiudzetas.Models;
using KTUSTPPBiudzetas.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace KTUSTPPBiudzetas
{
    public class UserService : IUserService
    {

        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMemberService _memberService;


        public UserService(IOptions<AppSettings> appSettings, IConfiguration configuration, SignInManager<User> signInManager, UserManager<User> userManager, IMemberService memberService)
        {
            //_appSettings = appSettings.Value;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _memberService = memberService;
        }

        public async Task<string> LoginAsync(Member user)
        {
            var login = await _signInManager.PasswordSignInAsync(user.Username, user.Password, false, false);

            string token = string.Empty;
            if(login.Succeeded)
            {
                var webUser = _userManager.Users.SingleOrDefault(x => x.Member.Username == user.Username);
                token = await GenerateJwtToken(webUser);
            }
            return token;
        }

        public async Task<string> RegisterAsync(Member user)
        {
            var newUser = new User
            {
                UserName = user.Username
            };
            var result = await _userManager.CreateAsync(newUser, user.Password);

            var token = string.Empty;
            if (result.Succeeded)
            {
                user.User = newUser;
                await _memberService.CreateAsync(user);

                switch (user.FamilyLevel)
                {
                    case (int)MemberLevel.FamilyHead:
                        await _userManager.AddClaimAsync(newUser, new Claim("FamilyHead", ""));
                        await _userManager.AddClaimAsync(newUser, new Claim("Controller", ""));
                        await _userManager.AddClaimAsync(newUser, new Claim("Member", ""));
                        break;
                    case (int)MemberLevel.Controller:
                        await _userManager.AddClaimAsync(newUser, new Claim("Controller", ""));
                        await _userManager.AddClaimAsync(newUser, new Claim("Member", ""));
                        break;
                    default:
                        await _userManager.AddClaimAsync(newUser, new Claim("Member", ""));
                        break;
                }

                token = await LoginAsync(user);
            }
            return token;
        }

        public async Task<bool> ChangePassword(User user, string userId)
        {
            var newUser = _userManager.Users.SingleOrDefault(x => x.Id == userId);
            var result = await _userManager.ChangePasswordAsync(newUser, newUser.Member.Password, user.Member.Password);

            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            claims.AddRange(await _userManager.GetClaimsAsync(user));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        //public User Authenticate(string username, string password)
        //{
        //    var user = _users.SingleOrDefault(x => x.username == username && x.password == password);

        //    // return null if user not found
        //    if (user == null)
        //        return null;

        //    // authentication successful so generate jwt token
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //            new Claim(ClaimTypes.Name, user.Id.ToString())
        //        }),
        //        Expires = DateTime.UtcNow.AddDays(7),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    user.token = tokenHandler.WriteToken(token);

        //    // remove password before returning
        //    user.password = null;

        //    return user;
        //}
    }
}