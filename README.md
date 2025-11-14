# auth0-sample

A .NET 8 console application demonstrating how to use the Auth0 SDK to retrieve a JWT token and authenticate to an upstream API.

## Overview

This sample application demonstrates:
- Using the Auth0 Authentication API to retrieve a JWT token via client credentials flow
- Including the JWT token in the Authorization header as a Bearer token when calling an upstream API
- Configuration-based setup for Auth0 tenant, API, and upstream API settings

## Prerequisites

- .NET 8.0 SDK or later
- An Auth0 tenant and API configured
- An upstream API that accepts Auth0 JWT tokens for authentication

## Configuration

The application uses `appsettings.json` for configuration. Before running the application, update the following settings:

### Auth0 Settings

- **Domain**: Your Auth0 tenant domain (e.g., `your-tenant.auth0.com`)
- **ClientId**: Your Auth0 application's client ID
- **ClientSecret**: Your Auth0 application's client secret
- **Audience**: Your Auth0 API identifier

### Upstream API Settings

- **BaseUrl**: The URL of the upstream API endpoint you want to call

### Example Configuration

```json
{
  "Auth0": {
    "Domain": "your-tenant.auth0.com",
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret",
    "Audience": "your-api-identifier"
  },
  "UpstreamApi": {
    "BaseUrl": "https://your-api.example.com/api"
  }
}
```

## Building the Application

Navigate to the `Auth0Sample` directory and build the application:

```bash
cd Auth0Sample
dotnet build
```

## Running the Application

After configuring `appsettings.json`, run the application:

```bash
cd Auth0Sample
dotnet run
```

The application will:
1. Request a JWT token from Auth0 using the client credentials flow
2. Display information about the retrieved token
3. Make an HTTP GET request to the configured upstream API with the JWT token in the Authorization header
4. Display the API response status and content

## Sample Output

```
Auth0 Sample - JWT Token Retrieval and API Authentication
==========================================================

Requesting JWT token from Auth0 (Domain: your-tenant.auth0.com)...
✓ Successfully retrieved JWT token from Auth0
  Token Type: Bearer
  Expires In: 86400 seconds
  Access Token (first 50 chars): eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Ik...

Calling upstream API at: https://your-api.example.com/api
  Response Status: 200 OK
  Response Content (first 200 chars): {"message":"Success"}...

✓ Successfully authenticated to upstream API
```

## Project Structure

- **Program.cs**: Main application logic for JWT token retrieval and API authentication
- **Auth0Sample.csproj**: Project file with NuGet package references
- **appsettings.json**: Configuration file for Auth0 and upstream API settings

## Dependencies

- **Auth0.AuthenticationApi** (v7.42.0): Auth0 SDK for .NET
- **Microsoft.Extensions.Configuration.Json** (v10.0.0): Configuration provider for JSON files
- **Microsoft.Extensions.Configuration.Binder** (v10.0.0): Configuration binding support

## Auth0 Setup

To use this sample, you need to:

1. Create an Auth0 account (if you don't have one)
2. Create an API in your Auth0 dashboard
3. Create a Machine-to-Machine (M2M) application
4. Authorize the M2M application to access your API
5. Copy the Domain, Client ID, Client Secret, and API Identifier to your `appsettings.json`

## Error Handling

The application includes error handling for:
- Missing or invalid configuration
- Auth0 authentication failures
- Upstream API call failures

Errors will be displayed in the console with relevant details to help troubleshoot issues.

## License

See the LICENSE file for details.