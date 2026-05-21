using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Factory;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repositorio;

        public event EventHandler TareasModificadas;

        public TaskService(ITaskRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public List<TaskItem> ObtenerTareas()
        {
            return _repositorio.ObtenerTodos();
        }

        public List<TaskItem> FiltrarPorEstado(TaskStatus estado)
        {
            return _repositorio.ObtenerTodos()
                .Where(t => t.Estado == estado)
                .ToList();
        }

        public List<TaskItem> FiltrarPorPrioridad(TaskPriority prioridad)
        {
            return _repositorio.ObtenerTodos()
                .Where(t => t.Prioridad == prioridad)
                .ToList();
        }

        public void CrearTarea(string titulo, string descripcion, TaskPriority prioridad, DateTime? fechaVencimiento)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("El titulo no puede estar vacio");

            var tarea = TaskFactory.Crear(titulo, descripcion, prioridad, fechaVencimiento);
            _repositorio.Agregar(tarea);
            TareasModificadas?.Invoke(this, EventArgs.Empty);
        }

        public void ActualizarTarea(TaskItem tarea)
        {
            _repositorio.Actualizar(tarea);
            TareasModificadas?.Invoke(this, EventArgs.Empty);
        }

        public void EliminarTarea(Guid id)
        {
            _repositorio.Eliminar(id);
            TareasModificadas?.Invoke(this, EventArgs.Empty);
        }

        public void CambiarEstado(Guid id, TaskStatus nuevoEstado)
        {
            var tarea = _repositorio.ObtenerPorId(id);
            if (tarea == null) return;
            tarea.Estado = nuevoEstado;
            _repositorio.Actualizar(tarea);
            TareasModificadas?.Invoke(this, EventArgs.Empty);
        }
    }
}
