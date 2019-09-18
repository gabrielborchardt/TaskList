using System.Data.Common;
using System.Data.SqlClient;

namespace Api.TaskList.Connection
{
    public class ConnectionSql
    {
        private SqlConnection _sqlConnection;
        private SqlTransaction _sqlTransaction;

        public ConnectionSql(string stringConexao)
        {
            _sqlConnection = new SqlConnection(stringConexao);
            _sqlConnection.Open();
        }

        public void IniciarTransacao()
        {
            _sqlTransaction = _sqlConnection.BeginTransaction();
        }

        public void FinalizarTransacao()
        {
            _sqlTransaction.Commit();
        }

        public void DesfazerTransacao()
        {
            _sqlTransaction.Rollback();
        }

        public void ExecutarComando(string sql)
        {

            if (this._sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                _sqlConnection.Close();
                _sqlConnection.Open();
            }
            using (var sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = _sqlConnection;
                sqlCommand.Transaction = _sqlTransaction;
                sqlCommand.CommandText = sql;
                sqlCommand.ExecuteNonQuery();
            }

        }

        public DbDataReader ExecutarDataReader(string sql)
        {
            return (DbDataReader)new SqlCommand(sql, _sqlConnection).ExecuteReader();
        }

        public void Dispose()
        {
            _sqlConnection.Close();
            _sqlConnection.Dispose();
        }
    }
}
