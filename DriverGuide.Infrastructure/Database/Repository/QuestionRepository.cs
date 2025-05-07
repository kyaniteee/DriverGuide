using DriverGuide.Domain.Enums;
using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DriverGuide.Infrastructure.Database;

public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
{
    public QuestionRepository(DriverGuideDbContext context) : base(context) { }

    public async Task<ICollection<Question>> GetRandomQuestionsQuantityByCategories(int quantity, params LicenseCategory[] categories)
    {
        var kategorie = categories.SelectMany(x => x.ToString().Split('_').Select(x => x.ToUpper()))
                                  .GroupBy(x => x)
                                  .Select(x => $"'{x.Key}'")
                                  .ToArray();
        var command = @$"
            select top {quantity} * from dbo.Questions 
            where exists(select value from string_split(Kategorie, ',') where value in ({string.Join(',', kategorie)})) 
            order by NEWID();";
        var result = await DBSet.FromSqlRaw(command).ToListAsync();

        return result;
    }
}
