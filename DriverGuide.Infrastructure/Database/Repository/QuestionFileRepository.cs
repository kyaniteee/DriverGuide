using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DriverGuide.Infrastructure.Database;

public class QuestionFileRepository : RepositoryBase<QuestionFile>, IQuestionFileRepository
{
    public QuestionFileRepository(DriverGuideDbContext context) : base(context) { }

    public async Task<QuestionFile> GetByNameAsync(string questionFileName)
    {
        return await DBSet.FirstOrDefaultAsync(x => x.Name == questionFileName);
    }

    public async Task<List<QuestionFile>> GetByNamesAsync(List<string> questionFileNames)
    {
        if (questionFileNames == null || questionFileNames.Count == 0)
            return new List<QuestionFile>();

        return await DBSet.Where(qf => questionFileNames.Contains(qf.Name)).ToListAsync();
    }
}
