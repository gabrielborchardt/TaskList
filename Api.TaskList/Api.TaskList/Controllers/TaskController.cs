using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.TaskList.Models;
using Api.TaskList.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Api.TaskList.Controllers
{
    [Produces("application/json")]
    [Route("")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private string _connectionString;
        public TaskController(IConfiguration Configuration)
        {
            _connectionString = Configuration["ConnectionsString:DefaultConnection"];
        }

        /// <summary>
        /// Return all tasks
        /// </summary>
        /// <param name="GetTasks"></param>
        /// <returns>List of tasks</returns>
        [HttpGet("GetTasks")]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetTasks()
        {
            try
            {
                using (var taskService = new TaskService(_connectionString))
                {
                    var lista = await taskService.GetTasks();

                    if (lista != null)
                    {
                        return Ok(lista);
                    }
                    else
                    {
                        return NoContent();
                    }
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(new ErrorResponse() { Mensagem = ex.Message });
            }
        }

        [HttpPost("PostTask")]
        public async Task<IActionResult> PostTask([FromBody] Models.Task task)
        {
            try
            {
                using (var taskService = new TaskService(_connectionString))
                {
                    if(task.Status == null)
                            task.Status = "A";

                    var retorno = await taskService.ValidateModel(task);
                    if (retorno != "OK")
                        throw new Exception(retorno);

                    await taskService.AddTask(task);

                    var lista = await taskService.GetTasks();

                    return Ok(lista);
                }

            }
            catch (System.Exception ex)
            {
                return BadRequest(new ErrorResponse() { Mensagem = ex.Message });
            }
        }

        [HttpPut("PutTask")]
        public async Task<IActionResult> PutTask([FromBody] Models.Task task)
        {
            try
            {
                using (var taskService = new TaskService(_connectionString))
                {
                    var retorno = await taskService.ValidateModel(task);
                    if (retorno != "OK")
                        throw new Exception(retorno);

                    await taskService.EditTask(task);

                    var lista = await taskService.GetTasks(task.Id);

                    return Ok(lista);
                }

            }
            catch (System.Exception ex)
            {
                return BadRequest(new ErrorResponse() { Mensagem = ex.Message });
            }
        }

        [HttpDelete("DeleteTask")]
        public async Task<IActionResult> DelTask(int id)
        {
            try
            {
                using (var taskService = new TaskService(_connectionString))
                {
                    await taskService.DelTask(id);

                    var lista = await taskService.GetTasks();

                    return Ok(lista);
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(new ErrorResponse() { Mensagem = ex.Message });
            }
        }

        [HttpPut("AlterState")]
        public async Task<IActionResult> AlterState(int id, string status)
        {
            try
            {
                using (var taskService = new TaskService(_connectionString))
                {
                    
                    await taskService.AlterState(id, status);

                    return Ok();
                }

            }
            catch (System.Exception ex)
            {
                return BadRequest(new ErrorResponse() { Mensagem = ex.Message });
            }
        }
    }
}
