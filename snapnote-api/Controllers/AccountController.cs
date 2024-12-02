using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using NoteApp.Models;
using NoteApp.Repositories;

namespace NoteApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountRepository accountRepository, ILogger<AccountController> logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }

        [HttpOptions("register")]
        
        // POST: api/account/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _accountRepository.RegisterAsync(model);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User registered successfully with username: {Username}", model.Username);
                        return Ok(new { message = "User registered successfully" });
                    }

                    // Log validation errors from the registration process
                    foreach (var error in result.Errors)
                    {
                        _logger.LogWarning("Registration error: {ErrorDescription}", error.Description);
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    _logger.LogWarning("Invalid registration model state for user: {Username}", model.Username);
                    foreach (var key in ModelState.Keys)
                    {
                        foreach (var error in ModelState[key].Errors)
                        {
                            _logger.LogWarning("Validation error for key {Key}: {Error}", key, error.ErrorMessage);
                        }
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration for user: {Username}", model.Username);
                return StatusCode(500, "Internal server error");
            }
        }


        // POST: api/account/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _accountRepository.LoginAsync(model);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in successfully with username: {Username}", model.Username);
                        return Ok(new { message = "Login successful" });
                    }

                    // Log the invalid login attempt
                    _logger.LogWarning("Invalid login attempt for user: {Username}", model.Username);
                    return Unauthorized(new { message = "Invalid login attempt" });
                }
                else
                {
                    _logger.LogWarning("Invalid login model state for user: {Username}", model.Username);
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for user: {Username}", model.Username);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // POST: api/account/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _accountRepository.LogoutAsync();
                _logger.LogInformation("User logged out successfully.");
                return Ok(new { message = "Logout successful" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during logout.");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}