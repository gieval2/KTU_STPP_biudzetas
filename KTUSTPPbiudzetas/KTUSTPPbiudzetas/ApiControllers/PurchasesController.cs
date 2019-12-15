using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KTUSTPPBiudzetas.Models;
using Microsoft.AspNetCore.Authorization;
using KTUSTPPBiudzetas.Services;
using System;
using System.Net.Http;

namespace KTUSTPPBiudzetas.ApiControllers
{
    //[Authorize(Policy = "RequireClaimMember")]
    //[Route("Budget/Purchases")]
    [Route("Budget/Checks/{CheckId}/[controller]")]
    [ApiController]
    public class PurchasesController : Controller
    {
        private readonly IPurchaseService _purchaseService;
        private readonly ICheckService _checkService;
        public PurchasesController(IPurchaseService purchaseService, ICheckService checkService)
        {
            _purchaseService = purchaseService;
            _checkService = checkService;
        }

        // GET: api/Purchases
        [HttpGet]
        //[NonAction]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetPurchases(int CheckId)
        {
            var check = await _checkService.GetAsync(CheckId);
            var purchases = await _purchaseService.GetByCheckIdAsync(CheckId);
            //var purchases = await _purchaseService.GetAllAsync();
            ViewData["CheckId"] = CheckId;
            return View("~/Views/Purchases/PurchaseList.cshtml", purchases);
            //return purchases;
        }

        // GET: api/Purchases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Purchase>> GetPurchase(int CheckId, int id)
        {
            var purchase = await _purchaseService.GetAsync(id);

            if (purchase == null)
            {
                return NotFound();
            }

            return View("~/Views/Purchases/Edit.cshtml", purchase);
        }

        // PUT: api/Purchases/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchase(int CheckId, int id,[FromBody] Purchase purchase)
        {
            if (id != purchase.Id)
            {
                return BadRequest();
            }

            try
            {
                await _purchaseService.UpdateAsync(purchase);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseExists(id))
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

        // POST: api/Purchases
        [HttpPost]
        public async Task<ActionResult<Purchase>> PostPurchase(int CheckId, [FromBody] Purchase purchase)
        {
            try
            {
                Check check = await _checkService.GetAsync((int)purchase.CheckId);
                if (check.Purchases==null)
                {
                    check.Purchases = new List<Purchase>();
                }
                purchase.Check = check;
                purchase = await _purchaseService.CreateAsync(purchase);
                check.Purchases.Add(purchase);
                await _checkService.UpdateAsync(check);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
            //return CreatedAtAction("GetPurchase", new { id = purchase.Id }, purchase);
            //return CreatedAtAction(nameof(GetPurchase), new { id = purchase.Id }, purchase);
            return Ok();
        }

        // DELETE: api/Purchases/5
        //[Authorize(Policy = "RequireClaimFamilyHead")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Purchase>> Delete(int CheckId, int id)
        {
            var purchase = await _purchaseService.GetAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }

            await _purchaseService.DeleteAsync(id);

            return purchase;
        }

        private bool PurchaseExists(int id)
        {
            return _purchaseService.GetAllAsync().Result.Any(e => e.Id == id);
        }

        
    }
}
