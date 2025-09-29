using System.ComponentModel.DataAnnotations;

namespace EmojiTrader.Models;

/// <summary>
/// Request model for team registration
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// Team identifier (letters, numbers, underscores, hyphens only)
    /// </summary>
    [Required]
    [StringLength(50, MinimumLength = 1)]
    [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "Team ID must contain only letters, numbers, underscores, and hyphens")]
    public string TeamId { get; init; } = string.Empty;
}