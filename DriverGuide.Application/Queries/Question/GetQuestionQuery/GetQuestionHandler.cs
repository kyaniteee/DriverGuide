using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Queries;

public class GetQuestionHandler : IRequestHandler<GetQuestionQuery, Question?>
{
    private readonly IQuestionRepository _repository;

    public GetQuestionHandler(IQuestionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Question?> Handle(GetQuestionQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.QuestionId);
    }
}
