using EPM_Blogger.Application.DTOs.Parameters;
using EPM_Blogger.Application.DTOs.Posts;
using EPM_Blogger.Application.Interfaces;
using EPM_Blogger.Domain.Interfaces;
using EPM_Blogger.Domains.Models;
namespace EPM_Blogger.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _repo;

        public PostService(IPostRepository repo)
        {
            _repo = repo;
        }

        public async Task<PostDto?> GetByIdAsync(int id)
        {
            var post = await _repo.GetByIdAsync(id);
            if (post == null) return null;

            return new PostDto
            {
                Id = post.PostId,
                Title = post.Title,
                Content = post.Content,
                CreatedAt = post.CreatedAt.GetValueOrDefault(),
                UpdatedAt = post.UpdatedAt.GetValueOrDefault(),
                UserId = post.UserId
            };
        }

        public async Task<IEnumerable<PostDto>> GetAllAsync()
        {
            var posts = await _repo.GetAllAsync();
            return posts.Select(post => new PostDto
            {
                Id = post.PostId,
                Title = post.Title,
                Content = post.Content,
                CreatedAt = post.CreatedAt.GetValueOrDefault(),
                UpdatedAt = post.UpdatedAt.GetValueOrDefault(),
                UserId = post.UserId
            });
        }
        public async Task<IEnumerable<PostDto>> GetAllByUserIdAsync(int userId)
        {
            var posts = await _repo.GetAllByUserIdAsync(userId);
            return posts.Select(post => new PostDto
            {
                Id = post.PostId,
                Title = post.Title,
                Content = post.Content,
                CreatedAt = post.CreatedAt.GetValueOrDefault(),
                UpdatedAt = post.UpdatedAt.GetValueOrDefault(),
                UserId = post.UserId
            });
        }

        public async Task<PostDto> CreateAsync(CreatePostDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Title cannot be empty.");

            if (string.IsNullOrWhiteSpace(dto.Content))
                throw new ArgumentException("Content cannot be empty.");

            var newPost = new Post
            {
                Title = dto.Title,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UserId = dto.UserId
            };

            var created = await _repo.AddAsync(newPost);

            return new PostDto
            {
                Id = created.PostId,
                Title = created.Title,
                Content = created.Content,
                CreatedAt = created.CreatedAt.GetValueOrDefault(),
                UpdatedAt = created.UpdatedAt.GetValueOrDefault(),
                UserId = created.UserId
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdatePostDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Title = dto.Title;
            existing.Content = dto.Content;
            existing.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            await _repo.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<PostDto>> GetAllPostsWithinRangeAsync(PostQueryParameters postQP)
        {
            var posts = await _repo.GetAllPostsWithinRangeAsync(postQP.Page, postQP.Size);
            return posts.Select(post => new PostDto
            {
                Id = post.PostId,
                Title = post.Title,
                Content = post.Content,
                CreatedAt = post.CreatedAt.GetValueOrDefault(),
                UpdatedAt = post.UpdatedAt.GetValueOrDefault(),
                UserId = post.UserId
            });
        }
    }
}
