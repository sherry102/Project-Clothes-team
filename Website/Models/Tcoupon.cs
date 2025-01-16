using System;
using System.Collections.Generic;

namespace Website.Models;

public partial class Tcoupon
{
    public int CouponId { get; set; }

    public string CouponName { get; set; } = null!;

    public int Mid { get; set; }

    public double CouponDiscount { get; set; }
}
