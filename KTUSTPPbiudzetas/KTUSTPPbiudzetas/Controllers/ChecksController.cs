using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using KTUSTPPBiudzetas.Models;
using KTUSTPPBiudzetas.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KTUSTPPBiudzetas.Controllers
{
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

        // GET: Checks
        public async Task<ActionResult> Index()
        {
            var checks = await _checkService.GetAllAsync();

            return View("~/Views/Checks/CheckList.cshtml", checks);
        }

        // GET: Checks/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Checks/Create
        [HttpGet("Budget/Checks/Create")]
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: Checks/Create
        [HttpPost("Budget/Checks/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                using (var httpClient = new HttpClient())
                {
                    Check check = new Check();
                    check.Date = DateTime.Parse(collection["Date"]);
                    check.MemberId = 1;
                    using (var response = await httpClient.PostAsJsonAsync<Check>("https://localhost:44330/Budget/Checks", check))
                    {
                        var checks = await _checkService.GetAllAsync();
                        return View("~/Views/Checks/CheckList.cshtml", checks);
                    }
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Checks/Edit/5
        [HttpGet("Budget/Checks/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44330/Budget/Checks/" + id))
                {
                    var apiResponse = await response.Content.ReadAsAsync<Check>();
                    //return View("~/Views/Checks/Edit.cshtml", apiResponse);
                    return View(apiResponse);
                }
            }
        }

        // POST: Checks/Edit/5
        [HttpPost("Budget/Checks/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                using (var httpClient = new HttpClient())
                {
                    Check check = new Check();
                    check.Id = id;
                    check.Date = DateTime.Parse(collection["Date"]);
                    //if (collection["MemberId"] == "")
                    //{
                        check.MemberId = 1;
                    //}
                    //check.MemberId = int.Parse(collection["MemberId"].ToString());
                    using (var apiResponse = await httpClient.PutAsJsonAsync<Check>("https://localhost:44330/Budget/Checks/"+id, check))
                    {
                        var response = await httpClient.GetAsync("https://localhost:44330/Budget/Checks");
                        var checks = await response.Content.ReadAsAsync<IList<Check>>();
                        return View("~/Views/Checks/CheckList.cshtml", checks);
                        //return View(response.);
                    }
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Checks/Delete/5
        [HttpGet("Budget/Checks/Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44330/Budget/Checks/" + id))
                {
                    var apiResponse = await response.Content.ReadAsAsync<Check>();
                    //return View("~/Views/Checks/Edit.cshtml", apiResponse);
                    return View(apiResponse);
                }
            }
        }

        // POST: Checks/Delete/5
        [HttpPost("Budget/Checks/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync("https://localhost:44330/Budget/Checks/" + id))
                    {
                        
                    }
                    var checks = await _checkService.GetAllAsync();
                    return View("~/Views/Checks/CheckList.cshtml", checks);
                }
            }
            catch
            {
                return View();
            }
        }
    }
}