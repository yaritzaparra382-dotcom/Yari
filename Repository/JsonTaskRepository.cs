using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Repository
{
    public class JsonTaskRepository : ITaskRepository
    {
        private readonly string _ruta;
        private List<TaskItem> _tareas;

        private readonly JsonSerializerOptions _opciones = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        public JsonTaskRepository(string ruta = "data/tareas.json")
        {
            _ruta = ruta;
            _tareas = Cargar();
        }

        public List<TaskItem> ObtenerTodos()
        {
            return _tareas;
        }

        public TaskItem ObtenerPorId(Guid id)
        {
            return _tareas.FirstOrDefault(t => t.Id == id);
        }

        public void Agregar(TaskItem tarea)
        {
            _tareas.Add(tarea);
            Guardar();
        }

        public void Actualizar(TaskItem tarea)
        {
            int i = _tareas.FindIndex(t => t.Id == tarea.Id);
            if (i == -1) return;
            _tareas[i] = tarea;
            Guardar();
        }

        public void Eliminar(Guid id)
        {
            var tarea = ObtenerPorId(id);
            if (tarea == null) return;
            _tareas.Remove(tarea);
            Guardar();
        }

        public void Guardar()
        {
            string carpeta = Path.GetDirectoryName(_ruta);
            if (!string.IsNullOrEmpty(carpeta))
                Directory.CreateDirectory(carpeta);

            string json = JsonSerializer.Serialize(_tareas, _opciones);
            File.WriteAllText(_ruta, json);
        }

        private List<TaskItem> Cargar()
        {
            if (!File.Exists(_ruta))
                return new List<TaskItem>();

            string json = File.ReadAllText(_ruta);
            return JsonSerializer.Deserialize<List<TaskItem>>(json, _opciones) ?? new List<TaskItem>();
        }
    }
}
