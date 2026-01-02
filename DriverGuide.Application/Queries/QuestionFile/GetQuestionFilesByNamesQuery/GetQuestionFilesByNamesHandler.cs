using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Queries;

public class GetQuestionFilesByNamesHandler : IRequestHandler<GetQuestionFilesByNamesQuery, List<QuestionFile>>
{
    private readonly IQuestionFileRepository _repository;

    public GetQuestionFilesByNamesHandler(IQuestionFileRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<QuestionFile>> Handle(GetQuestionFilesByNamesQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByNamesAsync(request.QuestionFileNames);
    }
}
