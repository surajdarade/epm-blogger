using EPM_Blogger.Application.DTOs.Authentication;
using EPM_Blogger.Application.DTOs.Users;
using EPM_Blogger.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPM_Blogger.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService userService)
        {
            _authService = userService;
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="dto">The user registration data.</param>
        /// <returns>A newly created user without password information.</returns>
        /// <response code="201">User successfully created.</response>
        /// <response code="400">Invalid input data.</response>
        /// <response code="409">Email or Username already exists.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _authService.CreateUserAsync(dto);
                return CreatedAtAction(nameof(Register), new { id = user.UserId }, user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpPost("login")]
        /// <summary>
        /// Logs a user into the system and issues JWT + Refresh token.
        /// </summary>
        /// <param name="dto">Login request (username/email + password)</param>
        /// <returns>Authentication tokens and user info</returns>
        /// <response code="200">Login successful</response>
        /// <response code="401">Invalid credentials</response>
        /// <response code="500">Unexpected error</response>
        [ProducesResponseType(typeof(AuthResponseDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _authService.LoginAsync(dto);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}
