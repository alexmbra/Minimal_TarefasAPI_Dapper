using Dapper.Contrib.Extensions;
using TarefasAPI_Dapper.Data;
using static TarefasAPI_Dapper.Data.TarefaContext;

namespace TarefasAPI_Dapper.Endpoints;

public static class TarefasEndpoints
{
    public static void MapTarefasEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => $"Bem-vindo a API Tarefas - {DateTime.Now}");

        app.MapGet("Tarefas", async(GetConnection connectionGetter) =>
        {
            using var con = await connectionGetter();
            return con.GetAll<Tarefa>().ToList() is List<Tarefa> tarefas ? Results.Ok(tarefas) : Results.NotFound();            
        });

        app.MapGet("tarefas/{id}", async(int id, GetConnection connectionGetter) =>
        {
            using var con = await connectionGetter();
            return con.Get<Tarefa>(id) is Tarefa tarefa ? Results.Ok(tarefa) : Results.NotFound();
        });

        app.MapPost("tarefas", async(Tarefa tarefa, GetConnection connectionGetter) =>
        {
            try
            {
                using var con = await connectionGetter();
                var id = con.Insert(tarefa);
                return Results.Created($"/tarefas/{id}", tarefa);
            }
            catch (Exception)
            {
                return Results.StatusCode(500);
            }
        });

        app.MapPut("tarefas/put", async(Tarefa tarefa, GetConnection connectionGetter) =>
        {
            try
            {
                using var con = await connectionGetter();
                var id = con.Update(tarefa);
                return Results.Ok();
            }
            catch (Exception)
            {
                //return Results.Ok(new { ErrorMessage = ex.Message });
                return Results.Problem("Ocorreu um problema ao tratar a sua solicitação",null,500);
                //return Results.StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação");
            }
        });

        app.MapDelete("tarefas/{id}", async(int id, GetConnection connectionGetter) =>
        {
            using var con = await connectionGetter();
            var deleted = con.Get<Tarefa>(id);
            if (deleted is null) return Results.NotFound();
            con.Delete(deleted);
            return Results.Ok(deleted);
        });
    }
}
