using EPM_Blogger.Domain.Interfaces;
using EPM_Blogger.Domains.Models;
using EPM_Blogger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EPM_Blogger.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly BloggingDbContext _context;

        public AuthRepository(BloggingDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }

}
