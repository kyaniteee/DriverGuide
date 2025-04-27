using DriverGuide.Domain.Models;
using DriverGuide.Domain.Interfaces;
using System.Linq.Expressions;

namespace DriverGuide.Infrastructure.Database;

public class QuestionSqlRepository : RepositoryBase, IQuestionRepository
{
    public QuestionSqlRepository(DriverGuideDbContext context) : base(context) { }

    public Task<Question> CreateAsync(Question dbRecord)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Question dbRecord)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<Question>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Question> GetAsync(Expression<Func<Question, bool>> filter, bool useNoTracking = false)
    {
        throw new NotImplementedException();
    }

    public Task<Question> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Question> UpdateAsync(Question dbRecord)
    {
        throw new NotImplementedException();
    }
}
