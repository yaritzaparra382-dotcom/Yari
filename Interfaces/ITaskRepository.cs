using System;
using System.Collections.Generic;
using TaskManager.Models;

namespace TaskManager.Interfaces
{
    public interface ITaskRepository
    {
        List<TaskItem> ObtenerTodos();
        TaskItem ObtenerPorId(Guid id);
        void Agregar(TaskItem tarea);
        void Actualizar(TaskItem tarea);
        void Eliminar(Guid id);
        void Guardar();
    }
}
