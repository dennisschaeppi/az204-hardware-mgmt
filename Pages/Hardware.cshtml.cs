using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace HardwareMgmt.Pages;

public class HardwareModel : PageModel
{
    private readonly ILogger<HardwareModel> _logger;
    private readonly CosmosDbService _cosmosDbService;
    public IEnumerable<HardwareItem> HardwareItems { get; private set; }
    public HardwareModel(ILogger<HardwareModel> logger, CosmosDbService cosmosDbService)
    {
        _logger = logger;
        _cosmosDbService = cosmosDbService;
    }

    public async Task OnGetAsync()
    {
      HardwareItems = await _cosmosDbService.GetHardwareItemsAsync();
    }
}

