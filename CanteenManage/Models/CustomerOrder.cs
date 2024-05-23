using System;
using System.Collections.Generic;

namespace CanteenManage.Models;

public partial class CustomerOrder
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Amount { get; set; }

    public decimal? RemainingBalance { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Menu? Product { get; set; }
}
