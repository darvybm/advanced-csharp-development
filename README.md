# <p align="center">P2 - Lógica Flexible: Task Management API con Delegados, Funciones Anónimas, Action y Func</p>
<p align="center">
  <img src="https://img.shields.io/badge/C%23-9B4D96?style=for-the-badge&logo=csharp&logoColor=white" alt="C# Badge" />
  <img src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=.net&logoColor=white" alt=".NET Badge" />
  <img src="https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" alt="SQL Server Badge" />
  <img src="https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white" alt="Docker Badge" />
  <img src="https://img.shields.io/badge/Delegates-F28DB2?style=for-the-badge" alt="Delegates Badge" />
  <img src="https://img.shields.io/badge/Action%2FFunc-FFD580?style=for-the-badge" alt="Action Func Badge" />
</p>

---

## Descripción del Proyecto

Esta versión mejorada de la API RESTful de gestión de tareas incorpora **una lógica más flexible y reutilizable**. La implementación utiliza potentes características del lenguaje C# como:

- Delegados personalizados para encapsular validaciones.
- Funciones anónimas para aplicar filtros dinámicos en tiempo de ejecución.
- Action para centralizar el sistema de notificaciones.
- Func para enriquecer las respuestas con lógica de cálculo adicional (como días restantes).

---

## Índice

