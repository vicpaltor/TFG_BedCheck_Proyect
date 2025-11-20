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
