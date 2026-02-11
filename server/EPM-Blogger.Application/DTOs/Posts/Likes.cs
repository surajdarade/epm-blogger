namespace EPM_Blogger.Application.DTOs.Posts
{
    public class LikeRequestDto
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
    }

    public class LikeResponseDto
    {
        public int PostId { get; set; }
        public int LikeCount { get; set; }
    }
}
