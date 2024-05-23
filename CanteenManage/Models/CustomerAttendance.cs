using System;
using System.Collections.Generic;

namespace CanteenManage.Models;

public partial class CustomerAttendance
{
    public int AttendanceId { get; set; }

    public int? CustomerId { get; set; }

    public DateOnly? Date { get; set; }

    public string? Status { get; set; }

    public string? CustomerName { get; set; }

    public virtual Customer? Customer { get; set; }
}
