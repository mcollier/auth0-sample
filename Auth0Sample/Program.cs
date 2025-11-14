using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

// Load configuration
var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

Console.WriteLine($"Environment: {environment}");
Console.WriteLine($"Current Directory: {Directory.GetCurrentDirectory()}");
Console.WriteLine($"Looking for: appsettings.{environment}.json");
Console.WriteLine();

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .Build();

var auth0Domain = configuration["Auth0:Domain"] ?? throw new InvalidOperationException("Auth0:Domain is not configured");
var auth0ClientId = configuration["Auth0:ClientId"] ?? throw new InvalidOperationException("Auth0:ClientId is not configured");
var auth0ClientSecret = configuration["Auth0:ClientSecret"] ?? throw new InvalidOperationException("Auth0:ClientSecret is not configured");
var auth0Audience = configuration["Auth0:Audience"] ?? throw new InvalidOperationException("Auth0:Audience is not configured");
var upstreamApiBaseUrl = configuration["UpstreamApi:BaseUrl"] ?? throw new InvalidOperationException("UpstreamApi:BaseUrl is not configured");

Console.WriteLine("Auth0 Sample - JWT Token Retrieval and API Authentication");
Console.WriteLine("==========================================================");
Console.WriteLine();

try
{
    // Step 1: Get JWT token from Auth0
    Console.WriteLine($"Requesting JWT token from Auth0 (Domain: {auth0Domain})...");
    
    using var authenticationApiClient = new AuthenticationApiClient(auth0Domain);
    
    var tokenRequest = new ClientCredentialsTokenRequest
    {
        ClientId = auth0ClientId,
        ClientSecret = auth0ClientSecret,
        Audience = auth0Audience
    };
    
    var tokenResponse = await authenticationApiClient.GetTokenAsync(tokenRequest);
    
    Console.WriteLine("✓ Successfully retrieved JWT token from Auth0");
    Console.WriteLine($"  Token Type: {tokenResponse.TokenType}");
    Console.WriteLine($"  Expires In: {tokenResponse.ExpiresIn} seconds");
    Console.WriteLine($"  Access Token Length: {tokenResponse.AccessToken.Length} characters");  
    Console.WriteLine();
    
    // Step 2: Call upstream API with JWT token
    Console.WriteLine($"Calling upstream API at: {upstreamApiBaseUrl}");
    
    using var httpClient = new HttpClient();
    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
    
    var apiResponse = await httpClient.GetAsync(upstreamApiBaseUrl);
    
    Console.WriteLine($"  Response Status: {(int)apiResponse.StatusCode} {apiResponse.StatusCode}");
    
    if (apiResponse.IsSuccessStatusCode)
    {
        var content = await apiResponse.Content.ReadAsStringAsync();
        Console.WriteLine($"  Response Content (first 200 chars): {(content.Length > 200 ? content[..200] + "..." : content)}");  

        Console.WriteLine();
        Console.WriteLine("✓ Successfully authenticated to upstream API");
    }
    else
    {
        var errorContent = await apiResponse.Content.ReadAsStringAsync();
        Console.WriteLine($"  Error Response: {errorContent}");
        Console.WriteLine();
        Console.WriteLine("✗ Failed to authenticate to upstream API");
    }
}
catch (Exception ex)
{
    Console.WriteLine();
    Console.WriteLine($"✗ Error: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"  Inner Exception: {ex.InnerException.Message}");
    }
    Console.WriteLine();
    Console.WriteLine("Please verify your configuration in appsettings.json");
    Environment.Exit(1);
}
