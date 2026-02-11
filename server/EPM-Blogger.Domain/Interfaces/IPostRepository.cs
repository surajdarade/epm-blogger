using EPM_Blogger.Domains.Models;

namespace EPM_Blogger.Domain.Interfaces
{
    public interface IPostRepository
    {
        Task<Post?> GetByIdAsync(int id);
        Task<IEnumerable<Post>> GetAllAsync();
        Task<IEnumerable<Post>> GetAllByUserIdAsync(int userId);
        Task<Post> AddAsync(Post post);
        Task UpdateAsync(Post post);
        Task DeleteAsync(int id);
        Task<IEnumerable<Post>> GetAllPostsWithinRangeAsync(int page, int size);
    }
}
