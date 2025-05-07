using DriverGuide.Domain.Enums;
using DriverGuide.Domain.Models;

namespace DriverGuide.Domain.Interfaces;
public interface IQuestionRepository : IRepositoryBase<Question>
{
    Task<ICollection<Question>> GetRandomQuestionsQuantityByCategories(int quantity, params LicenseCategory[] categories);
}
