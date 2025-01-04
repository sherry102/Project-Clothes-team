using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Models;

public partial class Tmember
{
    public int Mid { get; set; }
    [Display(Name = "姓名")]
    public string Mname { get; set; } = null!;
    [Display(Name = "性別")]
    public int Mgender { get; set; }
    [Display(Name = "帳號")]
    public string Maccount { get; set; } = null!;
    [Display(Name = "密碼")]
    public string Mpassword { get; set; } = null!;
    [Display(Name = "電子郵件")]
    public string Memail { get; set; } = null!;
    [Display(Name = "地址")]
    public string Maddress { get; set; } = null!;
    [Display(Name = "生日")]
    public DateOnly Mbirthday { get; set; }
    [Display(Name = "電話")]
    public string Mphone { get; set; } = null!;
    [Display(Name = "點數")]
    public int Mpoints { get; set; }
    [Display(Name = "權限")]
    public int Mpermissions { get; set; }
    [Display(Name = "創建日期")]
    public DateTime McreatedDate { get; set; }
    [Display(Name = "會員照片")]
    public string? Mphoto { get; set; }
    public bool Mishided { get; set; }
}
