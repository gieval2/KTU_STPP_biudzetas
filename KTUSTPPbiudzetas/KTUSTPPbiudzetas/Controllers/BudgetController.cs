using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KTUSTPPBiudzetas.Models;
using KTUSTPPBiudzetas.Services;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace KTUSTPPBiudzetas.Controllers
{
    public class BudgetController : Controller
    {
        private readonly ILogger<BudgetController> _logger;
        private readonly IMemberService _memberService;

        public BudgetController(ILogger<BudgetController> logger, IMemberService memberService)
        {
            _logger = logger;
            _memberService = memberService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("Budget/Members/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _memberService.GetAsync(id);
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44330/Budget/Members/" + id))
                {
                    var apiResponse = await response.Content.ReadAsAsync<Member>();
                    return View("~/Views/Members/MemberEdit.cshtml", apiResponse);
                }
            }
            
            //return View("~/Views/Members/MemberEdit.cshtml", user);
        }

        //public IActionResult Members()
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
