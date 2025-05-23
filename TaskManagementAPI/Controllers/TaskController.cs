﻿using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;
using TaskManagementAPI.Models.Factory;
using TaskManagementAPI.Services;
using TaskFactory = TaskManagementAPI.Models.Factory.TaskFactory;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly TaskService _service;
        private ReactiveTaskQueue _queue;

        public delegate bool TaskValidator(TaskRequest request);
        private readonly TaskValidator _validator = req => !string.IsNullOrWhiteSpace(req.Description) && req.DueDate > DateTime.Now;

        private readonly Action<string> _notify = msg => Console.WriteLine($"[NOTIFICACIÓN] {msg}");

        private readonly Func<TaskModel, object> _taskWithExtras = t => new
        {
            t.Id,
            t.Description,
            t.DueDate,
            t.Status,
            t.ExtraData,
            DaysLeft = (t.DueDate - DateTime.Now).Days
        };

        public TaskController(TaskService service, ReactiveTaskQueue queue)
        {
            _service = service;
            _queue = queue;
        }

        [HttpGet]
        public async Task<ActionResult<TaskResponse<List<TaskModel>>>> Get([FromQuery] string? status, [FromQuery] string? search, [FromQuery] DateTime? dueBefore)
        {
            var tasks = await _service.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(status))
                tasks = tasks.Where(t => t.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(search))
                tasks = tasks.Where(t => t.Description.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            if (dueBefore.HasValue)
                tasks = tasks.Where(t => t.DueDate < dueBefore.Value).ToList();

            return Ok(TaskResponse<List<TaskModel>>.Ok(tasks));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<TaskResponse<TaskModel>>> Get(Guid id)
        {
            var task = await _service.GetByIdAsync(id);
            if (task == null)
                return NotFound(TaskResponse<TaskModel>.Fail("Tarea no encontrada."));

            return Ok(TaskResponse<object>.Ok(_taskWithExtras(task)));
        }


        [HttpPost]
        public async Task<ActionResult<TaskResponse<TaskModel>>> Create(
        [FromBody] TaskRequest taskRequest,
        [FromQuery] string? prioriry)
        {
            Debug.WriteLine("TEST");
            if (string.IsNullOrWhiteSpace(taskRequest.Description) ||
                (string.IsNullOrWhiteSpace(prioriry) && taskRequest.DueDate <= DateTime.Now))
            {
                return BadRequest(TaskResponse<TaskModel>.Fail("Descripción inválida o fecha no válida."));
            }

            TaskModel task;

            if (!string.IsNullOrWhiteSpace(prioriry))
            {
                task = TaskFactory.CreateByPriority(taskRequest.Description, prioriry);
            }
            else
            {
                task = TaskFactory.CreateCustomTask(
                    taskRequest.Description,
                    taskRequest.DueDate,
                    taskRequest.Status,
                    taskRequest.ExtraData
                );
            }

            await _service.CreateAsync(task);

            // _notify($"Tarea '{task.Description}' creada (ID: {task.Id})");

            _queue.Enqueue(task);

            return Ok(TaskResponse<TaskModel>.Ok(task, "Tarea creada correctamente."));
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<TaskResponse<object>>> Update(Guid id, TaskRequest request)
        {
            if (!_validator(request))
                return BadRequest(TaskResponse<TaskModel>.Fail("Descripción inválida o fecha no válida."));

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

            _notify($"Tarea con ID {id} eliminada");

            return Ok(TaskResponse<object>.Ok(null, "Tarea eliminada correctamente."));
        }
    }
}