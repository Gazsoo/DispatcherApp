using DispatcherApp.Models.CommonConfigurations;
using DispatcherApp.BLL.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddCommonConfiguration();
builder.AddDataAccessServices();
builder.AddApiServices();
builder.AddBusinessLogicServices();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseCors("DefaultPolicy");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCustomMiddleware();
if (app.Environment.IsDevelopment())
{
    await app.RoleSeed();
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
app.UseExceptionHandler(options => {});
app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
