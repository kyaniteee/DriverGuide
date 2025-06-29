﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DriverGuide.Domain.Models;
public class User : IdentityUser<Guid>
{
    [Required]
    public override string? UserName { get; set; }

    [Required]
    public override string? Email { get; set; }

    public string? Avatar { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    public virtual ICollection<IdentityUserClaim<Guid>>? Claims { get; set; }
    public virtual ICollection<IdentityUserLogin<Guid>>? Logins { get; set; }
    public virtual ICollection<IdentityUserToken<Guid>>? Tokens { get; set; }
    public virtual ICollection<UserRole>? UserRoles { get; set; }
}
