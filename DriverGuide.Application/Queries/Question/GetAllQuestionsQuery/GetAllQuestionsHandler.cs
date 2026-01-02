using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Queries;

public class GetAllQuestionsHandler : IRequestHandler<GetAllQuestionsQuery, ICollection<Question>>
{
    private readonly IQuestionRepository _repository;

    public GetAllQuestionsHandler(IQuestionRepository repository)
    {
        _repository = repository;
    }

    public async Task<ICollection<Question>> Handle(GetAllQuestionsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync();
    }
}
