using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class TableOrderDetail
{
    public int OrderDetailId { get; set; }

    public int? OrderId { get; set; }

    public int? MemberId { get; set; }

    public int? ProductsId { get; set; }

    public string? OrderName { get; set; }

    public string? ProductsName { get; set; }

    public int? ProductsPrice { get; set; }

    public string? ProductsSize { get; set; }

    public string? ProductsColor { get; set; }

    public string? CustomizationText { get; set; }

    public string? CustomizationFont { get; set; }

    public double? CustomizationFontSize { get; set; }

    public string? CustomizationImage { get; set; }

    public int? OrderTotalPrice { get; set; }

    public int? OrderDetailCounts { get; set; }

    public bool? OrderPickMyself { get; set; }
}
