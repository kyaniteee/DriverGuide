using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DriverGuide.Infrastructure.Database;

public class QuestionAnswerRepository : RepositoryBase<QuestionAnswer>, IQuestionAnswerRepository
{
    public QuestionAnswerRepository(DriverGuideDbContext context) : base(context) { }

    public async Task<ICollection<QuestionAnswer>> GetByTestSessionIdAsync(string testSessionId)
    {
        return await Context.QuestionAnswers!
            .Where(qa => qa.TestSessionId == testSessionId)
            .OrderBy(qa => qa.StartDate)
            .ToListAsync();
    }

    public async Task<int> SaveAnswerAsync(QuestionAnswer questionAnswer)
    {
        if (string.IsNullOrEmpty(questionAnswer.QuestionAnswerId))
            questionAnswer.QuestionAnswerId = Guid.NewGuid().ToString();

        if (questionAnswer.StartDate == default)
            questionAnswer.StartDate = DateTimeOffset.Now;

        questionAnswer.EndDate = DateTimeOffset.Now;

        Context.QuestionAnswers!.Add(questionAnswer);
        return await Context.SaveChangesAsync();
    }

    public async Task<int> GetCorrectAnswersCountAsync(string testSessionId)
    {
        return await Context.QuestionAnswers!
            .Where(qa => qa.TestSessionId == testSessionId &&
                   qa.UserQuestionAnswer == qa.CorrectQuestionAnswer)
            .CountAsync();
    }

    public async Task<int> GetTotalPointsAsync(string testSessionId)
    {
        var answers = await Context.QuestionAnswers!
            .Where(qa => qa.TestSessionId == testSessionId)
            .ToListAsync();

        int totalPoints = 0;

        foreach (var answer in answers)
        {
            if (answer.UserQuestionAnswer == answer.CorrectQuestionAnswer &&
                int.TryParse(answer.QuestionId, out int questionId))
            {
                var question = await Context.Questions!.FindAsync(questionId);
                if (question != null)
                {
                    totalPoints += question.Points;
                }
            }
        }

        return totalPoints;
    }
}