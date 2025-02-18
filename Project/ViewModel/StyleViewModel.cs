﻿namespace Project.ViewModel
{
    public class StyleViewModel
    {
        public int Sid { get; set; }
        public string? SImg { get; set; }  
        public string SimgCategory { get; set; } = null!;

        public List<ProductViewModel> Products { get; set; } 
    }
}
