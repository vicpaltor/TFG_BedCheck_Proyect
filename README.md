# 🏥 TFG BedCheck - Sistema de Control de Camas Hospitalarias

![Status](https://img.shields.io/badge/Estado-En_Desarrollo-yellow)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![Build](https://img.shields.io/badge/Build-Passing-brightgreen)

## 📋 Descripción del Proyecto

**BedCheck** es un Trabajo de Fin de Grado (TFG) desarrollado en **ASP.NET Core 8.0 MVC** para la gestión integral de recursos hospitalarios. El sistema moderniza el control de camas, pacientes y personal mediante una arquitectura robusta, escalable y orientada a servicios.

El proyecto va más allá de un CRUD básico, implementando patrones de diseño avanzados, validaciones robustas, monitoreo de salud del sistema y documentación automática de API.

---

## 🚀 Mejoras Técnicas e Innovaciones (Novedades)

Este proyecto implementa prácticas de desarrollo profesional modernas:

### 🛡️ Arquitectura y Seguridad
- **Patrón DTO (Data Transfer Objects)**: Desacoplamiento total entre la Base de Datos y la Vista usando `AutoMapper`.
- **FluentValidation**: Reglas de validación de negocio separadas de los modelos para un código más limpio.
- **Middleware Personalizado**: Gestión global de excepciones para evitar errores no controlados.

### 👁️ Observabilidad y Documentación
- **Serilog**: Sistema de Logging estructurado (escribe logs diarios en archivos de texto).
- **Health Checks**: Sistema de monitoreo de salud (`/health`) para verificar el estado de la BD y la App.
- **Swagger / OpenAPI**: Documentación automática e interactiva de la API REST interna.

### ⚡ Experiencia de Usuario (UX)
- **DataTables.js**: Tablas interactivas con búsqueda instantánea, paginación y ordenación asíncrona (AJAX).
- **Feedback Visual**: Uso de SweetAlert y Toastr para notificaciones al usuario.

### 🧪 Calidad de Código (Testing)
- **Pruebas Unitarias (xUnit)**: Tests automatizados para asegurar la calidad del código.
- **Mocking (Moq)**: Simulación de dependencias para probar controladores de forma aislada.

---

## 🏗️ Arquitectura del Proyecto

El proyecto sigue una arquitectura en capas estricta para asegurar la mantenibilidad:

```text
BedCheck.sln
│
├── 🌐 BedCheck (Capa Web / Presentación)
│   ├── Areas/                  # Módulos (Admin, Empleado, etc.)
│   ├── Controllers/            # Controladores MVC y API
│   ├── Mapping/                # Configuraciones de AutoMapper
│   ├── Middleware/             # Gestión de errores y pipeline
│   ├── Views/                  # Interfaz de usuario (Razor)
│   └── wwwroot/
│       └── js/camas.js         # Lógica DataTables
│
├── 🗄️ BedCheck.AccesoDatos (Persistencia)
│   ├── Data/                   # DbContext
│   ├── Repository/             # Implementación Patrón Repositorio
│   └── Migrations/             # Historial de cambios de BD
│
├── 📦 BedCheck.Models (Dominio)
│   ├── DTOs/                   # Objetos de Transferencia de Datos (Seguros)
│   ├── Validators/             # Reglas de FluentValidation
│   ├── ViewModels/             # Modelos específicos para Vistas
│   └── Entidades/              # (Cama, Paciente, etc.)
│
├── 🧪 BedCheck.Tests (Quality Assurance)
│   ├── UnitTests/              # Pruebas de DTOs y Lógica
│   └── ControllerTests/        # Pruebas de Controladores con Moq
│
└── 🛠️ BedCheck.Utilidades (Transversal)
    └── Constantes y Helpers

## 💻 Stack Tecnológico

| Categoría | Tecnologías |
|-----------|-------------|
| **Core** | .NET 8.0, C# 12 |
| **Datos** | SQL Server, Entity Framework Core 8.0.11 |
| **Arquitectura** | Repository Pattern, Unit of Work, DTOs |
| **Validación** | FluentValidation.AspNetCore |
| **Mapeo** | AutoMapper |
| **Logging** | Serilog |
| **Testing** | xUnit, Moq |
| **Frontend** | Bootstrap 5, DataTables.js, jQuery |
| **API Doc** | Swashbuckle (Swagger UI) |

---

## 🔧 Configuración y Ejecución

### Requisitos Previos
- Visual Studio 2022 (v17.8 o superior)
- .NET 8.0 SDK
- SQL Server LocalDB o Express

### Pasos para iniciar

1. **Clonar el repositorio**
   ```bash
   git clone https://github.com/tu-usuario/BedCheck.git

2. **Restaurar dependencias**
   ```bash
   dotnet restore
3. **Configurar Base de Datos**
Abre el archivo appsettings.json en el proyecto BedCheck y verifica tu cadena de conexión. Luego ejecuta las migraciones para crear la base de datos:
```bash
dotnet ef database update --project BedCheck.AccesoDatos

4. **Ejecutar la aplicación**
Presiona F5 en Visual Studio o ejecuta el siguiente comando en la terminal:
```bash
dotnet run --project BedCheck

### Endpoints de Interés
Una vez iniciada la aplicación, puedes acceder a las nuevas funcionalidades implementadas:

- **Web Principal**: `https://localhost:PORT/`
- **Documentación API (Swagger)**: `https://localhost:PORT/swagger`
- **Monitor de Salud**: `https://localhost:PORT/health`

*(Nota: Reemplaza `PORT` por el puerto que te asigne Visual Studio, por ejemplo: 5134)*

---

## 🧪 Ejecución de Pruebas

Para verificar la integridad del sistema y ejecutar los tests unitarios (xUnit + Moq):

**Opción A: Desde Visual Studio**
1. Abre el menú **Prueba** > **Explorador de Pruebas**.
2. Haz clic en el botón **Ejecutar todas**.

**Opción B: Desde consola**
```bash
dotnet test

## 👨‍💻 Autor

**Victor Manuel Palos Torres**  
Trabajo de Fin de Grado - Ingeniería Informática  
Universidad de Sevilla  
Curso 2024/2025

---

## 📄 Licencia

Este proyecto es material académico desarrollado exclusivamente para fines educativos y de evaluación como Trabajo de Fin de Grado.




