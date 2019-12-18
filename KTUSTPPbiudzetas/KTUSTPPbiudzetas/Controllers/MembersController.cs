using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KTUSTPPBiudzetas.Models;
using KTUSTPPBiudzetas.Services;
using System.Net.Http;

namespace KTUSTPPBiudzetas.Controllers
{
    public class MembersController : Controller
    {
        private readonly BudgetContext _context;
        private readonly IMemberService _memberService;

        public MembersController(BudgetContext context, IMemberService memberService)
        {
            _context = context;
            _memberService = memberService;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            var members = _context.Members.Include(m => m.User);
            return View(await members.ToListAsync());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        [HttpGet("Budget/Members/Create")]
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Budget/Members/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,Password,FirstName,LastName,Email,Family,FamilyLevel,Token,Limit,LimitState,UserId,Id,LastUpdated")] Member member)
        {
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                var members = _context.Members.Include(m => m.User);
                return View(await members.ToListAsync());
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", member.UserId);
            return View(member);
        }

        // GET: Members/Edit/5
        [HttpGet("Budget/Members/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _memberService.GetAsync(id);
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44330/Budget/Members/" + id))
                {
                    var apiResponse = await response.Content.ReadAsAsync<Member>();
                    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", user.UserId);
                    return View(user);
                }
            }
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Budget/Members/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Username,Password,FirstName,LastName,Email,Family,FamilyLevel,Token,Limit,LimitState,UserId,Id,LastUpdated")] Member member)
        {
            if (id != member.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        Member user = new Member
                        {
                            Id = member.Id,
                            Username = member.Username,
                            Password = member.Password,
                            FirstName = member.FirstName,
                            LastName = member.LastName,
                            Email = member.Email,
                            Family = member.Family,
                            FamilyLevel = member.FamilyLevel,
                            Token = member.Token,
                            Limit = member.Limit,
                            LimitState = member.LimitState,
                            UserId = member.UserId,
                            User = member.User,
                            Checks = member.Checks,
                            Sent = member.Sent,
                            Recieved = member.Recieved,
                            LastUpdated = member.LastUpdated
                        };
                        using (var response = await httpClient.PutAsJsonAsync<Member>("https://localhost:44330/Budget/Members/" + id, member))
                        {
                            //return View(apiResponse);
                        }
                        var apiResponse = await _memberService.GetAllAsync();
                        return View("~/Views/Members/MemberList.cshtml", apiResponse);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", member.UserId);
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }
    }
}
