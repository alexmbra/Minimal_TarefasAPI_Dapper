using System.Data;

namespace TarefasAPI_Dapper.Data
{
    public class TarefaContext
    {
        public delegate Task<IDbConnection> GetConnection();
    }
}
