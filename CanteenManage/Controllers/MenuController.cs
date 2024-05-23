using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CanteenManage.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CanteenManage.Controllers
{
    public class MenuController : Controller
    {
        private readonly CanteenManageContext _dbContext;

        public MenuController(CanteenManageContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult MenuMain()
        {
            var menuItems = _dbContext.Menus.ToList();
            return View(menuItems);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Menu menuItem)
        {
            if (ModelState.IsValid)
            {
                await _dbContext.Menus.AddAsync(menuItem);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(MenuMain));
            }

            return View(menuItem);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var menuItem = _dbContext.Menus.Find(id);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        [HttpPost]
        public IActionResult Edit(Menu menuItem)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(menuItem).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(MenuMain));
            }
            return View(menuItem);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Menu viewModel)
        {
            var menuItem = await _dbContext.Menus.AsNoTracking().FirstOrDefaultAsync(x => x.ProductId == viewModel.ProductId);
            if (menuItem is not null)
            {
                _dbContext.Menus.Remove(menuItem);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("MenuMain", "Menu");
        }
    }
}
