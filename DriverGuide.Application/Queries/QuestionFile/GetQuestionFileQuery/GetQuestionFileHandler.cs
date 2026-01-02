using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Queries;

public class GetQuestionFileHandler : IRequestHandler<GetQuestionFileQuery, QuestionFile?>
{
    private readonly IQuestionFileRepository _repository;

    public GetQuestionFileHandler(IQuestionFileRepository repository)
    {
        _repository = repository;
    }

    public async Task<QuestionFile?> Handle(GetQuestionFileQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.QuestionFileId);
    }
}
