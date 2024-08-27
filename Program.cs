using Microsoft.Azure.Cosmos;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine(builder.Configuration["KeyVaultName"]);

builder.Configuration.AddAzureKeyVault(
    new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"),
    new DefaultAzureCredential()
);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSingleton(s =>
{
    try
    {
        var cosmosClient = new CosmosClient(builder.Configuration["CosmosDb-ConnectionString"]);
        return new CosmosDbService(cosmosClient, builder.Configuration["CosmosDb:DatabaseName"], builder.Configuration["CosmosDb:ContainerName"]);
    }
    catch (Exception ex)
    {
        //builder.Logging.LogError("Failed to configure CosmosDbService: {ExceptionMessage}", ex.Message);
        throw;
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
