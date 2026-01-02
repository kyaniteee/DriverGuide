using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Queries;

public class GetQuestionAnswersByTestSessionHandler : IRequestHandler<GetQuestionAnswersByTestSessionQuery, ICollection<QuestionAnswer>>
{
    private readonly IQuestionAnswerRepository _repository;

    public GetQuestionAnswersByTestSessionHandler(IQuestionAnswerRepository repository)
    {
        _repository = repository;
    }

    public async Task<ICollection<QuestionAnswer>> Handle(GetQuestionAnswersByTestSessionQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByTestSessionIdAsync(request.TestSessionId);
    }
}
