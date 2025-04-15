namespace MedicalCodingAssistant.UI.Models;

public class ApiRequest
{
    public string Query { get; set; } = string.Empty; // Initializing to avoid null warnings
    public int MaxSqlResults { get; set; }
}
