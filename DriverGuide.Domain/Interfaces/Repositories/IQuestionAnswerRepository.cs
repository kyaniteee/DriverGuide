using DriverGuide.Domain.Models;

namespace DriverGuide.Domain.Interfaces;

public interface IQuestionAnswerRepository : IRepositoryBase<QuestionAnswer>
{
    Task<ICollection<QuestionAnswer>> GetByTestSessionIdAsync(string testSessionId);
    Task<int> SaveAnswerAsync(QuestionAnswer questionAnswer);
    Task<int> GetCorrectAnswersCountAsync(string testSessionId);
    Task<int> GetTotalPointsAsync(string testSessionId);
}
