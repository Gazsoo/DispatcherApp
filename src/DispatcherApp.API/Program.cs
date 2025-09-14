using DispatcherApp.Models.CommonConfigurations;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.AddCommonConfiguration();
builder.AddDataAccessServices();
builder.AddApiServices();
builder.AddBusinessLogicServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(settings =>
    {
        settings.Path = "/api/specification.json";
    });
    app.UseSwaggerUi(settings =>
    {
        settings.Path = "/api";
        settings.DocumentPath = "/api/specification.json";
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapGroup("/account").MapIdentityApi<IdentityUser>();
app.MapControllers();

app.Run();
