using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Xunit;

public class AiAssistantTests
{
    private readonly HttpClient _client;
    private readonly string _deployment;

    public AiAssistantTests()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json")
            .Build();

        _deployment = config["AzureOpenAI:Deployment"];

        _client = new HttpClient
        {
            BaseAddress = new System.Uri(config["AzureOpenAI:Endpoint"])
        };
        _client.DefaultRequestHeaders.Add("api-key", config["AzureOpenAI:ApiKey"]);
    }

    [Fact(Skip = "Requires valid Azure OpenAI credentials")]
    public async Task OpenAI_ReturnsResponse_ForSimplePrompt()
    {
        var request = new HttpRequestMessage(HttpMethod.Post,
            $"/openai/deployments/{_deployment}/chat/completions?api-version=2025-01-01-preview");

        var body = new
        {
            messages = new[]
            {
                new { role = "system", content = "You are a helpful assistant." },
                new { role = "user", content = "Say hello" }
            },
            max_tokens = 50
        };

        var json = JsonSerializer.Serialize(body);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode);
        Assert.Contains("hello", content, System.StringComparison.OrdinalIgnoreCase);
    }
}