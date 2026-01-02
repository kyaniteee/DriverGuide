using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Queries;

public class GetQuestionFileByNameHandler : IRequestHandler<GetQuestionFileByNameQuery, QuestionFile?>
{
    private readonly IQuestionFileRepository _repository;

    public GetQuestionFileByNameHandler(IQuestionFileRepository repository)
    {
        _repository = repository;
    }

    public async Task<QuestionFile?> Handle(GetQuestionFileByNameQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByNameAsync(request.QuestionFileName);
    }
}
