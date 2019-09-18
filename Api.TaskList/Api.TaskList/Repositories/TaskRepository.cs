using Api.TaskList.Helpers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Api.TaskList.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<Models.Task>> GetTasks(int id = 0);
        Task AddTask(Models.Task task);
        Task EditTask(Models.Task task);
        Task DelTask(int id);
        Task AlterState(Models.Task task);
    }

    public class TaskRepository : BaseRepository, ITaskRepository
    {
        public TaskRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<IEnumerable<Models.Task>> GetTasks(int id = 0)
        {
            var sql = new StringBuilder();

            sql.Append("SELECT * FROM TASK ");

            if (id > 0)
                sql.Append($" WHERE ID = {id}");

            using (var dr = _connectionSql.ExecutarDataReader(sql.ToString()))
            {
                if (dr.HasRows)
                {
                    return MapHelper.DataReaderMapToList<Models.Task>(dr);
                }

                return null;
            }
        }

        public async Task AddTask(Models.Task task)
        {
            var sql = new StringBuilder();

            sql.Append(" INSERT INTO TASK ");
            sql.Append(" ( ");
            sql.Append(" TITULO, ");
            sql.Append(" STATUS, ");
            sql.Append(" DESCRICAO, ");
            sql.Append(" DATACRIACAO, ");
            sql.Append(" DATAEDICAO, ");
            sql.Append(" DATAREMOCAO, ");
            sql.Append(" DATACONCLUSAO ");
            sql.Append(" ) ");
            sql.Append(" VALUES ");
            sql.Append(" ( ");
            sql.Append(SqlHelper.FormatarCampoSql(task.Titulo) + ",");
            sql.Append(SqlHelper.FormatarCampoSql(task.Status) + ",");
            sql.Append(SqlHelper.FormatarCampoSql(task.Descricao) + ",");
            sql.Append(SqlHelper.FormatarCampoSql(task.DataCriacao) + ",");
            sql.Append(SqlHelper.FormatarCampoSql(task.DataEdicao) + ",");
            sql.Append(SqlHelper.FormatarCampoSql(task.DataRemocao) + ",");
            sql.Append(SqlHelper.FormatarCampoSql(task.DataConclusao));
            sql.Append(" ) ");

            _connectionSql.ExecutarComando(sql.ToString());
        }

        public async Task EditTask(Models.Task task)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" UPDATE TASK SET ");
            sql.AppendFormat(" TITULO = {0}, ", SqlHelper.FormatarCampoSql(task.Titulo));
            sql.AppendFormat(" STATUS = {0}, ", SqlHelper.FormatarCampoSql(task.Status));
            sql.AppendFormat(" DESCRICAO = {0}, ", SqlHelper.FormatarCampoSql(task.Descricao));
            //sql.AppendFormat(" DATACRIACAO = {0}, ", SqlHelper.FormatarCampoSql(task.DataCriacao));
            sql.AppendFormat(" DATAEDICAO = {0}, ", SqlHelper.FormatarCampoSql(task.DataEdicao));
            sql.AppendFormat(" DATAREMOCAO = {0}, ", SqlHelper.FormatarCampoSql(task.DataRemocao));
            sql.AppendFormat(" DATACONCLUSAO = {0} ", SqlHelper.FormatarCampoSql(task.DataConclusao));
            sql.AppendFormat(" WHERE ID = {0} ", task.Id);

            _connectionSql.ExecutarComando(sql.ToString());
        }

        public async Task AlterState(Models.Task task)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" UPDATE TASK SET ");
            sql.AppendFormat(" STATUS = {0}, ", SqlHelper.FormatarCampoSql(task.Status));
            sql.AppendFormat(" DATAEDICAO = {0}, ", SqlHelper.FormatarCampoSql(task.DataEdicao));
            sql.AppendFormat(" DATAREMOCAO = {0}, ", SqlHelper.FormatarCampoSql(task.DataRemocao));
            sql.AppendFormat(" DATACONCLUSAO = {0} ", SqlHelper.FormatarCampoSql(task.DataConclusao));
            sql.AppendFormat(" WHERE ID = {0} ", task.Id);

            _connectionSql.ExecutarComando(sql.ToString());
        }

        public async Task DelTask(int id)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(" DELETE FROM TASK WHERE ID = {0} ", id);

            _connectionSql.ExecutarComando(sql.ToString());
        }
    }
}
