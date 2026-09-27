namespace RMS.Persistence;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public int RoleId { get; set; }

    public bool IsActive { get; set; }

    public int FailedAttempts { get; set; }

    public bool IsLocked { get; set; }

    public string? LockReason { get; set; }

    public DateTime? LockedAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public DateTime? PasswordChangedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public Roles Role { get; set; } = null!;
}