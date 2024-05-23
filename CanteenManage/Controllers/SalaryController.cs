using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CanteenManage.Models;
using Microsoft.EntityFrameworkCore;

namespace CanteenManage.Controllers
{
    public class SalaryController : Controller
    {
        private readonly CanteenManageContext _dbContext;

        public SalaryController(CanteenManageContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult SalaryMain(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var salary = _dbContext.Salaries.FirstOrDefault(s => s.EmployeeId == id);
            if (salary == null)
            {
                return NotFound();
            }

            return View(salary);
        }
    }
}
