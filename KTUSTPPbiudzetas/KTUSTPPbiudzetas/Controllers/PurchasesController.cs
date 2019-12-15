using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using KTUSTPPBiudzetas.Models;
using KTUSTPPBiudzetas.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KTUSTPPBiudzetas.Controllers
{
    //[Route("Budget/Checks/{CheckId}/[controller]")]
    public class PurchasesController : Controller
    {
        private readonly IPurchaseService _purchaseService;
        private readonly ICheckService _checkService;
        public PurchasesController(IPurchaseService purchaseService, ICheckService checkService)
        {
            _purchaseService = purchaseService;
            _checkService = checkService;
        }

        //[HttpGet("Budget/Members/Edit/{id}")]
        //[Route("Budget/Checks/{CheckId}/Purchases")]
        //public async Task<IActionResult> Index(int CheckId)
        //{
        //    //var purchases = await _purchaseService.GetByCheckIdAsync(CheckId);
        //    using (var httpClient = new HttpClient())
        //    {
        //        using (var response = await httpClient.GetAsync("https://localhost:44330/Budget/Checks/" + CheckId + "/Purchases"))
        //        {
        //            var apiResponse = await response.Content.ReadAsAsync<IEnumerable<Purchase>>();
        //            return View("~/Views/Purchases/PurchaseList.cshtml", apiResponse);
        //        }
        //    }
        //}

        //// GET: Purchases/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: Purchases/Create
        [HttpGet("Budget/Checks/{CheckId}/Purchases/Create")]
        public async Task<IActionResult> Create(int CheckId)
        {
            //using (var httpClient = new HttpClient())
            //{
            //    Purchase purchase = new Purchase();
            //    purchase.CheckId = CheckId;
            //    using (var response = await httpClient.GetAsync("https://localhost:44330/Budget/Checks/"+ CheckId + "/Purchases/" + 1))
            //    {
            //        var apiResponse = await response.Content.ReadAsAsync<Check>();
            //        //purchase.Check = apiResponse;
            //        return View("~/Views/Purchases/Create.cshtml", apiResponse);
            //    }
            //}
            return View();
        }

        // POST: Purchases/Create
        [HttpPost("Budget/Checks/{CheckId}/Purchases/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection, int CheckId)
        {
            ViewData["CheckId"] = CheckId;
            try
            {
                // TODO: Add insert logic here
                using (var httpClient = new HttpClient())
                {
                    Purchase purchase = new Purchase();
                    purchase.Name = collection["Name"].ToString();
                    purchase.Amount = Double.Parse(collection["Amount"]);
                    purchase.Price = Double.Parse(collection["Price"]);
                    purchase.CheckId = CheckId;
                    using (var response = await httpClient.PostAsJsonAsync<Purchase>("https://localhost:44330/Budget/Checks/" + CheckId + "/Purchases",purchase))
                    {
                        var purchases = await _purchaseService.GetByCheckIdAsync(CheckId);
                        ViewData["CheckId"] = CheckId;
                        return View("~/Views/Purchases/PurchaseList.cshtml", purchases);
                        //var apiResponse = await response.Content.ReadAsAsync<Check>();
                    }
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Purchases/Edit/5
        //[HttpGet("Budget/Purchases/Edit/{id}")]
        [HttpGet("Budget/Checks/{CheckId}/Purchases/Edit/{id}")]
        //[HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int CheckId, int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44330/Budget/Checks/" + CheckId + "/Purchases/" + id))
                {
                    var apiResponse = await _purchaseService.GetAsync(id);
                    //return View("~/Views/Purchases/Edit.cshtml", apiResponse);
                    return View(apiResponse);
                }
            }

            //return View("~/Views/Members/MemberEdit.cshtml", user);
        }

        // POST: Purchases/Edit/5
        [HttpPost("Budget/Checks/{CheckId}/Purchases/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int CheckId, IFormCollection collection, int id)
        {
            ViewData["CheckId"] = CheckId;
            try
            {
                // TODO: Add insert logic here
                using (var httpClient = new HttpClient())
                {
                    Purchase purchase = new Purchase();
                    purchase.Name = collection["Name"].ToString();
                    purchase.Amount = Double.Parse(collection["Amount"]);
                    purchase.Price = Double.Parse(collection["Price"]);
                    purchase.CheckId = CheckId;
                    using (var response = await httpClient.PostAsJsonAsync<Purchase>("https://localhost:44330/Budget/Checks/" + CheckId + "/Purchases", purchase))
                    {
                        var purchases = await _purchaseService.GetByCheckIdAsync(CheckId);
                        ViewData["CheckId"] = CheckId;
                        return View("~/Views/Purchases/PurchaseList.cshtml", purchases);
                        //var apiResponse = await response.Content.ReadAsAsync<Check>();
                    }
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Purchases/Delete/5
        [HttpGet("Budget/Checks/{CheckId}/Purchases/Delete/{id}")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Purchases/Delete/5
        [HttpPost("Budget/Checks/{CheckId}/Purchases/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection, int CheckId)
        {
            try
            {
                // TODO: Add delete logic here
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync("https://localhost:44330/Budget/Checks/" + CheckId + "/Purchases/" + id))
                    {
                        var purchases = await _purchaseService.GetByCheckIdAsync(CheckId);
                        ViewData["CheckId"] = CheckId;
                        return View("~/Views/Purchases/PurchaseList.cshtml", purchases);
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}