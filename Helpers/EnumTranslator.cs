using TaskManager.Models;

namespace TaskManager.Helpers
{
    // convierte los enums a texto para mostrar en la UI
    public static class EnumTranslator
    {
        public static string Traducir(TaskPriority prioridad) => prioridad switch
        {
            TaskPriority.Baja  => "Baja",
            TaskPriority.Media => "Media",
            TaskPriority.Alta  => "Alta",
            _                  => prioridad.ToString()
        };

        public static string Traducir(TaskStatus estado) => estado switch
        {
            TaskStatus.Pendiente   => "Pendiente",
            TaskStatus.EnProgreso  => "En progreso",
            TaskStatus.Completada  => "Completada",
            _                      => estado.ToString()
        };

        public static string[] OpcionesPrioridad => new[] { "Todas", "Baja", "Media", "Alta" };
        public static string[] OpcionesEstado    => new[] { "Todos", "Pendiente", "En progreso", "Completada" };

        public static TaskPriority APrioridad(string texto) => texto switch
        {
            "Baja"  => TaskPriority.Baja,
            "Media" => TaskPriority.Media,
            "Alta"  => TaskPriority.Alta,
            _       => TaskPriority.Media
        };

        public static TaskStatus AEstado(string texto) => texto switch
        {
            "Pendiente"   => TaskStatus.Pendiente,
            "En progreso" => TaskStatus.EnProgreso,
            "Completada"  => TaskStatus.Completada,
            _             => TaskStatus.Pendiente
        };
    }
}
