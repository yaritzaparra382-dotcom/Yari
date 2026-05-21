using System;
using TaskManager.Models;

namespace TaskManager.Factory
{
    public static class TaskFactory
    {
        public static TaskItem Crear(string titulo, string descripcion, TaskPriority prioridad, DateTime? fechaVencimiento = null)
        {
            return new TaskItem
            {
                Id = Guid.NewGuid(),
                Titulo = titulo,
                Descripcion = descripcion,
                Prioridad = prioridad,
                Estado = TaskStatus.Pendiente,
                FechaCreacion = DateTime.Now,
                FechaVencimiento = fechaVencimiento
            };
        }

        public static TaskItem CrearSimple(string titulo)
        {
            return Crear(titulo, "", TaskPriority.Media);
        }

        public static TaskItem CrearUrgente(string titulo, string descripcion)
        {
            var tarea = Crear(titulo, descripcion, TaskPriority.Alta);
            tarea.FechaVencimiento = DateTime.Now.AddDays(1);
            return tarea;
        }
    }
}
