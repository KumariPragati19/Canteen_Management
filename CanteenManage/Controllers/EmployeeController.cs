using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CanteenManage.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CanteenManage.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly CanteenManageContext _dbContext;

        public EmployeeController(CanteenManageContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Main()
        {
            var employees = _dbContext.Employees.ToList();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _dbContext.Employees.AddAsync(employee);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Main));
            }

            return View(employee);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var employee = _dbContext.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(employee).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Main));
            }
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Employee viewModel)
        {
            var employee = await _dbContext.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.EmployeeId == viewModel.EmployeeId);
            if (employee is not null)
            {
                _dbContext.Employees.Remove(employee);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Main", "Employee");
        }

        [HttpGet]
        public IActionResult SalaryMain(int id)
        {
            var employeeSalaries = _dbContext.Salaries.Where(s => s.EmployeeId == id).ToList();
            var employeeAttendance = _dbContext.Attendances.Where(a => a.EmployeeId == id).ToList();

            ViewBag.EmployeeId = id;
            ViewBag.AttendanceDetails = employeeAttendance;
            return View(employeeSalaries);
        }

        [HttpGet]
        public IActionResult AddSalary(int employeeId)
        {
            ViewBag.EmployeeId = employeeId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSalary(Salary salary)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Salaries.Add(salary);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("SalaryMain", new { id = salary.EmployeeId });
            }

            return View(salary);
        }

        [HttpPost]
        public IActionResult MarkAttendance(Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Attendances.Add(attendance);
                _dbContext.SaveChanges();
                var employeeAttendance = _dbContext.Attendances.Where(a => a.EmployeeId == attendance.EmployeeId).ToList();
                ViewBag.AttendanceDetails = employeeAttendance;
                return RedirectToAction("SalaryMain", new { id = attendance.EmployeeId });
       
            }
         
            return View(attendance);
        }

        [HttpGet]
        public IActionResult MarkAttendanceForm(int employeeId)
        {
            var todayAttendance = _dbContext.Attendances.FirstOrDefault(a => a.EmployeeId == employeeId && a.Date == DateOnly.FromDateTime(DateTime.Today));

            if (todayAttendance != null)
            {
                return View("AlreadyMarked", todayAttendance);
            }
            
            var newAttendance = new Attendance
            {
                EmployeeId = employeeId,
                Date = DateOnly.FromDateTime(DateTime.Today)
            };

            return View(newAttendance);
        }
    }
}
