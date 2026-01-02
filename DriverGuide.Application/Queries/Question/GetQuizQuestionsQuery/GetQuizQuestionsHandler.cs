using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Queries;

public class GetQuizQuestionsHandler : IRequestHandler<GetQuizQuestionsQuery, ICollection<Question>>
{
    private readonly IQuestionRepository _repository;

    public GetQuizQuestionsHandler(IQuestionRepository repository)
    {
        _repository = repository;
    }

    public async Task<ICollection<Question>> Handle(GetQuizQuestionsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetQuizQuestions(request.Category);
    }
}
