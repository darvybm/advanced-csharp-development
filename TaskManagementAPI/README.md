# <p align="center"> P1 - Conceptos Preliminares: Task Management API</p>
<p align="center">
  <img src="https://img.shields.io/badge/C%23-9B4D96?style=for-the-badge&logo=csharp&logoColor=white" alt="C# Badge" />
  <img src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=.net&logoColor=white" alt=".NET Badge" />
  <img src="https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" alt="SQL Server Badge" />
  <img src="https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white" alt="Docker Badge" />
  <img src="https://img.shields.io/badge/API%20Design-FF5C5C?style=for-the-badge" alt="API Design Badge" />
  <img src="https://img.shields.io/badge/Error%20Handling-EAB308?style=for-the-badge" alt="Error Handling Badge" />
  <img src="https://img.shields.io/badge/DTO-FFB0A6?style=for-the-badge" alt="DTO Badge" />
</p>


Este proyecto es una API RESTful construida con **ASP.NET Core** que permite gestionar tareas. La API sigue buenas prácticas de desarrollo y arquitectura modular, utilizando tecnologías como **SQL Server** en una instancia Dockerizada, validaciones, manejo de errores centralizado, y patrones de diseño como Repositorios, Servicios y Controladores.

## Índice

1. [Descripción del Proyecto](#descripción-del-proyecto)
2. [Tecnologías Utilizadas](#tecnologías-utilizadas)
3. [Arquitectura del Proyecto](#arquitectura-del-proyecto)
4. [Estructura de Carpetas](#estructura-de-carpetas)
5. [Endpoints](#endpoints)
6. [Configuración de SQL Server con Docker](#configuración-de-sql-server-con-docker)
7. [Manejo de Errores](#manejo-de-errores)
8. [Cómo Ejecutar el Proyecto](#cómo-ejecutar-el-proyecto)

---

## Descripción del Proyecto

Este proyecto es una API de gestión de tareas que permite crear, leer, actualizar y eliminar tareas a través de endpoints RESTful. Los datos se almacenan en **SQL Server** y el proyecto está diseñado siguiendo una arquitectura modular y organizada.

El objetivo principal es mantener el código limpio, escalable y fácil de mantener, integrando los mejores patrones de diseño como:

* **DTOs**: para la transferencia de datos.
* **Servicios**: para la lógica de negocio.
* **Repositorios**: para la interacción con la base de datos.
* **Controladores**: para la exposición de la API.
* **Manejo de errores centralizado**: para capturar excepciones y retornar respuestas estructuradas.

---

## Tecnologías Utilizadas

* **C# / ASP.NET Core**: Framework principal para la creación de la API.
* **SQL Server**: Base de datos relacional utilizada para el almacenamiento de datos.
* **Docker**: Para ejecutar SQL Server en contenedores.
* **Swagger**: Para la documentación y pruebas de la API.

---

## Arquitectura del Proyecto

La arquitectura del proyecto está organizada en varias capas, cada una con responsabilidades específicas, lo que facilita la escalabilidad y el mantenimiento del código.

1. **DTOs**: Son los objetos que se utilizan para recibir y enviar datos entre el cliente y la API.
2. **Models**: Son las entidades de negocio que representan los objetos de la base de datos.
3. **Data**: Contiene las clases relacionadas con la configuración de la base de datos y el contexto.
4. **Repositories**: Definen los métodos para interactuar con la base de datos.
5. **Services**: Contienen la lógica de negocio.
6. **Controllers**: Exponen los endpoints de la API.

---

## Estructura de Carpetas

```
TaskManagementAPI/
│
├── Controllers/
│   └── TaskController.cs
│
├── Models/
│   ├── TaskModel.cs
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

## Endpoints

1. **GET /api/task**

   * Devuelve todas las tareas en la base de datos.

2. **GET /api/task/{id}**

   * Devuelve una tarea específica según el `id`.

3. **POST /api/task**

   * Crea una nueva tarea. Requiere un `TaskRequest` con la descripción, fecha de vencimiento y otros detalles.

4. **PUT /api/task/{id}**

   * Actualiza una tarea existente. Requiere un `TaskRequest` con los nuevos datos.

5. **DELETE /api/task/{id}**

   * Elimina una tarea específica.

---

## Configuración de SQL Server con Docker

Este proyecto utiliza **SQL Server 2022** ejecutado en un contenedor Docker. A continuación se muestra cómo configurar y levantar la base de datos localmente.

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

Define las variables de entorno utilizadas en `docker-compose.yml`:

```
ACCEPT_EULA=Y
MSSQL_SA_PASSWORD=Darvy!BM
MSSQL_PID=Developer
MSSQL_TCP_PORT=1433
```

### 3. Cadena de conexión en `appsettings.json`

Asegúrate de usar esta cadena de conexión en tu archivo de configuración de la API:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=TaskDb;User Id=sa;Password=Darvy!BM;"
}
```

### 4. Inicializar el contenedor

Ejecuta el siguiente comando para levantar la base de datos:

```bash
docker-compose up -d
```

Esto creará un contenedor de SQL Server disponible en `localhost:1433`.

### 5. Crear la base de datos y tabla manualmente

Conéctate al contenedor (puedes usar Azure Data Studio, DBeaver, o `sqlcmd`) y ejecuta los siguientes comandos SQL para crear la base de datos y la tabla principal:

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

La API implementa un **manejador de errores centralizado** que captura excepciones no controladas y devuelve una respuesta JSON estructurada.

### Ejemplo de respuesta de error

```json
{
    "success": false,
    "message": "Ocurrió un error inesperado.",
    "detail": "Mensaje de error detallado"
}
```

También se utiliza la clase genérica `TaskResponse<T>` para unificar las respuestas exitosas y fallidas:

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

1. **Clona el repositorio**:

   ```bash
   git clone https://github.com/tu-usuario/task-management-api.git
   cd task-management-api
   ```

2. **Ejecuta SQL Server en Docker**:

3. **Instala las dependencias y ejecuta la API**:

   * Abre el proyecto en Visual Studio o ejecuta desde la línea de comandos:

   ```bash
   dotnet run
   ```

4. **Accede a la documentación Swagger**:

   * La documentación de la API estará disponible en `http://localhost:5000/swagger`.
