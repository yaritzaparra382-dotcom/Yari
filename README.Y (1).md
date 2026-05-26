# 📝 Task Manager - WinForms

Aplicación de escritorio para gestión de tareas personales desarrollada en C# con WinForms.

**Materia:** Herramientas de Programación II

| Nombre |
|---|
| Julián Camilo Cabrera Echeverri |
| Juan José Uribe Castañeda |
| Yaritza Parra Mazo |

---

## 🚀 Funcionalidades

- CRUD de tareas
- Filtros por estado y prioridad
- Persistencia en JSON
- Interfaz amigable

---

## 🏗️ Arquitectura

Se utiliza una arquitectura en capas:

- **UI (Forms):** Maneja interacción con el usuario
- **Services:** Lógica de negocio
- **Repository:** Persistencia de datos
- **Models:** Entidades

Esto permite separación de responsabilidades, facilidad de mantenimiento y escalabilidad.

---

## 📂 Estructura

```
TaskManager/
├── Models/
│   ├── TaskItem.cs
│   ├── TaskPriority.cs
│   └── TaskStatus.cs
├── Interfaces/
│   ├── ITaskRepository.cs
│   └── ITaskService.cs
├── Repository/
│   └── JsonTaskRepository.cs
├── Services/
│   └── TaskService.cs
├── Factory/
│   └── TaskFactory.cs
├── Helpers/
│   └── EnumTranslator.cs
├── Forms/
│   ├── MainForm.cs
│   └── TaskForm.cs
├── data/
│   └── tareas.json
└── Program.cs
```

---

## 🧩 Patrones de diseño

### 🔹 Repository Pattern
Encapsula el acceso a datos.

- Ejemplo: `JsonTaskRepository`
- Ventaja: permite cambiar JSON por base de datos sin afectar la lógica

### 🔹 Factory Method
Centraliza la creación de objetos `TaskItem`.

- Ejemplo: `TaskFactory.Crear()`, `TaskFactory.CrearUrgente()`
- Ventaja: evita instanciar la clase directamente en múltiples lugares

### 🔹 Observer
El servicio notifica a la UI cuando hay cambios.

- Ejemplo: evento `TareasModificadas` en `TaskService`
- Ventaja: la UI se actualiza automáticamente sin acoplamiento

### 🔹 Dependency Injection (manual)
El servicio recibe el repositorio por constructor:

```csharp
var repo = new JsonTaskRepository("data/tareas.json");
var servicio = new TaskService(repo);
```

- Ventaja: bajo acoplamiento entre capas

---

## 🧠 Principios SOLID

### S — Single Responsibility
Cada clase tiene una sola responsabilidad:
- `TaskService` → lógica de negocio
- `JsonTaskRepository` → persistencia
- `MainForm` → interfaz de usuario

### O — Open/Closed
El sistema se puede extender sin modificar código existente. Ejemplo: crear otro repositorio (SQL, API) implementando `ITaskRepository`.

### L — Liskov Substitution
Las implementaciones cumplen sus contratos: `JsonTaskRepository` puede reemplazar a `ITaskRepository` sin romper el sistema.

### I — Interface Segregation
Interfaces específicas y separadas:
- `ITaskService`
- `ITaskRepository`

### D — Dependency Inversion
Se depende de abstracciones:

```csharp
ITaskService _servicio;
ITaskRepository _repositorio;
```

---

## ▶️ Cómo ejecutar

**Requisitos:**
- Visual Studio 2022 o superior
- .NET 8 SDK
- Windows

**Pasos:**
1. Clonar el repositorio
```bash
git clone <url-del-repo>
```
2. Abrir `TaskManager.csproj` en Visual Studio
3. Presionar `F5`

La primera vez se crea automáticamente el archivo `data/tareas.json`.

---

## 📘 Manual de usuario

### Pantalla principal
Muestra la lista de tareas con título, descripción, prioridad, estado y fecha de vencimiento.

### ➕ Agregar tarea
1. Clic en **+ Agregar**
2. Completar el formulario
3. Clic en **Guardar**

### ✏️ Editar tarea
1. Seleccionar una tarea de la lista
2. Clic en **Editar**
3. Modificar los datos
4. Clic en **Guardar**

### ❌ Eliminar tarea
1. Seleccionar una tarea de la lista
2. Clic en **Eliminar**
3. Confirmar la acción

### 🔍 Filtros
- Filtrar por **Estado:** Pendiente, En progreso, Completada
- Filtrar por **Prioridad:** Baja, Media, Alta

### 🎨 Colores
- 🔴 Alta → rojo claro
- 🟡 Media → amarillo claro
- ⚪ Baja → blanco
