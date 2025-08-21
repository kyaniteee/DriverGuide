using DriverGuide.Domain.Models;

namespace DriverGuide.Domain.Interfaces;

public interface IQuestionFileRepository : IRepositoryBase<QuestionFile>
{
    Task<QuestionFile> GetByNameAsync(string questionFileName);
    Task<List<QuestionFile>> GetByNamesAsync(List<string> questionFileNames);
}
