using TarefasAPI_Dapper;
using TarefasAPI_Dapper.Endpoints;
using TarefasAPI_Dapper.Extensions;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

builder.AddPersistence();

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        // using static System.Net.Mime.MediaTypeNames;  
        context.Response.ContentType = Text.Plain;

        var exceptionHandlerPathFeature =
           context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature?.Error is not null)
        {
            await context.Response.WriteAsync(exceptionHandlerPathFeature?.Error.Message);
        }

    });
});

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapTarefasEndpoints();

app.Run();
