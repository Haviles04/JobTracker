using JobTracker.Models;
using JobTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(UserService userService) : ControllerBase
    {
        private readonly UserService _userService = userService;

        [HttpPost("login")]
        public async Task<ActionResult<EmployeeDTO>> Login([FromBody] LoginDTO loginInfo)
        {
            try
            {
                var employee = await _userService.Login(loginInfo);
                return Ok(employee);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<EmployeeDTO>> Register([FromBody] RegisterDTO registerInfo)
        {
            try
            {
                var employee = await _userService.Register(registerInfo);
                return Ok(employee);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}