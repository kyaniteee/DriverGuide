using DriverGuide.Application.Commands;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DriverGuide.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionFileController(IMediator mediator, IQuestionFileRepository questionFileRepository, ILogger<QuestionFileController> logger) : ControllerBase
    {
        [HttpPost(nameof(UploadQuestionFile))]
        public async Task<ActionResult<QuestionFile>> UploadQuestionFile([FromForm] CreateQuestionFileCommand command)
        {
            var result = await mediator.Send(command);
            if (result == null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet(nameof(GetQuestionFile), Name = nameof(GetQuestionFile))]
        public async Task<ActionResult<QuestionFile>> GetQuestionFile([Required] int questionFileId)
        {
            var result = await questionFileRepository.GetByIdAsync(questionFileId);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet(nameof(GetQuestionFileByName), Name = nameof(GetQuestionFileByName))]
        public async Task<ActionResult<QuestionFile>> GetQuestionFileByName([Required] string questionFileName)
        {
            var result = await questionFileRepository.GetByNameAsync(questionFileName);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet(nameof(GetQuestionFilesByNames), Name = nameof(GetQuestionFilesByNames))]
        public async Task<ActionResult<IEnumerable<QuestionFile>>> GetQuestionFilesByNames([FromQuery][Required] List<string> questionFileNames)
        {
            if (questionFileNames == null || questionFileNames.Count == 0)
                return BadRequest("No file names provided.");

            var files = await questionFileRepository.GetByNamesAsync(questionFileNames);
            if (files.Count == 0)
                return NotFound();

            return Ok(files);
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
