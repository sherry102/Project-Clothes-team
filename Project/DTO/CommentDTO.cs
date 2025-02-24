namespace Project.DTO
{
    public class CommentDTO
    {
        public int Mid { get; set; }

        public string Mname { get; set; } = null!;

        public int Pid { get; set; }

        public string Comment { get; set; } = null!;

        public DateTime ComCreateDate { get; set; }

        public string? ComImage1 { get; set; }

        public string? ComImage2 { get; set; }

        public List<IFormFile> Images { get; set; }  // 存放上傳的圖片
    }
}
