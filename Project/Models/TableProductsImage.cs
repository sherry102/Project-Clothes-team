using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class TableProductsImage
{
    public int ProductsImageId { get; set; }

    public string? ProductsImageName { get; set; }

    public int? ProductsId { get; set; }
}
