using System;
using System.Collections.Generic;

namespace CanteenManage.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? Name { get; set; }

    public int? Age { get; set; }

    public DateOnly? JoiningDate { get; set; }

    public DateOnly? ResignDate { get; set; }

    public decimal? Salary { get; set; }

    public string? Address { get; set; }

    public string? Aadhar { get; set; }

    public string? Phone { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();
}
