using Api.TaskList.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.TaskList.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<Models.Task>> GetTasks(int id = 0);
        Task AddTask(Models.Task task);
        Task EditTask(Models.Task task);
        Task DelTask(int id);
        Task<string> ValidateModel(Models.Task task);
        Task AlterState(int id, string status);
    }

    public class TaskService : ITaskService, IDisposable
    {
        private readonly TaskRepository _taskRepository;

        public TaskService(string connectionString)
        {
            _taskRepository = new TaskRepository(connectionString);
        }

        public async Task<IEnumerable<Models.Task>> GetTasks(int id = 0)
        {
            return await _taskRepository.GetTasks(id);
        }

        public async Task AddTask(Models.Task task)
        {
            task.DataCriacao = DateTime.Now;

            await _taskRepository.AddTask(task);
        }

        public async Task EditTask(Models.Task task)
        {
            task.DataEdicao = DateTime.Now;

            if (task.Status == "A")
                task.DataConclusao = null;
            else if (task.Status == "C")
                task.DataConclusao = DateTime.Now;

            if (task.Status == "R")
                task.DataRemocao = DateTime.Now;

            await _taskRepository.EditTask(task);
        }

        public async Task DelTask(int id)
        {
            await _taskRepository.DelTask(id);
        }

        public async Task<string> ValidateModel(Models.Task task)
        {
            if (task.Titulo.Trim().Length == 0)
                return "Título inválido";
            else if(task.Status.Trim().Length != 1)
                return "Status inválido";
            else if (!task.Status.Contains("A") && !task.Status.Contains("R") && !task.Status.Contains("C"))
                return "Status inválido";
            else if (task.DataCriacao == null)
                return "Data de criação inválida";

            return "OK";
        }

        public async Task AlterState(int id, string status)
        {
            var task = new Models.Task()
            {
                Id = id,
                Status = status,
            };

            switch (status)
            {
                case "A":
                    task.DataEdicao = DateTime.Now;
                    task.DataConclusao = null;
                    task.DataRemocao = null;
                    break;
                case "R":
                    task.DataEdicao = DateTime.Now;
                    task.DataConclusao = null;
                    task.DataRemocao = DateTime.Now;
                    break;
                case "C":
                    task.DataEdicao = DateTime.Now;
                    task.DataConclusao = DateTime.Now;
                    task.DataRemocao = null;
                    break;
                default:
                    return;
            }

            await _taskRepository.AlterState(task);
        }

        public void Dispose()
        {
            _taskRepository.Dispose();
        }
    }
}
