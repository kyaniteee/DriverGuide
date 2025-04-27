using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;

namespace DriverGuide.Infrastructure.Database;

public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
{
    public QuestionRepository(DriverGuideDbContext context) : base(context) { }
}
