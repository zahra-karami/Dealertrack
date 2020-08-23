using System;
using System.Threading;
using System.Threading.Tasks;
using DealerTrack.Web.Models;
using DealerTrack.Web.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DealerTrack.Web.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<VehicleSaleController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<VehicleSaleController> logger , IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(UserAuthenticateModel auth)
        {
            try
            {
                var response = await _userService.Authenticate(auth.Username, auth.Password);
                if (response.IsSucceeded) return Ok(response);

                _logger.LogCritical($"Error on User.Register: {response.ResponseMessage}");
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error on User.SignIn");
                return StatusCode(500, $"Internal server error: {ex}");

            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserModel user)
        {
            try
            {
                var response = await _userService.RegisterUser(user);
                if (response.IsSucceeded) return Ok();

                _logger.LogCritical($"Error on User.Register: {response.ResponseMessage}");
                return BadRequest(response);
               
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error on User.Register");
                return StatusCode(500, $"Internal server error: {ex}");

            }
        }
    }
}
