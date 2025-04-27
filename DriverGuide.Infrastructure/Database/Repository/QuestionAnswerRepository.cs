using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;

namespace DriverGuide.Infrastructure.Database;

public class QuestionAnswerRepository : RepositoryBase<QuestionAnswer>, IQuestionAnswerRepository
{
    public QuestionAnswerRepository(DriverGuideDbContext context) : base(context) { }

}
