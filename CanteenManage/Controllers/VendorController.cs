using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CanteenManage.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CanteenManage.Controllers
{
    
    public class VendorController : Controller
    {
        private readonly CanteenManageContext _dbContext;

        public VendorController(CanteenManageContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult VendorMain()
        {
            var vendors = _dbContext.Vendors.ToList();
            return View(vendors);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                await _dbContext.Vendors.AddAsync(vendor);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(VendorMain));
            }

            return View(vendor);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var vendor = _dbContext.Vendors.Find(id);
            if (vendor == null)
            {
                return NotFound();
            }

            return View(vendor);
        }

        [HttpPost]
        public IActionResult Edit(Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(vendor).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(VendorMain));
            }
            return View(vendor);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Vendor viewModel)
        {
            var vendor = await _dbContext.Vendors.AsNoTracking().FirstOrDefaultAsync(x => x.VendorId == viewModel.VendorId);
            if (vendor is not null)
            {
                _dbContext.Vendors.Remove(vendor);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("VendorMain", "Vendor");
        }

        [HttpGet]
         public IActionResult PurchaseDetails(int id) 
         {
            var vendorPurchases= _dbContext.VendPurchases.Where(p=>p.VendorId == id).ToList();
            ViewBag.VendorId=id;
            ViewBag.Purchases=vendorPurchases;
            return View(vendorPurchases);
         }

        [HttpGet]
        public IActionResult AddPurchase(int vendor_id)
        {
            ViewBag.VendorId=vendor_id;
            return View();
        }
        [HttpPost]
        public IActionResult AddPurchase(VendPurchase purchase)
        {
            if (ModelState.IsValid)
            {
                _dbContext.VendPurchases.Add(purchase);
                _dbContext.SaveChanges();
                

                // Redirect to the PurchaseDetails action with updated data
                return RedirectToAction("PurchaseDetails", new { id = purchase.VendorId });
            }

            return View(purchase);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePurchase(int id)
        {
            var purchase = await _dbContext.VendPurchases.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }

            _dbContext.VendPurchases.Remove(purchase);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("PurchaseDetails", new { id = purchase.VendorId });
        }


    }
}