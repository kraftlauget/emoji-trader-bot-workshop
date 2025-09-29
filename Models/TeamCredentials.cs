using System.ComponentModel.DataAnnotations;

namespace EmojiTrader.Models;

/// <summary>
/// Represents team credentials for API authentication
/// </summary>
public class TeamCredentials
{
    /// <summary>
    /// Team identifier for authentication
    /// </summary>
    [Required]
    [StringLength(50, MinimumLength = 1)]
    [RegularExpression(@"^[a-zA-Z0-9_-]+$")]
    public string TeamId { get; init; } = string.Empty;

    /// <summary>
    /// API key received from registration
    /// </summary>
    [Required]
    public string ApiKey { get; init; } = string.Empty;

    /// <summary>
    /// Initial cash balance from registration
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal InitialCash { get; init; }
}