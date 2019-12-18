using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KTUSTPPBiudzetas.Models;

namespace KTUSTPPBiudzetas.Controllers
{
    public class MessagesController : Controller
    {
        private readonly BudgetContext _context;

        public MessagesController(BudgetContext context)
        {
            _context = context;
        }

        // GET: Messages
        //public async Task<IActionResult> Index()
        //{
        //    var budgetContext = _context.Messages.Include(m => m.Reciever).Include(m => m.Sender);
        //    return View(await budgetContext.ToListAsync());
        //}

        // GET: Messages/Details/5
        [HttpGet("Budget/Messages/Details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .Include(m => m.Reciever)
                .Include(m => m.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: Messages/Create
        [HttpGet("Budget/Messages/Create")]
        public IActionResult Create()
        {
            ViewData["RecieverId"] = new SelectList(_context.Members, "Id", "FirstName");
            ViewData["SenderId"] = new SelectList(_context.Members, "Id", "FirstName");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Budget/Messages/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,Text,SenderId,RecieverId,Id,LastUpdated")] Message message)
        {
            if (ModelState.IsValid)
            {
                message.Date = DateTime.UtcNow;
                message.LastUpdated = DateTime.UtcNow;
                _context.Add(message);
                await _context.SaveChangesAsync();
                var messages = await _context.Messages.ToListAsync();
                return View("~/Views/Messages/List.cshtml", messages);
            }
            ViewData["RecieverId"] = new SelectList(_context.Members, "Id", "FirstName", message.RecieverId);
            ViewData["SenderId"] = new SelectList(_context.Members, "Id", "FirstName", message.SenderId);
            return View(message);
        }

        // GET: Messages/Edit/5
        [HttpGet("Budget/Messages/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            ViewData["RecieverId"] = new SelectList(_context.Members, "Id", "FirstName", message.RecieverId);
            ViewData["SenderId"] = new SelectList(_context.Members, "Id", "FirstName", message.SenderId);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Budget/Messages/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Date,Text,SenderId,RecieverId,Id,LastUpdated")] Message message)
        {
            if (id != message.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var messages = await _context.Messages.ToListAsync();
                return View("~/Views/Messages/List.cshtml", messages);
            }
            ViewData["RecieverId"] = new SelectList(_context.Members, "Id", "FirstName", message.RecieverId);
            ViewData["SenderId"] = new SelectList(_context.Members, "Id", "FirstName", message.SenderId);
            return View(message);
        }

        // GET: Messages/Delete/5
        [HttpGet("Budget/Messages/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .Include(m => m.Reciever)
                .Include(m => m.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost("Budget/Messages/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            var messages = await _context.Messages.ToListAsync();
            return View("~/Views/Messages/List.cshtml", messages);
        }

        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }
    }
}
