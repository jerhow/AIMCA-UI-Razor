using System.Text.Json.Serialization; // Required for JsonPropertyName (if C# property name differs from JSON key, including casing)

namespace MedicalCodingAssistant.UI.Models;

public class ApiResponse
{
    public bool UsedFreeTextFallback { get; set; }
    public int TotalSqlResultCount { get; set; }
    public string AiModel { get; set; } = string.Empty;
    public string AiVersion { get; set; } = string.Empty;
    public double AiTemperature { get; set; }
    public List<SearchResult> SearchResults { get; set; } = new List<SearchResult>();
}

public class SearchResult
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;
    
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
    
    [JsonPropertyName("rank")]
    public int Rank { get; set; }
    
    [JsonPropertyName("reason")]
    public string Reason { get; set; } = string.Empty;
    
    [JsonPropertyName("source")]
    public string Source { get; set; } = string.Empty;
    
    [JsonPropertyName("confidence")]
    public int Confidence { get; set; }
    
    public bool IsValid { get; set; }
}
