using Microsoft.EntityFrameworkCore;
using WebApp.Components;
using WebApp.Data;
using Microsoft.EntityFrameworkCore.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// 他のサービス設定の下に追加:
builder.Services.AddHttpClient();



// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Make a blueprint for client instance named by AzureOpenAI
/*AKA Factory register, which enables client object(AiAssistant.razor) to reuse it.
If not Factory register, the client object is created by "new" every time(Socket Memory leak)
and hard to extend for other URI.
*/
builder.Services.AddHttpClient("AzureOpenAI", client =>
{
    var endpoint = builder.Configuration["AzureOpenAI:Endpoint"];
    var apiKey = builder.Configuration["AzureOpenAI:ApiKey"];
    
    // For debug
    Console.WriteLine($"🔍 AzureOpenAI Endpoint: {endpoint}");
    Console.WriteLine($"🔍 AzureOpenAI ApiKey is null? {string.IsNullOrEmpty(apiKey)}");

    client.BaseAddress = new Uri(endpoint);
    client.DefaultRequestHeaders.Add("api-key", apiKey);
    
    // Dependency Injection for various class, calling DI container which creates and manages objects 
    // client.BaseAddress = new Uri(builder.Configuration["AzureOpenAI:Endpoint"]);
    // client.DefaultRequestHeaders.Add("api-key", builder.Configuration["AzureOpenAI:ApiKey"]);
});


// Register AppDbContext to service container in Azure
// Instance is created in the view page based on user request
// The instance is deleted once the DB action is done
//DefaultConnection is a connection string from Azure DB resource 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);



var app = builder.Build();

// For TestQueries.cs
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    WebApp.TestQueries.Run(db);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();