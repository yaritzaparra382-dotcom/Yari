using System;

namespace TaskManager.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public TaskPriority Prioridad { get; set; }
        public TaskStatus Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaVencimiento { get; set; }

        public TaskItem()
        {
            Id = Guid.NewGuid();
            FechaCreacion = DateTime.Now;
            Estado = TaskStatus.Pendiente;
            Prioridad = TaskPriority.Media;
        }
    }
}
