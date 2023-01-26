using System.ComponentModel.DataAnnotations.Schema;

namespace TarefasAPI_Dapper.Data;

[Table("Tarefas")]
public record Tarefa(int Id, string Atividade, string Status);
