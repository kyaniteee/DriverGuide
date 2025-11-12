using DriverGuide.Application.Models;
using DriverGuide.Application.Requests;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DriverGuide.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IMediator mediator, IUserRepository userReposiotory, ILogger<UserController> logger) : ControllerBase
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

    [HttpPost(nameof(Register))]
    public async Task<IActionResult> Register(CreateUserRequest request)
    {
        if (request == null)
            return BadRequest("Request cannot be null.");

        try
        {
            var result = await mediator.Send(request);
            return Ok(new { userId = result, message = "Rejestracja zakończona sukcesem" });
        }
        catch (FluentValidation.ValidationException ex)
        {
            // FluentValidation ValidationException
            var errorResponse = new ValidationErrorResponse
            {
                Message = "Wystąpiły błędy walidacji",
                Errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToList()
                    )
            };

            logger.LogWarning("Validation failed for registration: {@Errors}", errorResponse.Errors);
            return BadRequest(errorResponse);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during registration");
            return StatusCode(500, new { message = "Wystąpił nieoczekiwany błąd podczas rejestracji" });
        }
    }

    [HttpPost(nameof(Login))]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        if (request == null)
            return BadRequest("Request cannot be null.");

        try
        {
            var result = await mediator.Send(request);
            return Ok(new { token = result });
        }
        catch (Exception ex)
        {
            return Unauthorized(ex.Message);
        }
    }
}
