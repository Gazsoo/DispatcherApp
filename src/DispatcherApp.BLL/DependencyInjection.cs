using System.Reflection;
using DispatcherApp.API.Controllers;
using DispatcherApp.BLL.Common.Configurations;
using DispatcherApp.BLL.Common.Extentions;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.BLL.Common.Mapping;
using DispatcherApp.BLL.Common.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Hosting;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddBusinessLogicServices(this IHostApplicationBuilder builder)
        {

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxNzkwMjA4MDAwIiwiaWF0IjoiMTc1ODcxMTA5NiIsImFjY291bnRfaWQiOiIwMTk5N2I1OTQ4Zjk3ZDJjOTBmZTczNWQ0YWMwOTAyYiIsImN1c3RvbWVyX2lkIjoiY3RtXzAxazV4bms4dnJybmZheDZtZjd6eTc1ZHg0Iiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.QMErGqav2gKbTkDZWLkc_AtmR-u2sIFCtn6SJHxrUV8jh-kn-95xoUq9iH7QUSECvWr6aS0f-EKGCUgoSHla6fjzpkLJdY_bmkZSOmN2NISeluEn3Hob9M7kGwYoPhRhV-tFJAi1fEVo0JlgnrAsfpfjGAmYFrcKgNWAKaW7CFub-2Kvs6KRVkx-QkWAVZg_6Q8tythqnKpe-wOy6u76L7gaqjDFqXyUTwSpVDhaym8C13Mufwo5DZWxdKqLiqCwk-9NrOIz-A3e1Vqqxeo2Mb7wu6KIgJxm--Nn4gfDFpUUbBarK1EC4x-syDTZ-gHrEbjHEG5fBsF2XPxgLDYfzA", typeof(MappingProfile));
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddSingleton<IEmailSender<IdentityUser>, DummyEmailSender>();
            builder.Services.AddSingleton<IEmailSender, DummyEmailSender>();
            builder.Services.AddScoped<IAssignmentService, AssignmentService>();
            builder.Services.AddScoped<ITutorialService, TutorialService>();
            builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
            builder.Services.AddScoped<IFileService, FileService>();

        }

    }

}
