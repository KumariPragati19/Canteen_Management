using System;
using System.Collections.Generic;

namespace CanteenManage.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Type { get; set; }

    public DateOnly? SubscriptionStartDate { get; set; }

    public DateOnly? SubscriptionExpiryDate { get; set; }

    public decimal? PrepaidBalance { get; set; }

    public virtual ICollection<CustomerAttendance> CustomerAttendances { get; set; } = new List<CustomerAttendance>();

    public virtual ICollection<CustomerOrder> CustomerOrders { get; set; } = new List<CustomerOrder>();
}
