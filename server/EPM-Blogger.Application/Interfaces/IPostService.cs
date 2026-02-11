using EPM_Blogger.Application.DTOs.Parameters;
using EPM_Blogger.Application.DTOs.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPM_Blogger.Application.Interfaces
{
    public interface IPostService
    {
        Task<PostDto?> GetByIdAsync(int id);
        Task<IEnumerable<PostDto>> GetAllAsync();
        Task<IEnumerable<PostDto>> GetAllByUserIdAsync(int userId);
        Task<PostDto> CreateAsync(CreatePostDto dto);
        Task<bool> UpdateAsync(int id, UpdatePostDto dto);
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<PostDto>> GetAllPostsWithinRangeAsync(PostQueryParameters postQueryParameters);
    }
}
