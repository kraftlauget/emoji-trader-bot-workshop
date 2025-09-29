using System.Text.Json;
using EmojiTrader.Models;
using Microsoft.Extensions.Logging;

namespace EmojiTrader.Services;

/// <summary>
/// Manages team credentials persistence to JSON file
/// </summary>
public class CredentialsManager
{
    private readonly ILogger<CredentialsManager> _logger;
    private const string CredentialsFile = "team-credentials.json";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Initializes a new instance of the CredentialsManager
    /// </summary>
    /// <param name="logger">Logger instance</param>
    /// <exception cref="ArgumentNullException">Thrown when logger is null</exception>
    public CredentialsManager(ILogger<CredentialsManager> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Loads team credentials from JSON file
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Team credentials if file exists and is valid, null otherwise</returns>
    public async Task<TeamCredentials?> LoadCredentialsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (!File.Exists(CredentialsFile))
            {
                _logger.LogInformation("Credentials file {FileName} not found", CredentialsFile);
                return null;
            }

            _logger.LogInformation("Loading credentials from {FileName}", CredentialsFile);
            var json = await File.ReadAllTextAsync(CredentialsFile, cancellationToken);

            if (string.IsNullOrWhiteSpace(json))
            {
                _logger.LogWarning("Credentials file {FileName} is empty", CredentialsFile);
                return null;
            }

            var credentials = JsonSerializer.Deserialize<TeamCredentials>(json, JsonOptions);

            if (credentials == null || string.IsNullOrEmpty(credentials.TeamId) || string.IsNullOrEmpty(credentials.ApiKey))
            {
                _logger.LogWarning("Invalid credentials in file {FileName}", CredentialsFile);
                return null;
            }

            _logger.LogInformation("Successfully loaded credentials for team {TeamId}", credentials.TeamId);
            return credentials;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Loading credentials was cancelled");
            return null;
        }
        catch (FileNotFoundException)
        {
            _logger.LogInformation("Credentials file {FileName} not found", CredentialsFile);
            return null;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Invalid JSON format in credentials file {FileName}", CredentialsFile);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error loading credentials from {FileName}", CredentialsFile);
            return null;
        }
    }

    /// <summary>
    /// Saves team credentials to JSON file
    /// </summary>
    /// <param name="credentials">Team credentials to save</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="ArgumentNullException">Thrown when credentials is null</exception>
    /// <exception cref="InvalidOperationException">Thrown when save operation fails</exception>
    public async Task SaveCredentialsAsync(TeamCredentials credentials, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(credentials);

        try
        {
            _logger.LogInformation("Saving credentials for team {TeamId} to {FileName}",
                credentials.TeamId, CredentialsFile);

            var json = JsonSerializer.Serialize(credentials, JsonOptions);
            await File.WriteAllTextAsync(CredentialsFile, json, cancellationToken);

            _logger.LogInformation("Successfully saved credentials to {FileName}", CredentialsFile);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Saving credentials was cancelled");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save credentials to {FileName}", CredentialsFile);
            throw new InvalidOperationException("Failed to save credentials", ex);
        }
    }

    /// <summary>
    /// Creates team credentials from registration response
    /// </summary>
    /// <param name="registerResponse">Registration response from API</param>
    /// <returns>Team credentials object</returns>
    /// <exception cref="ArgumentNullException">Thrown when registerResponse is null</exception>
    public TeamCredentials CreateCredentials(RegisterResponse registerResponse)
    {
        ArgumentNullException.ThrowIfNull(registerResponse);

        return new TeamCredentials
        {
            TeamId = registerResponse.TeamId,
            ApiKey = registerResponse.ApiKey,
            InitialCash = registerResponse.InitialCash
        };
    }
}