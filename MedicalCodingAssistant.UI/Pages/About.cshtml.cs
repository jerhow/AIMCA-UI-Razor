using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicalCodingAssistant.UI.Pages;

public class AboutModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;

    public AboutModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}
