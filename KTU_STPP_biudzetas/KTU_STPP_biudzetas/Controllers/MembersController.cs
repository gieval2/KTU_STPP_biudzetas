using KTU_STPP_biudzetas.Models;
using KTU_STPP_biudzetas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KTU_STPP_biudzetas.Controllers
{
    [Authorize(Policy = "RequireClaimMember")]
    [Route("api/Members")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;
        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        //GET: api/Members
        [HttpGet]
        public async Task<IEnumerable<Member>> Get()
        {
            return await _memberService.GetAllAsync();
        }

        // GET: api/Members/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> Get(int id)
        {
            //var user = await _context.Users.Include(a => a.Checks).ThenInclude(b => b.Purchases).FirstOrDefaultAsync(c => c.Id == id);
            var user = await _memberService.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Members/5
        [Authorize(Policy = "RequireClaimFamilyHead")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Member user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            try
            {
                user.Id = id;
                await _memberService.UpdateAsync(user);
            }
            catch (Exception e)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(e.InnerException); ;
                }
            }

            return NoContent();
        }

        // POST: api/Members
        [Authorize(Policy = "RequireClaimFamilyHead")]
        [HttpPost]
        public async Task<ActionResult<Member>> Post([FromBody] Member user)
        {
            await _memberService.CreateAsync(user);

            //return CreatedAtAction("GetUser", new { id = user.Id }, user);
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        // DELETE: api/Members/5
        [Authorize(Policy = "RequireClaimFamilyHead")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Member>> Delete(int id)
        {
            var user = await _memberService.GetAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _memberService.DeleteAsync(id);

            return user;
        }

        [Authorize(Policy = "RequireClaimFamilyHead")]
        [HttpPost("SetLimit")]
        public async Task<ActionResult<Member>> SetLimit(int memberId, double newLimit)
        {
            try
            {
                var user = await _memberService.SetLimit(memberId, newLimit);

                return user;
            }
            catch (Exception e)
            {
                if (!UserExists(memberId))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(e.InnerException); ;
                }
            }
        }
        
        [Authorize(Policy = "RequireClaimController")]
        [HttpPost("ConfirmLimit")]
        public async Task<ActionResult<Member>> ConfirmLimit(int memberId)
        {
            try
            {
                var user = await _memberService.ConfirmLimit(memberId);

                return user;
            }
            catch (Exception e)
            {
                if (!UserExists(memberId))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(e.InnerException); ;
                }
            }
        }

        [Authorize(Policy = "RequireClaimFamilyHead")]
        [HttpPost("ResetLimits")]
        public async Task<IEnumerable<Member>> ResetLimits()
        {
            return await _memberService.ResetLimits();
        }

        private bool UserExists(int id)
        {
            return _memberService.GetAllAsync().Result.Any(e => e.Id == id);
        }
    }
}
