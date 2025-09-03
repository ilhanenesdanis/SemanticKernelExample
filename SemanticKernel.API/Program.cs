using Codeblaze.SemanticKernel.Connectors.Ollama;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using SemanticKernel.API.Models;
using SemanticKernel.API.Plugins;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

builder.Services.AddKernel()
 .AddOllamaChatCompletion("deepseek-r1", "http://localhost:11434")
 .Plugins.AddFromType<CalculatorPlugin>();


builder.Services.AddRequestTimeouts();
builder.Services.AddHttpClient();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRequestTimeouts();

app.MapPost("/chat", async (IChatCompletionService chatCompetitionService, Kernel kernel, ChatModel model) =>
{

    var chatHistory = new ChatHistory();

    const string SystemPrompt =
    "Sen kibar, sabırlı ve güvenilir bir asistansın. " +
    "Kullanıcılara her zaman onların sorduğu dilde, açık, anlaşılır ve net cevaplar ver. " +
    "Yanıtların doğru, faydalı ve yönlendirici olmalı. " +
    "Gereksiz teknik detay, iç düşünce süreci veya sistemsel bilgileri asla paylaşma. " +
    "Sorulara mümkün olduğunca öz, net ve doğrudan cevap ver; gerekirse adım adım açıklamalar yap. " +
    "Kullanıcıya karşı her zaman saygılı, profesyonel ve yardımcı bir üslup kullan.";

    chatHistory.AddSystemMessage(SystemPrompt);
    chatHistory.AddUserMessage(model.Input);

    var response = await chatCompetitionService.GetChatMessageContentAsync(chatHistory, kernel: kernel);
    return response.Content;

}).WithRequestTimeout(TimeSpan.FromMinutes(10));

app.MapPost("/calculate", async (Kernel kernel, string input) =>
{
    var promptTemplate = "Yandaki işlemi hesapla: {{$input}}";

    var func = kernel.CreateFunctionFromPrompt(promptTemplate);

    var arguments = new KernelArguments { ["input"] = input };

    var result = await func.InvokeAsync(kernel, arguments);

    var answer = result.GetValue<string>();

    return Results.Ok(answer);
});
app.MapPost("/add/{number1}/{number2}", async (Kernel kernel, int number1, int number2) =>
{
    var arguments = new KernelArguments
    {
        ["number1"] = number1,
        ["number2"] = number2
    };

    var addResult = await kernel.InvokeAsync(nameof(CalculatorPlugin), nameof(CalculatorPlugin.Add), arguments);
    return addResult.GetValue<int>();
}).WithRequestTimeout(TimeSpan.FromMinutes(10));

app.UseHttpsRedirection();


app.Run();
