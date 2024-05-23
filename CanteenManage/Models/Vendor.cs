using System;
using System.Collections.Generic;

namespace CanteenManage.Models;

public partial class Vendor
{
    public int VendorId { get; set; }

    public string VendorName { get; set; } = null!;

    public string? VendorType { get; set; }

    public string? Phone { get; set; }

    public virtual ICollection<VendPurchase> VendPurchases { get; set; } = new List<VendPurchase>();
}
