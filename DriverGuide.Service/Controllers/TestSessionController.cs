using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DriverGuide.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestSessionController(ITestSessionRepository testSessionReposiotory, ILogger<TestSessionController> logger) : ControllerBase
    {
        [HttpGet("{testSessionId}", Name = nameof(GetTestSession))]
        public async Task<ActionResult<QuestionAnswer>> GetTestSession([Required] int testSessionId)
        {
            var result = await testSessionReposiotory.GetByIdAsync(testSessionId);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }

}
