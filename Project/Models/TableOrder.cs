using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class TableOrder
{
    public int OrderId { get; set; }

    public string? OrderName { get; set; }

    public int? OrderDiscountedprice { get; set; }

    public int? OrderTotalPrice { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? MemberId { get; set; }

    public string? OrderAddress { get; set; }

    public string? OrderPhone { get; set; }

    public string? OrderStatus { get; set; }

    public bool? OrderPayment { get; set; }
}
