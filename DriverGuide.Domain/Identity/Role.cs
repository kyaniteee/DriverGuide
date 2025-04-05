using Microsoft.AspNetCore.Identity;

namespace DriverGuide.Domain.Identity
{
    public class Role : IdentityRole<Guid>
    {
        public Role(string name)
        {
            Name = name;
        }

        public virtual ICollection<UserRole>? UserRoles { get; set; }
    }
}
