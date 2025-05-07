using DriverGuide.Domain.Enums;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DriverGuide.Infrastructure.Database;

public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
{
    public QuestionRepository(DriverGuideDbContext context) : base(context) { }

    public async Task<ICollection<Question>> GetQuizQuestions(LicenseCategory category)
    {
        var categories = string.Join(",", category.ToString().Split('_').Select(x => $"'{x.ToUpper()}'"));

        var general3 = await DBSet
            .FromSqlRaw($"select top 10 * from dbo.Questions where IsGeneral = 1 and Points = 3 and exists(select value from string_split(Kategorie, ',') where value in ({categories})) order by NEWID();")
            .ToListAsync();

        var general2 = await DBSet
            .FromSqlRaw($"select top 6 * from dbo.Questions where IsGeneral = 1 and Points = 2 and exists(select value from string_split(Kategorie, ',') where value in ({categories})) order by NEWID();")
            .ToListAsync();

        var general1 = await DBSet
            .FromSqlRaw($"select top 4 * from dbo.Questions where IsGeneral = 1 and Points = 1 and exists(select value from string_split(Kategorie, ',') where value in ({categories})) order by NEWID();")
            .ToListAsync();

        var special3 = await DBSet
            .FromSqlRaw($"select top 6 * from dbo.Questions where IsGeneral = 0 and Points = 3 and exists(select value from string_split(Kategorie, ',') where value in ({categories})) order by NEWID();")
            .ToListAsync();

        var special2 = await DBSet
            .FromSqlRaw($"select top 4 * from dbo.Questions where IsGeneral = 0 and Points = 2 and exists(select value from string_split(Kategorie, ',') where value in ({categories})) order by NEWID();")
            .ToListAsync();

        var special1 = await DBSet
            .FromSqlRaw($"select top 2 * from dbo.Questions where IsGeneral = 0 and Points = 1 and exists(select value from string_split(Kategorie, ',') where value in ({categories})) order by NEWID();")
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
