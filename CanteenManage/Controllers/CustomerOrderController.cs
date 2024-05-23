using CanteenManage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CanteenManage.Controllers
{
    public class CustomerOrderController : Controller
    {
        private readonly CanteenManageContext _dbContext;

        public CustomerOrderController(CanteenManageContext dbContext)
        {
            this._dbContext = dbContext;
        }

        private void PopulateProductsInViewBag()
        {
            ViewBag.Products = _dbContext.Menus.ToList(); 
            ViewBag.Price = _dbContext.Menus.ToDictionary(m => m.ProductId, m => m.Price); 
        }

        public IActionResult OrderMain()
        {
            var customerOrders = _dbContext.CustomerOrders.ToList();
            return View(customerOrders);
        }

        public IActionResult PlaceOrder()
        {
            PopulateProductsInViewBag();

            ViewBag.Customers = _dbContext.Customers.Where(c => c.Type == "Prepaid").ToList(); // Assuming Customer model has CustomerId and Name properties
            ViewBag.PrepaidBalance = _dbContext.Customers.Where(c => c.Type == "Prepaid").ToDictionary(c => c.CustomerId, c => c.PrepaidBalance);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(CustomerOrder customerOrder)
        {
            if (ModelState.IsValid)
            {
                var customer = await _dbContext.Customers.FindAsync(customerOrder.CustomerId);
                if (customer != null)
                {
                    decimal remainingBalance = (customer.PrepaidBalance ?? 0) - (customerOrder.Amount ?? 0);

                    if (remainingBalance < 0)
                    {
                        ModelState.AddModelError(string.Empty, "Insufficient balance.");
                        PopulateProductsInViewBag(); 
                        ViewBag.Customers = _dbContext.Customers.Where(c => c.Type == "Prepaid").ToList();
                        return View(customerOrder);
                    }
                    customer.PrepaidBalance = remainingBalance;
                }
                _dbContext.CustomerOrders.Add(customerOrder);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(OrderMain));
            }
            PopulateProductsInViewBag(); 
            ViewBag.Customers = _dbContext.Customers.Where(c => c.Type == "Prepaid").ToList();
            return View(customerOrder);
        }


        public IActionResult NewOrder()
        {
            PopulateProductsInViewBag();
            return View(new CustomerOrder());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewOrder(CustomerOrder customerOrder)
        {
            if (ModelState.IsValid)
            {
                // Calculate amount based on the quantity and product price
                var productPrice = _dbContext.Menus.Where(m => m.ProductId == customerOrder.ProductId).Select(m => m.Price).FirstOrDefault();
                customerOrder.Amount = productPrice * customerOrder.Quantity;

                _dbContext.CustomerOrders.Add(customerOrder);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(OrderMain));
            }

            PopulateProductsInViewBag(); // Populate products again if model state is not valid
            return View(customerOrder);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var orderToDelete = await _dbContext.CustomerOrders.FindAsync(id);
            if (orderToDelete != null)
            {
                _dbContext.CustomerOrders.Remove(orderToDelete);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(OrderMain));
        }
    }
}
