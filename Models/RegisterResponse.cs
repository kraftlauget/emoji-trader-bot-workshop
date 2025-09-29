using System.ComponentModel.DataAnnotations;

namespace EmojiTrader.Models;

/// <summary>
/// Response model from team registration
/// </summary>
public class RegisterResponse
{
    /// <summary>
    /// Registered team identifier
    /// </summary>
    [Required]
    public string TeamId { get; init; } = string.Empty;

    /// <summary>
    /// API key for authentication
    /// </summary>
    [Required]
    public string ApiKey { get; init; } = string.Empty;

    /// <summary>
    /// Initial cash balance
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal InitialCash { get; init; }
}