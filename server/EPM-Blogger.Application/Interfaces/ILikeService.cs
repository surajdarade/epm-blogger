using EPM_Blogger.Application.DTOs.Parameters;
using EPM_Blogger.Application.DTOs.Posts;
namespace EPM_Blogger.Application.Interfaces
{
    public interface ILikeService
    {
        Task<LikeResponseDto> AddLikeAsync(LikeRequestDto dto);
        Task<LikeResponseDto> RemoveLikeAsync(LikeRequestDto dto);
        Task<int> GetAllLikeCountByPostIdAsync(int postId);
        Task<bool> HasUserLikedThePostAsync(LikeCheckQueryParameter qp);
    }
}
