using DispatcherApp.DAL.Seed;
using DispatcherApp.Common.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddCommonConfiguration();
builder.AddDataAccessServices();
builder.AddApiServices();
builder.AddBusinessLogicServices();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("DefaultPolicy");
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
app.MapFallbackToFile("index.html");
app.MapControllers();

app.Run();
