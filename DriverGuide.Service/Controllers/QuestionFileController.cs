using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DriverGuide.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionFileController(IQuestionFileRepository questionFileRepository, ILogger<QuestionFileController> logger) : ControllerBase
    {
        [HttpGet("{questionFileId}", Name = nameof(GetQuestionFile))]
        public async Task<ActionResult<QuestionFile>> GetQuestionFile([Required] int questionFileId)
        {
            var result = await questionFileRepository.GetByIdAsync(questionFileId);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet(nameof(GetQuestionFiles), Name = nameof(GetQuestionFiles))]
        public async Task<ActionResult<IEnumerable<QuestionFile>>> GetQuestionFiles()
        {
            var result = await questionFileRepository.GetAllAsync();
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
