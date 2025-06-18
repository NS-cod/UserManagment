using Microsoft.AspNetCore.Mvc;
using Models.Dtos.User;
using Models.Requests;
using Models.Responses;
using Services.Interface;

namespace API.Controllers
{
    public class AuthController(IUserService userService, ILogger<AuthController> logger) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly ILogger<AuthController> _logger = logger;

        /// Autentica un usuario y devuelve un JWT token
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                _logger.LogInformation("Login attempt for UsernameOrEmail: {UsernameOrEmail}", loginDto.UsernameOrEmail);
                var result = await _userService.LoginAsync(loginDto);

                if (result == null)
                {
                    _logger.LogWarning("Failed login attempt for UsernameOrEmail: {UsernameOrEmail}", loginDto.UsernameOrEmail);
                    return Unauthorized("Invalid UsernameOrEmail or password");
                }

                _logger.LogInformation("Successful login for UsernameOrEmail: {UsernameOrEmail}", loginDto.UsernameOrEmail);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for UsernameOrEmail: {UsernameOrEmail}", loginDto.UsernameOrEmail);
                return StatusCode(500, "Internal server error");
            }
        }

        /// Registra un nuevo usuario
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserDto>> Register([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                _logger.LogInformation("Registration attempt for UsernameOrEmail: {UsernameOrEmail}", createUserDto.Username);
                var user = await _userService.CreateUserAsync(createUserDto);
                _logger.LogInformation("Successful registration for UsernameOrEmail: {UsernameOrEmail}", createUserDto.Username);
                return CreatedAtAction("GetUser", "Users", new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Registration validation error for UsernameOrEmail: {UsernameOrEmail}", createUserDto.Username);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for UsernameOrEmail: {UsernameOrEmail}", createUserDto.Username);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
