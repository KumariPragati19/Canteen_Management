using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CanteenManage.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;

namespace CanteenManage.Controllers
{
    
    public class CustomerController : Controller
    {
        private readonly CanteenManageContext _dbContext;

        public CustomerController(CanteenManageContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult CustomerMain(string searchInput)
        {
            var customers = _dbContext.Customers.ToList();

            if (!string.IsNullOrEmpty(searchInput))
            {
                customers = customers.Where(c => c.Name.Contains(searchInput) || c.Phone.Contains(searchInput)).ToList();
            }
            foreach (var customer in customers)
            {
                // Calculate balance for each customer (assuming balance is the remaining balance)
                customer.PrepaidBalance = customer.PrepaidBalance - customer.CustomerOrders.Sum(co => co.Amount ?? 0);
            }
            return View(customers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                await _dbContext.Customers.AddAsync(customer);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(CustomerMain));
            }

            return View(customer);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var customer = _dbContext.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(customer).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(CustomerMain));
            }
            return View(customer);
        }
      
        [HttpPost]
        public async Task<IActionResult> Delete(Customer viewModel)
        {
            var customer = await _dbContext.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.CustomerId == viewModel.CustomerId);
            if (customer is not null)
            {
                _dbContext.Customers.Remove(customer);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("CustomerMain", "Customer");
        }

   
        [HttpGet]
        public IActionResult TakeAttendance()
        {
            var todayDate = DateOnly.FromDateTime(DateTime.Today);

            var subscribedCustomers = _dbContext.Customers
                .Where(c => c.SubscriptionStartDate <= todayDate && c.SubscriptionExpiryDate >= todayDate)
                .ToList();

            var existingAttendance = _dbContext.CustomerAttendances
                .Where(a => a.Date == todayDate)
                .ToList();

            var attendanceList = new List<CustomerAttendance>();
            foreach (var customer in subscribedCustomers)
            {
                var attendance = existingAttendance.FirstOrDefault(a => a.CustomerId == customer.CustomerId && a.CustomerName == customer.Name);
                if (attendance == null)
                {
                    attendance = new CustomerAttendance
                    {
                        CustomerId = customer.CustomerId,
                        CustomerName = customer.Name,
                        Date = todayDate,
                        Status = "Absent"
                    };
                }
                else
                {
                    attendance.CustomerName = customer.Name;
                }
                attendanceList.Add(attendance);
            }

            return View(attendanceList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TakeAttendance(List<CustomerAttendance> AttendanceRecords)
        {
            if (ModelState.IsValid)
            {
                foreach (var attendanceRecord in AttendanceRecords)
                {
                    var customer = _dbContext.Customers.FirstOrDefault(c => c.CustomerId == attendanceRecord.CustomerId);

                    if (customer != null)
                    {
                        var existingAttendance = _dbContext.CustomerAttendances.FirstOrDefault(a => a.CustomerId == attendanceRecord.CustomerId && a.Date == attendanceRecord.Date);

                        if (existingAttendance != null)
                        {
                            // Update the existing attendance record
                            existingAttendance.Status = attendanceRecord.Status;
                        }
                        else
                        {
                            // Add the new attendance record to the database
                            attendanceRecord.CustomerName = customer.Name; // Assign the customer name
                            _dbContext.CustomerAttendances.Add(attendanceRecord);
                        }
                    }
                    else
                    {
                        // Handle scenario where the customer doesn't exist
                    }
                }

                // Save changes to the database
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(TakeAttendance));
            }

            return View(AttendanceRecords);
        }

    }
}


