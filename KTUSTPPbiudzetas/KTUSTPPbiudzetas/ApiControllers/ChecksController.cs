using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using KTUSTPPBiudzetas.Services;
using KTUSTPPBiudzetas.Models;

namespace KTUSTPPBiudzetas.ApiControllers
{
    //[Authorize(Policy = "RequireClaimMember")]
    [Route("Budget/Checks")]
    [ApiController]
    public class ChecksController : Controller
    {
        private readonly ICheckService _checkService;
        private readonly IMemberService _memberService;
        private readonly IPurchaseService _purchaseService;

        public ChecksController(ICheckService checkService, IPurchaseService purchaseService, IMemberService memberService)
        {
            _checkService = checkService;
            _memberService = memberService;
            _purchaseService = purchaseService;
        }

        // GET: api/Checks
        [HttpGet]
        public async Task<IEnumerable<Check>> GetChecks()
        {
            //ViewData["CheckId"] = CheckId;
            var checks = await _checkService.GetAllAsync();
            return checks;
            //return View("~/Views/Checks/CheckList.cshtml", checks);
        }

        // GET: api/Checks/5
        [HttpGet("{CheckId}")]
        public async Task<ActionResult<Check>> GetCheck(int CheckId)
        {
            var check = await _checkService.GetAsync(CheckId);

            if (check == null)
            {
                return NotFound();
            }

            return check;
        }

        // PUT: api/Checks/5
        [HttpPut("{CheckId}")]
        public async Task<IActionResult> Put(int CheckId, Check check)
        {
            if (CheckId != check.Id)
            {
                return BadRequest();
            }

            try
            {
                check.Id = CheckId;
                await _checkService.UpdateAsync(check);
            }
            catch (Exception e)
            {
                if (!CheckExists(CheckId))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(e.InnerException);
                }
            }

            return NoContent();
        }

        // POST: api/Checks
        [HttpPost]
        public async Task<ActionResult<Check>> Post([FromBody] Check check)
        {
            try
            {
                Member member = await _memberService.GetAsync((int)check.MemberId);
                check.Member = member;
                //check.Purchases = new List<Purchase>();
                check = await _checkService.CreateAsync(check);
                member.Checks.Add(check);
                await _memberService.UpdateAsync(member);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
            //return CreatedAtAction("GetCheck", new { id = check.Id }, check);
            //return CreatedAtAction(nameof(GetCheck), new { id = check.Id }, check);
            return Ok();
        }

        // DELETE: api/Checks/5
        [Authorize(Policy = "RequireClaimFamilyHead")]
        [HttpDelete("{CheckId}")]
        public async Task<ActionResult<Check>> Delete(int CheckId)
        {
            var check = await _checkService.GetAsync(CheckId);
            if (check == null)
            {
                return NotFound();
            }
            
            if (check.Purchases.Count != 0)
            {
                foreach (var purchase in check.Purchases)
                {
                    await _purchaseService.DeleteAsync(purchase.Id);
                }
                check.Purchases = new List<Purchase>();
            }
            
            await _checkService.DeleteAsync(CheckId);

            return check;
        }

        private bool CheckExists(int id)
        {
            return _checkService.GetAllAsync().Result.Any(e => e.Id == id);
        }
    }
}
