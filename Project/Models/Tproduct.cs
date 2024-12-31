using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Project.Models;

public partial class Tproduct
{
    public int Pid { get; set; }
    [DisplayName("商品名稱")]
    public string Pname { get; set; } = null!;
    [DisplayName("商品價格")]
    public int Pprice { get; set; }
    [DisplayName("商品系列")]
    public string Ptype { get; set; } = null!;
    [DisplayName("商品分類")]
    public string Pcategory { get; set; } = null!;
    [DisplayName("商品尺寸")]
    public string Psize { get; set; } = null!;
    [DisplayName("商品顏色")]
    public string Pcolor { get; set; } = null!;
    [DisplayName("照片描述")]
    public string Pdepiction { get; set; } = null!;
    [DisplayName("商品庫存")]
    public int Pinventory { get; set; }
    [DisplayName("上傳日期")]
    public string Pdate { get; set; } = null!;
    [DisplayName("照片")]
    public string Pimage { get; set; } = null!;

    public bool IsHided { get; set; }

}
