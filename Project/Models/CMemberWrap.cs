﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Project.Models
{
    public class CMemberWrap
    {
        private Tmember _member = null;
        public Tmember member
        {
            get { return _member; }
            set { _member = value; }
        }
        public CMemberWrap()
        {
            _member = new Tmember();
            _member.Mpoints = 0; // 設定初始點數為 0
        }
        public int Mid
        {
            get { return _member.Mid; }
            set { _member.Mid = value; }
        }
        [Required(ErrorMessage = "暱稱必須填寫")]
        [DisplayName("會員暱稱")]
        public string Mname
        {
            get { return _member.Mname; }
            set { _member.Mname = value; }
        }
        [DisplayName("性別")]
        public int Mgender
        {
            get { return _member.Mgender; }
            set { _member.Mgender = value; }
        }

        // 新增這個屬性來處理性別顯示
        public string GenderText
        {
            get
            {
                return _member.Mgender == 0 ? "男性" : "女性";
            }
        }

        [DisplayName("帳號")]
        public string? Maccount
        {
            get { return _member.Maccount; }
            set { _member.Maccount = value; }
        }
        [DisplayName("密碼")]
        public string? Mpassword
        {
            get { return _member.Mpassword; }
            set { _member.Mpassword = value; }
        }
        [DisplayName("電子郵件")]
        public string? Memail
        {
            get { return _member.Memail; }
            set { _member.Memail = value; }
        }
        [DisplayName("地址")]
        public string? Maddress
        {
            get { return _member.Maddress; }
            set { _member.Maddress = value; }
        }
        [DisplayName("生日")]
        public string? Mbirthdays
        {
            get { return _member.Mbirthday.ToString("yyyy-MM-dd"); }
            set { _member.Mbirthday = DateOnly.Parse(value); }
        }
        [DisplayName("電話")]
        public string? Mphone
        {
            get { return _member.Mphone; }
            set { _member.Mphone = value; }
        }
        [DisplayName("點數")]
        public int Mpoints
        {
            get { return _member.Mpoints; }
            set { _member.Mpoints = value; }
        }
     
        [DisplayName("權限")]
        public int Mpermissions
        {
            get { return _member.Mpermissions; }
            set { _member.Mpermissions = value; }
        }

             public string PermissionText
             {
                get
                {
                    return _member.Mpermissions switch
                    {
                        0 => "一般訪客",
                        1 => "黑名單",
                        2 => "會員",
                        3 => "VIP會員",
                        11 => "一般員工",
                        21 => "副店長",
                        31 => "店長",
                        51 => "經理",
                        61=> "區經理",
                        71=> "執行長",
                        81 => "副總",
                        91 => "負責人",
                     };
                }
             }
        
        [DisplayName("創建日期")]
        public string? McreatedDate
        {
            get { return _member.McreatedDate.ToString("yyyy-MM-dd HH:mm:ss"); }
            set { _member.McreatedDate = DateTime.Parse(value); }
        }
        [DisplayName("會員照片")]
        public string? Mphoto
        {
            get { return _member.Mphoto; }
            set { _member.Mphoto = value; }
        }
        [DisplayName("黑名單")]
        public int MBlackList
        {
            get { return _member.MBlackList; }
            set { _member.MBlackList = value; }
        }

        public IFormFile photoPath { get; set; }
    }
}
    

