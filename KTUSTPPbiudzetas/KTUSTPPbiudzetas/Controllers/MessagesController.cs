﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KTUSTPPBiudzetas.Models;
using Microsoft.AspNetCore.Authorization;

namespace KTUSTPPBiudzetas.Controllers
{
    //[Authorize(Policy = "RequireClaimMember")]
    //[Route("api/Members/{memberId}/Messages")]
    [Route("Home/Messages")]
    [ApiController]
    public class MessagesController : Controller
    {
        private readonly BudgetContext _context;

        public MessagesController(BudgetContext context)
        {
            _context = context;
        }

        // GET: api/Messages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> Get()
        {
            var messages = await _context.Messages.ToListAsync();
            return View("~/Views/Messages/MessageList.cshtml", messages);
        }

        // GET: api/Messages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> Get(int id)
        {
            Message message = await _context.Messages.FirstOrDefaultAsync(c => c.Id == id);

            if (message == null)
            {
                return NotFound();
            }

            return message;
        }

        // PUT: api/Messages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Message message)
        {
            if (id != message.Id)
            {
                return BadRequest();
            }

            _context.Entry(message).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
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

        // POST: api/Messages
        [HttpPost]
        public async Task<ActionResult<Message>> Post(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetMessage", new { id = message.Id }, message);
            return CreatedAtAction(nameof(Get), new { id = message.Id }, message);
        }

        // DELETE: api/Messages/5
        [Authorize(Policy = "RequireClaimFamilyHead")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Message>> Delete(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return message;
        }

        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }
    }
}
