using SupportBot.Core.Data;
using SupportBot.Core.Models;
using SupportBot.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddSingleton<IReadOnlyList<IssueTemplate>>(_ => IssueTemplateProvider.CreateDefaultTemplates());
builder.Services.AddSingleton<IResponseGenerator>(sp =>
{
    var templates = sp.GetRequiredService<IReadOnlyList<IssueTemplate>>();
    return new IssueResponseGenerator(templates);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/api/chat", (ChatRequest request, IResponseGenerator generator) =>
    Results.Ok(generator.GenerateResponse(request)))
    .WithName("GenerateChatResponse")
    .WithOpenApi();

app.MapGet("/api/knowledge-base", (IReadOnlyList<IssueTemplate> templates) =>
    Results.Ok(templates.Select(template => new
    {
        template.Category,
        template.Summary,
        template.SuggestedActions,
        template.FollowUpQuestions
    })))
    .WithName("GetKnowledgeBase")
    .WithOpenApi();

app.Run();
