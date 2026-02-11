using EPM_Blogger.Domains.Models;

namespace EPM_Blogger.Domain.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<User> AddAsync(User user);
    }

}
