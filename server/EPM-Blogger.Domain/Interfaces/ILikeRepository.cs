
using EPM_Blogger.Domains.Models;

namespace EPM_Blogger.Domain.Interfaces
{
    public interface ILikeRepository
    {
        Task<Like?> GetUserLikeAsync(int userId, int postId);
        Task AddLikeAsync(Like like);
        Task DeleteLikeAsync(Like like);
        Task<int> GetLikeCountAsync(int postId);
        Task<bool> HasUserLikedThePostAsync(int userId, int postId);
    }
}
