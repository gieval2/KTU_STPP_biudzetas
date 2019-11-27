using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KTU_STPP_biudzetas.Models;
using Microsoft.AspNetCore.Authorization;

namespace KTU_STPP_biudzetas.Controllers
{
    [Authorize(Policy = "RequireClaimMember")]
    [Route("api/Checks")]
    [ApiController]
    public class ChecksController : ControllerBase
    {
        private readonly BudgetContext _context;

        public ChecksController(BudgetContext context)
        {
            _context = context;
        }

        // GET: api/Checks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Check>>> GetChecks()
        {
            return await _context.Checks.Include(b => b.Purchases).ToListAsync();
        }

        // GET: api/Checks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Check>> GetCheck(int id)
        {
            var check = await _context.Checks.Include(b => b.Purchases).FirstOrDefaultAsync(c => c.Id == id);

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

            _context.Entry(check).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Checks
        [HttpPost]
        public async Task<ActionResult<Check>> PostCheck(Check check)
        {
            _context.Checks.Add(check);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetCheck", new { id = check.Id }, check);
            return CreatedAtAction(nameof(GetCheck), new { id = check.Id }, check);
        }

        // DELETE: api/Checks/5
        [Authorize(Policy = "RequireClaimFamilyHead")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Check>> DeleteCheck(int id)
        {
            var check = await _context.Checks.FindAsync(id);
            if (check == null)
            {
                return NotFound();
            }

            _context.Checks.Remove(check);
            await _context.SaveChangesAsync();

            return check;
        }

        private bool CheckExists(int id)
        {
            return _context.Checks.Any(e => e.Id == id);
        }
    }
}