1. [Tecnologías Utilizadas](#tecnologías-utilizadas)
2. [Novedades Principales](#novedades-principales)
3. [Arquitectura del Proyecto](#arquitectura-del-proyecto)
4. [Estructura de Carpetas](#estructura-de-carpetas)
5. [Endpoints RESTful](#endpoints-restful)
6. [Configuración de SQL Server con Docker](#configuración-de-sql-server-con-docker)
7. [Manejo de Errores](#manejo-de-errores)
8. [Cómo Ejecutar el Proyecto](#cómo-ejecutar-el-proyecto)

---

## Tecnologías Utilizadas

* **C# / ASP.NET Core**
* **SQL Server 2022** en contenedor Docker
* **Swagger**
* **LINQ avanzado**
* **Delegates / Func / Action**

---

## Novedades Principales

### ✅ Validación con Delegado Personalizado

```csharp
public delegate bool TaskValidator(TaskRequest request);
private readonly TaskValidator _validator = req =>
    !string.IsNullOrWhiteSpace(req.Description) && req.DueDate > DateTime.Now;
```

### ✅ Notificaciones con Action

```csharp
private readonly Action<string> _notify = msg =>
    Console.WriteLine($"[NOTIFICACIÓN] {msg}");
```

### ✅ Func para enriquecer la respuesta con días restantes

```csharp
private readonly Func<TaskModel, object> _taskWithExtras = t => new
{
    t.Id,
    t.Description,
    t.DueDate,
    t.Status,
    t.ExtraData,
    DaysLeft = (t.DueDate - DateTime.Now).Days
};
```

### ✅ Filtros dinámicos en `GET /api/task`

```csharp
if (!string.IsNullOrWhiteSpace(status))
    tasks = tasks.Where(t => t.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();

if (!string.IsNullOrWhiteSpace(search))
    tasks = tasks.Where(t => t.Description.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

if (dueBefore.HasValue)
    tasks = tasks.Where(t => t.DueDate < dueBefore.Value).ToList();
```

---

## Arquitectura del Proyecto

Sigue el mismo patrón modular que en la primera práctica:

- **DTOs**: intercambio de datos.
- **Models**: entidades de dominio.
- **Repositories**: acceso a datos.
- **Services**: lógica de negocio con funciones flexibles.
- **Controllers**: exposición de endpoints REST.

---

## Estructura de Carpetas

```
TaskManagementAPI/
│
├── Controllers/
│   └── TaskController.cs
│
├── Models/
│   └── TaskModel.cs
│   └── Data/
│       └── AppDbContext.cs
│
├── DTOs/
│   └── TaskRequest.cs
│   └── TaskResponse.cs
│
├── Repositories/
│   └── TaskRepository.cs
│
├── Services/
│   └── TaskService.cs
│
└── Config/
    └── ErrorHandler.cs
```

---

## Endpoints RESTful

1. **GET /api/task?status=&search=&dueBefore=**

   * Devuelve una lista de tareas aplicando filtros dinámicos opcionales.

2. **GET /api/task/{id}**

   * Devuelve una tarea específica.

3. **POST /api/task**

   * Crea una nueva tarea validada mediante un delegado personalizado.

4. **PUT /api/task/{id}**

   * Actualiza una tarea y notifica al sistema.

5. **DELETE /api/task/{id}**

   * Elimina una tarea y muestra un mensaje de notificación.

---

## Configuración de SQL Server con Docker

Este proyecto utiliza SQL Server 2022 ejecutado en un contenedor Docker. A continuación se detallan los archivos necesarios:

### 1. Archivo `docker-compose.yml`

```yaml
version: '3.9'

services:
  sql-server:
    container_name: sql-server
    image: mcr.microsoft.com/mssql/server:2022-latest 
    environment:
      ACCEPT_EULA: ${ACCEPT_EULA}
      MSSQL_SA_PASSWORD: ${MSSQL_SA_PASSWORD}
      MSSQL_PID: ${MSSQL_PID}
      MSSQL_TCP_PORT: ${MSSQL_TCP_PORT}
    ports:
      - "${MSSQL_TCP_PORT}:${MSSQL_TCP_PORT}"
    volumes:
      - sql-volume:/var/opt/mssql
    networks:
      - sql-network

volumes:
  sql-volume:

networks:
  sql-network:
```

### 2. Archivo `.env`

```env
ACCEPT_EULA=Y
MSSQL_SA_PASSWORD=Darvy!BM
MSSQL_PID=Developer
MSSQL_TCP_PORT=1433
```

### 3. Cadena de conexión en `appsettings.json`

Asegúrate de que tu archivo `appsettings.json` contenga la siguiente conexión:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=TaskDb;User Id=sa;Password=Darvy!BM;"
}
```

### 4. Inicializar el contenedor

Ejecuta el siguiente comando para levantar SQL Server:

```bash
docker-compose up -d
```

Esto creará un contenedor disponible en `localhost:1433`.

### 5. Crear la base de datos y la tabla `Tasks`

Puedes conectarte usando Azure Data Studio, DBeaver o `sqlcmd` y ejecutar lo siguiente:

```sql
CREATE DATABASE TaskDb;
GO

USE TaskDb;
GO

CREATE TABLE Tasks (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Description NVARCHAR(MAX) NOT NULL,
    DueDate DATETIME2 NOT NULL,
    Status NVARCHAR(MAX) NOT NULL,
    ExtraData NVARCHAR(MAX)
);
```

---

## Manejo de Errores

Se mantiene el mismo manejador de errores estructurado:

```json
{
  "success": false,
  "message": "Ocurrió un error inesperado.",
  "detail": "Mensaje de error detallado"
}
```

Además, las respuestas están encapsuladas en `TaskResponse<T>`:

```csharp
public class TaskResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
    public string? ErrorDetail { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static TaskResponse<T> Ok(T data, string message = "Operación exitosa.") { ... }
    public static TaskResponse<T> Fail(string message, string? errorDetail = null) { ... }
}
```

---

## Cómo Ejecutar el Proyecto

1. **Clona el repositorio y cambia a la rama correspondiente**:

```bash
git clone https://github.com/darvybm/advanced-csharp-development.git
cd advanced-csharp-development
git checkout practicas/p2-logica-flexible
```

2. **Levanta SQL Server con Docker** (si no lo has hecho ya):

```bash
docker-compose up -d
```

3. **Ejecuta la API**:

```bash
dotnet run
```

4. **Abre Swagger para probar los endpoints**:

```
http://localhost:5000/swagger
```
