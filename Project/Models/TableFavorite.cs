using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class TableFavorite
{
    public int FavoriteId { get; set; }

    public int? MemberId { get; set; }

    public int? ProductsId { get; set; }

    public DateTime? FavoriteCreateDate { get; set; }
}
