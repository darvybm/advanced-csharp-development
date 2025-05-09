using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly TaskService _service;

        public TaskController(TaskService service)
        {
            _service = service;
        }

        [HttpGet]
        [HttpGet]
        public async Task<ActionResult<TaskResponse<List<TaskModel>>>> Get()
        {
            return Ok(TaskResponse<List<TaskModel>>.Ok(await _service.GetAllAsync()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskResponse<TaskModel>>> Get(Guid id)
        {
            var task = await _service.GetByIdAsync(id);
            if (task == null)
                return NotFound(TaskResponse<TaskModel>.Fail("Tarea no encontrada."));

            return Ok(TaskResponse<TaskModel>.Ok(task));
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponse<TaskModel>>> Create(TaskRequest taskRequest)
        {
            if (string.IsNullOrWhiteSpace(taskRequest.Description))
                return BadRequest(TaskResponse<TaskModel>.Fail("La descripción no puede estar vacía."));

            if (taskRequest.DueDate <= DateTime.Now)
                return BadRequest(TaskResponse<TaskModel>.Fail("La fecha de vencimiento debe ser futura."));

            var task = new TaskModel
            {
                Id = Guid.NewGuid(), // El ID lo asigna aquí
                Description = taskRequest.Description,
                DueDate = taskRequest.DueDate,
                Status = taskRequest.Status,
                ExtraData = taskRequest.ExtraData
            };

            await _service.CreateAsync(task);

            return CreatedAtAction(nameof(Get), new { id = task.Id }, TaskResponse<TaskModel>.Ok(task, "Tarea creada correctamente."));
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<TaskResponse<object>>> Update(Guid id, TaskRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Description))
                return BadRequest(TaskResponse<object>.Fail("La descripción no puede estar vacía."));

            if (request.DueDate <= DateTime.Now)
                return BadRequest(TaskResponse<object>.Fail("La fecha de vencimiento debe ser futura."));

            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound(TaskResponse<object>.Fail("Tarea no encontrada."));

            existing.Description = request.Description;
            existing.DueDate = request.DueDate;
            existing.Status = request.Status;
            existing.ExtraData = request.ExtraData;

            await _service.UpdateAsync(existing);
            return Ok(TaskResponse<object>.Ok(existing, "Tarea actualizada correctamente."));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TaskResponse<object>>> Delete(Guid id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound(TaskResponse<object>.Fail("Tarea no encontrada."));

            await _service.DeleteAsync(id);
            return Ok(TaskResponse<object>.Ok(null, "Tarea eliminada correctamente."));
        }
    }
}
