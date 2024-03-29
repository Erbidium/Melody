﻿namespace Melody.Infrastructure.Data.DbEntites;

public class UserRole : IEquatable<UserRole>
{
    public long UserId { get; set; }
    public long RoleId { get; set; }

    public bool Equals(UserRole? other)
    {
        if (other is null) return false;
        return UserId == other.UserId && RoleId == other.RoleId;
    }
}