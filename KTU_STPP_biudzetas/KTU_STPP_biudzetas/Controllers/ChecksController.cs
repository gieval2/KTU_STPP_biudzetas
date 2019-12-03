using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KTU_STPP_biudzetas.Models;
using Microsoft.AspNetCore.Authorization;
using KTU_STPP_biudzetas.Services;
using System;

namespace KTU_STPP_biudzetas.Controllers
{
    [Authorize(Policy = "RequireClaimMember")]
    [Route("api/Checks")]
    [ApiController]
    public class ChecksController : ControllerBase
    {
        private readonly ICheckService _checkService;
        public ChecksController(ICheckService checkService)
        {
            _checkService = checkService;
        }

        // GET: api/Checks
        [HttpGet]
        public async Task<IEnumerable<Check>> Get()
        {
            return await _checkService.GetAllAsync();
        }

        // GET: api/Checks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Check>> GetCheck(int id)
        {
            var check = await _checkService.GetAsync(id);

            if (check == null)
            {
                return NotFound();
            }

            return check;
        }

        // PUT: api/Checks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCheck(int id, Check check)
        {
            if (id != check.Id)
            {
                return BadRequest();
            }

            try
            {
                check.Id = id;
                await _checkService.UpdateAsync(check);
            }
            catch (Exception e)
            {
                if (!CheckExists(id))
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

        // POST: api/Checks
        [HttpPost]
        public async Task<ActionResult<Check>> PostCheck([FromBody] Check check)
        {
            await _checkService.CreateAsync(check);

            //return CreatedAtAction("GetCheck", new { id = check.Id }, check);
            return CreatedAtAction(nameof(GetCheck), new { id = check.Id }, check);
        }

        // DELETE: api/Checks/5
        [Authorize(Policy = "RequireClaimFamilyHead")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Check>> DeleteCheck(int id)
        {
            var check = await _checkService.GetAsync(id);
            if (check == null)
            {
                return NotFound();
            }

            await _checkService.DeleteAsync(id);

            return check;
        }

        private bool CheckExists(int id)
        {
            return _checkService.GetAllAsync().Result.Any(e => e.Id == id);
        }
    }
}
