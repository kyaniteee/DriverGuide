using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;

namespace DriverGuide.Infrastructure.Database;

public class QuestionFileRepository : RepositoryBase<QuestionFile>, IQuestionFileRepository
{
    public QuestionFileRepository(DriverGuideDbContext context) : base(context) { }

}
