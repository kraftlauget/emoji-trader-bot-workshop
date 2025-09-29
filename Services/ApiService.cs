using System.Net.Http.Json;
using System.Text.Json;
using EmojiTrader.Models;
using Microsoft.Extensions.Logging;

namespace EmojiTrader.Services;

/// <summary>
/// Service for interacting with the Emoji Stock Exchange API
/// </summary>
public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiService> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Initializes a new instance of the ApiService
    /// </summary>
    /// <param name="httpClient">HTTP client for API calls</param>
    /// <param name="logger">Logger instance</param>
    public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Registers a team with the exchange
    /// </summary>
    /// <param name="teamId">Team identifier to register</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Registration response with API key</returns>
    /// <exception cref="HttpRequestException">Thrown when registration fails</exception>
    /// <exception cref="ArgumentNullException">Thrown when teamId is null</exception>
    public async Task<RegisterResponse> RegisterTeamAsync(string teamId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(teamId);

        _logger.LogInformation("Registering team {TeamId}", teamId);

        var request = new RegisterRequest { TeamId = teamId };
        var response = await _httpClient.PostAsJsonAsync("/v1/register", request, JsonOptions, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError("Registration failed for team {TeamId}: {Status} - {Error}",
                teamId, response.StatusCode, errorContent);
            throw new HttpRequestException($"Registration failed: {response.StatusCode}");
        }

        var registerResponse = await response.Content.ReadFromJsonAsync<RegisterResponse>(JsonOptions, cancellationToken);

        if (registerResponse == null)
        {
            throw new InvalidOperationException("Failed to deserialize registration response");
        }

        _logger.LogInformation("Successfully registered team {TeamId} with initial cash {InitialCash}",
            registerResponse.TeamId, registerResponse.InitialCash);

        return registerResponse;
    }

    /// <summary>
    /// Tests API connectivity with health check
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if API is healthy</returns>
    public async Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Testing API connection to {BaseUrl}", _httpClient.BaseAddress);
            var response = await _httpClient.GetAsync("/healthz", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("API connection successful");
                return true;
            }

            _logger.LogWarning("API health check failed: {Status}", response.StatusCode);
            return false;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("API connection test was cancelled");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect to API");
            return false;
        }
    }

    /// <summary>
    /// Sets authentication headers for subsequent API calls
    /// </summary>
    /// <param name="teamId">Team identifier</param>
    /// <param name="apiKey">API key from registration</param>
    /// <exception cref="ArgumentNullException">Thrown when teamId or apiKey is null</exception>
    public void SetAuthHeaders(string teamId, string apiKey)
    {
        ArgumentNullException.ThrowIfNull(teamId);
        ArgumentNullException.ThrowIfNull(apiKey);

        // Remove existing auth headers specifically instead of clearing all
        _httpClient.DefaultRequestHeaders.Remove("X-Team-Id");
        _httpClient.DefaultRequestHeaders.Remove("X-Api-Key");

        _httpClient.DefaultRequestHeaders.Add("X-Team-Id", teamId);
        _httpClient.DefaultRequestHeaders.Add("X-Api-Key", apiKey);

        _logger.LogInformation("Set authentication headers for team {TeamId}", teamId);
    }
}