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

        [HttpPost(nameof(UploadQuestionFile))]
        public async Task<ActionResult<QuestionFile>> UploadQuestionFile([FromForm] CreateQuestionFileCommand command)
        {
            var result = await mediator.Send(command);
            if (result == null)
                return BadRequest(result);

            return Ok(result);
        }

        //[HttpPost(nameof(UploadFromPath))]
        //public async Task<IActionResult> UploadFromPath([FromBody] UploadFromPathRequest request)
        //{
        //    if (!Directory.Exists(request.DirectoryPath))
        //        return BadRequest("Folder nie istnieje.");

        //    var files = Directory.GetFiles(request.DirectoryPath);
        //    if (files.Length == 0)
        //        return BadRequest("Brak plików w folderze.");

        //    foreach (var filePath in files)
        //    {
        //        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
        //        var command = new CreateQuestionFileCommand
        //        {
        //            File = fileBytes,
        //            FileName = Path.GetFileName(filePath)
        //        };

        //        await mediator.Send(command);
        //    }

        //    return Ok($"Wysłano {files.Length} plików.");
        //}


    }
}
