using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class TableProduct
{
    public int ProductsId { get; set; }

    public string? ProductsName { get; set; }

    public int? ProductsPrice { get; set; }

    public string? ProductsType { get; set; }

    public string? ProductsCategory { get; set; }

    public string? ProductsSize { get; set; }

    public string? ProductsColor { get; set; }

    public string? ProductsDepiction { get; set; }

    public int? ProductsInventory { get; set; }
}
