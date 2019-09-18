using Api.TaskList.Connection;
using System;

namespace Api.TaskList.Repositories
{
    public class BaseRepository : IDisposable
    {
        protected ConnectionSql _connectionSql;

        public BaseRepository(string connectionString)
        {
            _connectionSql = new ConnectionSql(connectionString);
        }

        public void Dispose()
        {
            _connectionSql.Dispose();
        }
    }
}
