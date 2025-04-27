using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;

namespace DriverGuide.Infrastructure.Database;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(DriverGuideDbContext context) : base(context) { }

}
