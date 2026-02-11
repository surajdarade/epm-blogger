using EPM_Blogger.Domain.Interfaces;
using EPM_Blogger.Domains.Models;
using EPM_Blogger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EPM_Blogger.Infrastructure.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly BloggingDbContext _context;

        public LikeRepository(BloggingDbContext context)
        {
            _context = context;
        }

        public async Task<Like?> GetUserLikeAsync(int userId, int postId)
        {
            return await _context.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId);
        }

        public async Task AddLikeAsync(Like like)
        {
            _context.Likes.Add(like);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLikeAsync(Like like)
        {
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetLikeCountAsync(int postId)
        {
            return await _context.Likes.CountAsync(l => l.PostId == postId);
        }

        public async Task<bool> HasUserLikedThePostAsync(int userId, int postId)
        {
            return await _context.Likes.Where(x => x.PostId == postId && x.UserId == userId).AnyAsync();
        }
    }
}
