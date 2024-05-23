using System;
using System.Collections.Generic;

namespace CanteenManage.Models;

public partial class Salary
{
    public int SalaryId { get; set; }

    public int? EmployeeId { get; set; }

    public string? SalaryMonth { get; set; }

    public decimal? PaidAmount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public virtual Employee? Employee { get; set; }
}
