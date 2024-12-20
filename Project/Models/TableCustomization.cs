using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class TableCustomization
{
    public int CustomizationId { get; set; }

    public string? CustomizationName { get; set; }

    public int? CustomizationPrice { get; set; }

    public string? CustomizationType { get; set; }

    public string? CustomizationCategory { get; set; }

    public string? CustomizationSize { get; set; }

    public string? CustomizationColor { get; set; }

    public string? CustomizationText { get; set; }

    public string? CustomizationFont { get; set; }

    public double? CustomizationFontSize { get; set; }

    public string? CustomizationImage { get; set; }
}
