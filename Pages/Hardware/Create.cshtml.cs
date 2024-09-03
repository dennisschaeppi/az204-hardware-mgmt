using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace HardwareMgmt.Pages;

public class CreateHardwareModel : PageModel
{
  private readonly ILogger<HardwareModel> _logger;
  private readonly IConfiguration _configuration;
  public CreateHardwareModel(ILogger<HardwareModel> logger, IConfiguration configuration)
  {
      _logger = logger;
      _configuration = configuration;
  }

  [BindProperty]
  public string Name { get; set; }

  [BindProperty]
  public string Description { get; set; }

  [BindProperty]
  public string Category { get; set; }

  [BindProperty]
  public decimal Price { get; set; }

  [BindProperty]
  public int StockQuantity { get; set; }

  public string Message { get; set; }

  public async Task<IActionResult> OnPostAsync()
  {
    var hardwareItem = new HardwareItem
    {
      Id = Guid.NewGuid().ToString(),
      Name = this.Name,
      Description = this.Description,
      Category = this.Category,
      Price = this.Price,
      StockQuantity = this.StockQuantity
    };

    var json = JsonSerializer.Serialize(hardwareItem);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    using (var httpClient = new HttpClient())
    {
        // Set the authorization key here
      httpClient.DefaultRequestHeaders.Add("x-functions-key", _configuration["Functions-Key"]);

      var response = await httpClient.PostAsync($"{_configuration["FunctionsUri"]}/CreateHardware", content);

      if (response.IsSuccessStatusCode)
      {
        Message = "Hardware item created successfully.";
        return RedirectToPage("/Hardware");
      }
      else
      {
        Message = $"Error {response.StatusCode}: {response.ReasonPhrase}";
      }
    }

    return Page();
  }
}

