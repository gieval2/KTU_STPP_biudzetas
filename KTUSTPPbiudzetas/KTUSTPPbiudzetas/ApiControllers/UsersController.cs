using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KTUSTPPBiudzetas.Models;
using KTUSTPPBiudzetas.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace KTUSTPPBiudzetas.ApiControllers
{
    //[Authorize(Policy = "RequireClaimMember")]
    [Route("Budget/Users")]
    [ApiController]
    public class UsersController : Controller
    {
        //private readonly BudgetContext _context;
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Member user)
        {
            try
            {
                var result = await _userService.LoginAsync(user);
                if (result.Length == 0)
                {
                    return BadRequest("Failed to login!");
                }
                return Ok(new string($"Bearer {result.Trim()}"));
                //return Ok(new JsonResult(new { a = "Bearer ", result }));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(Member user)
        {
            try
            {
                var result = await _userService.RegisterAsync(user);
                if (result.Length == 0)
                {
                    return BadRequest("Failed to register!");
                }
                return Ok(new string($"Bearer {result.Trim()}"));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] User user)
        {
            try
            {
                var userId = User.FindFirst(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value;
                var result = await _userService.ChangePassword(user, userId);
                if (result == false)
                {
                    return BadRequest("Failed to change password!");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }

        //[AllowAnonymous]
        //[HttpPost("authenticate")]
        //public IActionResult Authenticate([FromBody]User userParam)
        //{
        //    var user = _userService.Authenticate(userParam.username, userParam.password);

        //    if (user == null)
        //        return BadRequest(new { message = "Username or password is incorrect" });

        //    return Ok(user);
        //}

        //public UsersController(BudgetContext context)
        //{
        //    _context = context;
        //}
    }
}
