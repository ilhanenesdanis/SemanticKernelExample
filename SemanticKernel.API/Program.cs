using Codeblaze.SemanticKernel.Connectors.Ollama;
using Microsoft.SemanticKernel.ChatCompletion;
using SemanticKernel.API.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

builder.Services.AddKernel()
 .AddOllamaChatCompletion("deepseek-r1","http://localhost:11434");

builder.Services.AddRequestTimeouts();
builder.Services.AddHttpClient();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRequestTimeouts();

app.MapPost("/chat", async (IChatCompletionService chatCompetitionService, ChatModel model) =>
{
    var response = await chatCompetitionService.GetChatMessageContentAsync(model.Input);
    return response.ToString();
}).WithRequestTimeout(TimeSpan.FromMinutes(10));



app.UseHttpsRedirection();


app.Run();
