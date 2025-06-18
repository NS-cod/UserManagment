using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos.User;
using Services.Interface;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController(IUserService userService, ILogger<UsersController> logger) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly ILogger<UsersController> _logger = logger;

        /// Obtiene todos los usuarios
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            try
            {
                _logger.LogInformation("Getting all users");
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users");
                return StatusCode(500, "Internal server error");
            }
        }

        /// Obtiene un usuario por ID
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            try
            {
                _logger.LogInformation("Getting user with ID: {UserId}", id);
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found", id);
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user with ID: {UserId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// Crea un nuevo usuario
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                _logger.LogInformation("Creating new user with username: {Username}", createUserDto.Username);
                var user = await _userService.CreateUserAsync(createUserDto);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation error creating user");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, "Internal server error");
            }
        }

        /// Actualiza un usuario existente
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                _logger.LogInformation("Updating user with ID: {UserId}", id);
                var user = await _userService.UpdateUserAsync(id, updateUserDto);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "User with ID {UserId} not found for update", id);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation error updating user");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with ID: {UserId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// Elimina un usuario (soft delete)
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                _logger.LogInformation("Deleting user with ID: {UserId}", id);
                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "User with ID {UserId} not found for deletion", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID: {UserId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// Busca usuarios por término de búsqueda
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
        public async Task<ActionResult<IEnumerable<UserDto>>> SearchUsers([FromQuery] string searchTerm)
        {
            try
            {
                _logger.LogInformation("Searching users with term: {SearchTerm}", searchTerm);
                var users = await _userService.SearchUsersAsync(searchTerm);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching users");
                return StatusCode(500, "Internal server error");
            }
        }

        /// Obtiene usuarios por rol
       /* [HttpGet("by-role/{role}")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersByRole(string role)
        {
            try
            {
               _logger.LogInformation("Getting users by role: {Role}", role);
                var users = await _userService.GetUsersByRoleAsync(role);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users by role");
                return StatusCode(500, "Internal server error");
            }
        }*/

        /// Obtiene usuarios paginados
        [HttpGet("paged")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetPagedUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                _logger.LogInformation("Getting paged users: Page {PageNumber}, Size {PageSize}", pageNumber, pageSize);
                var users = await _userService.GetPagedUsersAsync(pageNumber, pageSize);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged users");
                return StatusCode(500, "Internal server error");
            }
        }

        /// Verifica si un username está disponible
        [HttpGet("check-username/{username}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<ActionResult<bool>> CheckUsernameAvailability(string username, [FromQuery] int? excludeUserId = null)
        {
            try
            {
                var isAvailable = await _userService.IsUsernameAvailableAsync(username, excludeUserId);
                return Ok(isAvailable);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking username availability");
                return StatusCode(500, "Internal server error");
            }
        }

        /// Verifica si un email está disponible
        [HttpGet("check-email/{email}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<ActionResult<bool>> CheckEmailAvailability(string email, [FromQuery] int? excludeUserId = null)
        {
            try
            {
                var isAvailable = await _userService.IsEmailAvailableAsync(email, excludeUserId);
                return Ok(isAvailable);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking email availability");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
