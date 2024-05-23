using System;
using System.Collections.Generic;

namespace CanteenManage.Models;

public partial class VendPurchase
{
    public int OrderId { get; set; }

    public int? VendorId { get; set; }

    public string? ProductName { get; set; }

    public decimal? Price { get; set; }

    public DateTime? OrderDate { get; set; }

    public virtual Vendor? Vendor { get; set; }
}
