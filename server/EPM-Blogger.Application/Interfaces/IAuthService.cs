using EPM_Blogger.Application.DTOs.Authentication;
using EPM_Blogger.Application.DTOs.Users;

namespace EPM_Blogger.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> CreateUserAsync(RegisterUserDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}
