using System;
using System.Collections.Generic;
using TaskManager.Models;

namespace TaskManager.Interfaces
{
    public interface ITaskService
    {
        List<TaskItem> ObtenerTareas();
        List<TaskItem> FiltrarPorEstado(TaskStatus estado);
        List<TaskItem> FiltrarPorPrioridad(TaskPriority prioridad);
        void CrearTarea(string titulo, string descripcion, TaskPriority prioridad, DateTime? fechaVencimiento);
        void ActualizarTarea(TaskItem tarea);
        void EliminarTarea(Guid id);
        void CambiarEstado(Guid id, TaskStatus nuevoEstado);
    }
}
