using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DriverGuide.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserRepository userReposiotory, ILogger<UserController> logger) : ControllerBase
{
    [HttpGet("{userGuid}", Name = nameof(GetUser))]
    public async Task<ActionResult<User>> GetUser([Required] string userGuid)
    {
        if (string.IsNullOrWhiteSpace(userGuid))
            throw new ArgumentNullException(nameof(userGuid));

        var result = await userReposiotory.GetByGuidAsync(Guid.Parse(userGuid));
        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
