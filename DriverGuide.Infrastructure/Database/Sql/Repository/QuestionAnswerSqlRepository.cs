using DriverGuide.Domain.Models;
using DriverGuide.Domain.Interfaces;
using System.Linq.Expressions;

namespace DriverGuide.Infrastructure.Database;

public class QuestionAnswerSqlRepository : RepositoryBase, IQuestionAnswerRepository
{
    public QuestionAnswerSqlRepository(DriverGuideDbContext context) : base(context) { }

    public Task<QuestionAnswer> CreateAsync(QuestionAnswer dbRecord)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(QuestionAnswer dbRecord)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<QuestionAnswer>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<QuestionAnswer> GetAsync(Expression<Func<QuestionAnswer, bool>> filter, bool useNoTracking = false)
    {
        throw new NotImplementedException();
    }

    public Task<QuestionAnswer> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<QuestionAnswer> UpdateAsync(QuestionAnswer dbRecord)
    {
        throw new NotImplementedException();
    }
}
