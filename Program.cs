using EmojiTrader.Models;
using EmojiTrader.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    // Setup DI container
    var services = new ServiceCollection();

    // Configure HttpClient with factory and timeouts
    services.AddHttpClient<ApiService>(client =>
    {
        client.BaseAddress = new Uri("https://emoji-stock-exchange-2-h52e5.ondigitalocean.app");
        client.Timeout = TimeSpan.FromSeconds(30);
    });

    // Add Serilog
    services.AddLogging(builder => builder.AddSerilog());

    // Register services
    services.AddTransient<CredentialsManager>();

    var serviceProvider = services.BuildServiceProvider();
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("Starting Emoji Trader Bot for TEAM-AWESOME");

    // Get services from DI
    var apiService = serviceProvider.GetRequiredService<ApiService>();
    var credentialsManager = serviceProvider.GetRequiredService<CredentialsManager>();

    using var cts = new CancellationTokenSource();
    Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

    // Test API connection first
    var isConnected = await apiService.TestConnectionAsync(cts.Token);
    if (!isConnected)
    {
        logger.LogError("Failed to connect to API. Exiting.");
        return;
    }

    // Try to load existing credentials
    var credentials = await credentialsManager.LoadCredentialsAsync(cts.Token);

    if (credentials == null)
    {
        logger.LogInformation("No existing credentials found. Registering team TEAM-AWESOME");

        // Register team
        var registerResponse = await apiService.RegisterTeamAsync("TEAM-AWESOME", cts.Token);

        // Create and save credentials
        credentials = credentialsManager.CreateCredentials(registerResponse);
        await credentialsManager.SaveCredentialsAsync(credentials, cts.Token);

        logger.LogInformation("Team registration completed successfully");
    }
    else
    {
        logger.LogInformation("Loaded existing credentials for team {TeamId}", credentials.TeamId);
    }

    // Set authentication headers
    apiService.SetAuthHeaders(credentials.TeamId, credentials.ApiKey);

    logger.LogInformation("Bot initialized successfully with team {TeamId} and initial cash {InitialCash}",
        credentials.TeamId, credentials.InitialCash);

    // TODO: Add trading logic here
    logger.LogInformation("Bot is ready for trading operations");

    // Keep the application running
    Console.WriteLine("Press Ctrl+C to exit...");
    await Task.Delay(Timeout.Infinite, cts.Token);
}
catch (OperationCanceledException)
{
    Log.Information("Application shutdown requested");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
