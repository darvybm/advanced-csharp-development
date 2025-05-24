using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services
{
    public class ReactiveTaskQueue
    {
        private readonly Subject<TaskModel> _taskSubject = new();
        private readonly Queue<TaskModel> _taskQueue = new();
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private bool _isProcessing = false;

        public ReactiveTaskQueue()
        {
            _taskSubject.Subscribe(async task =>
            {
                _taskQueue.Enqueue(task);
                await ProcessQueueAsync();
            });
        }

        public void Enqueue(TaskModel task)
        {
            Debug.WriteLine($"📥 Tarea encolada: {task.Description}");
            _taskSubject.OnNext(task);
        }

        private async Task ProcessQueueAsync()
        {
            if (_isProcessing) return;
            _isProcessing = true;

            while (_taskQueue.Any())
            {
                var current = _taskQueue.Dequeue();
                Debug.WriteLine($"⚙️ Procesando: {current.Description}");

                await _semaphore.WaitAsync();
                try
                {
                    // Simulación del procesamiento de una tarea
                    await Task.Delay(50000); // Aquí podrías usar lógica real
                    Debug.WriteLine($"✅ Finalizada: {current.Description}");
                }
                finally
                {
                    _semaphore.Release();
                }
            }

            _isProcessing = false;
        }
    }
}
