using EPM_Blogger.Domain.Interfaces;
using EPM_Blogger.Domains.Models;
using EPM_Blogger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EPM_Blogger.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly BloggingDbContext _context;

        public PostRepository(BloggingDbContext context)
        {
            _context = context;
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.Likes)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.PostId == id);
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _context.Posts
                .Include(p => p.Likes)
                .Include(p => p.Tags)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetAllByUserIdAsync(int userId)
        {
            return await _context.Posts
                .Where(p => p.UserId == userId)
                .Include(p => p.Likes)
                .Include(p => p.Tags)
                .ToListAsync();
        }

        public async Task<Post> AddAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task UpdateAsync(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Post>> GetAllPostsWithinRangeAsync(int page, int size)
        {
            return await _context.Posts
                .Skip((page-1)*size)
                .Take(size)
                .Include(p => p.Likes)
                .Include(p => p.Tags)
                .ToListAsync();
        }
    }
}
