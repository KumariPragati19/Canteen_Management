using System;
using System.Collections.Generic;

namespace CanteenManage.Models;

public partial class Attendance
{
    public int AttendanceId { get; set; }

    public int EmployeeId { get; set; }

    public DateOnly Date { get; set; }

    public string? Status { get; set; }

    public double? OvertimeHours { get; set; }

    public virtual Employee? Employee { get; set; } = null!;
}
