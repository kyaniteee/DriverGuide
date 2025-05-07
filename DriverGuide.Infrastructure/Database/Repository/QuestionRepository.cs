using DriverGuide.Domain.Enums;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DriverGuide.Infrastructure.Database;

public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
{
    public QuestionRepository(DriverGuideDbContext context) : base(context) { }

    private string GetQuizQuestionsCommand(int quantity, bool isGeneral, int points, string categories)
        => $"select top {quantity} * from dbo.Questions where IsGeneral = {(isGeneral ? 1 : 0)} and Points = {points} and exists(select value from string_split(Kategorie, ',') where value in ({categories})) order by NEWID();";

    public async Task<ICollection<Question>> GetQuizQuestions(LicenseCategory category)
    {
        var categories = string.Join(",", category.ToString().Split('_').Select(x => $"'{x.ToUpper()}'"));

        var general3 = await DBSet
            .FromSqlRaw(GetQuizQuestionsCommand(10, true, 3, categories))
            .ToListAsync();

        var general2 = await DBSet
            .FromSqlRaw(GetQuizQuestionsCommand(6, true, 2, categories))
            .ToListAsync();

        var general1 = await DBSet
            .FromSqlRaw(GetQuizQuestionsCommand(4, true, 1, categories))
            .ToListAsync();

        var special3 = await DBSet
            .FromSqlRaw(GetQuizQuestionsCommand(6, false, 3, categories))
            .ToListAsync();

        var special2 = await DBSet
            .FromSqlRaw(GetQuizQuestionsCommand(4, false, 2, categories))
            .ToListAsync();

        var special1 = await DBSet
            .FromSqlRaw(GetQuizQuestionsCommand(2, false, 1, categories))
            .ToListAsync();

        var allQuestions = general3
            .Concat(general2)
            .Concat(general1)
            .Concat(special3)
            .Concat(special2)
            .Concat(special1)
            .ToList();

        allQuestions = allQuestions.OrderBy(q => Guid.NewGuid()).ToList();

        return allQuestions;
    }
}
