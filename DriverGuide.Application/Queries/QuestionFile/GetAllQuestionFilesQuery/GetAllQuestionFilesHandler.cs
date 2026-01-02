using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Queries;

public class GetAllQuestionFilesHandler : IRequestHandler<GetAllQuestionFilesQuery, ICollection<QuestionFile>>
{
    private readonly IQuestionFileRepository _repository;

    public GetAllQuestionFilesHandler(IQuestionFileRepository repository)
    {
        _repository = repository;
    }

    public async Task<ICollection<QuestionFile>> Handle(GetAllQuestionFilesQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync();
    }
}
