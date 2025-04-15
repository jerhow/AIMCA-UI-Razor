using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http; // Required for HttpClient
using System.Net.Http.Headers; // Required for AuthenticationHeaderValue
using System.Text; // Required for StringContent encoding
using System.Text.Json; // Required for JsonSerializer
using MedicalCodingAssistant.UI.Models; // Your models namespace
using Microsoft.Extensions.Configuration; // Required for IConfiguration
using System.Threading.Tasks; // Required for async methods

namespace MedicalCodingAssistant.UI.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory; // Best practice for HttpClient
    private readonly IConfiguration _configuration; // To read settings

    // Bind properties from the form post
    [BindProperty]
    public string Query { get; set; } = "chronic bronchitis and emphysema"; // Default value

    [BindProperty]
    public int MaxSqlResults { get; set; } = 3; // Default value

    // Property to hold the API response for display
    public ApiResponse? ApiResult { get; private set; } // Nullable

    // Property to hold any error messages
    public string? ErrorMessage { get; private set; } // Nullable

    // Constructor Injection for dependencies
    public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    /// <summary>
    /// Handles GET requests (initial page load)
    /// </summary>
    public void OnGet()
    {
        // Pre-populate the form or load initial data if needed
    }

    /// <summary>
    /// Handles the POST request and calls the API
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) // Basic validation check
        {
            return Page(); // Re-display the page with validation errors if any
        }

        // Retrieve API settings from configuration
        string? apiUrl = _configuration["ApiSettings:BaseUrl"];
        string? bearerToken = _configuration["ApiSettings:BearerToken"]; // Reads from appsettings or user secrets

        if (string.IsNullOrEmpty(apiUrl) || string.IsNullOrEmpty(bearerToken))
        {
            ErrorMessage = "API URL or Bearer Token is not configured correctly in appsettings or user secrets.";
            _logger.LogError(ErrorMessage);
            return Page();
        }

        // Create the request payload
        var requestPayload = new ApiRequest
        {
            Query = this.Query, // Use the value bound from the form
            MaxSqlResults = this.MaxSqlResults // Use the value bound from the form
        };

        try
        {
            // Use HttpClientFactory to create an HttpClient instance
            var httpClient = _httpClientFactory.CreateClient();

            // Serialize the C# object to a JSON string
            string jsonPayload = JsonSerializer.Serialize(requestPayload);

            // Create the HTTP request content
            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Create the HttpRequestMessage
            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
            {
                Content = httpContent
            };

            // Add the Authorization header
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            _logger.LogInformation("Sending API request to {ApiUrl}", apiUrl);

            // Send the request
            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                // Read and deserialize the response body
                string responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("API Response received: {ResponseBody}", responseBody);

                // Deserialize JSON response to our C# model
                ApiResult = JsonSerializer.Deserialize<ApiResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }); // Be flexible with casing

                if (ApiResult == null)
                {
                    ErrorMessage = "Failed to deserialize API response.";
                    _logger.LogError(ErrorMessage);
                }
            }
            else
            {
                // Handle API error response
                string errorContent = await response.Content.ReadAsStringAsync();
                ErrorMessage = $"API request failed with status code {response.StatusCode}. Details: {errorContent}";
                _logger.LogError("API Error: Status {StatusCode}, Content: {ErrorContent}", response.StatusCode, errorContent);
            }
        }
        catch (HttpRequestException httpEx)
        {
            ErrorMessage = $"Network error calling API: {httpEx.Message}";
            _logger.LogError(httpEx, "Network error occurred during API call.");
        }
        catch (JsonException jsonEx)
        {
                ErrorMessage = $"Error parsing API response: {jsonEx.Message}";
            _logger.LogError(jsonEx, "JSON deserialization error.");
        }
        catch (Exception ex)
        {
            // Catch unexpected errors
            ErrorMessage = $"An unexpected error occurred: {ex.Message}";
            _logger.LogError(ex, "An unexpected error occurred.");
        }

        // Return the Page to re-render it with results or errors
        return Page();
    }
}
