# TFG BedCheck - Sistema de Control de Camas Hospitalarias

## 📋 Descripción del Proyecto

**BedCheck** es un Trabajo de Fin de Grado (TFG) que consiste en una aplicación web desarrollada en **ASP.NET Core MVC** utilizando **C#** y **.NET 8.0** para la gestión y control de camas hospitalarias.

El sistema permite realizar un seguimiento completo del estado de las camas, habitaciones, pacientes, operaciones y personal de enfermería en un entorno hospitalario.

## 🎯 Objetivos del TFG

- Desarrollar una aplicación web funcional para la gestión hospitalaria
- Implementar un sistema de autenticación y autorización basado en roles
- Crear una arquitectura en capas siguiendo principios de clean code
- Utilizar Entity Framework Core para la persistencia de datos
- Aplicar patrones de diseño como Repository y Unit of Work

## 🏗️ Arquitectura del Proyecto

El proyecto está estructurado en **4 capas** principales:

```
BedCheck.sln
│
├── BedCheck                          # Capa de presentación (MVC)
│   ├── Areas/
│   ├── Controllers/
│   ├── Views/
│   └── wwwroot/
│
├── BedCheck.AccesoDatos              # Capa de acceso a datos
│   ├── Data/
│   ├── Repository/
│   └── Migrations/
│
├── BedCheck.Models                   # Capa de modelos de dominio
│   ├── Cama.cs
│   ├── Habitacion.cs
│   ├── Paciente.cs
│   ├── Operacion.cs
│   └── Enfermero.cs
│
└── BedCheck.Utilidades              # Capa de utilidades y constantes
    └── CNT.cs
```

## 🔑 Entidades Principales

- **Cama**: Gestión de camas individuales con estado, tipo y asignación
- **Habitación**: Control de habitaciones y capacidad
- **Paciente**: Información de pacientes, enfermedades y tratamientos
- **Operación**: Registro de operaciones quirúrgicas
- **Enfermero**: Gestión del personal de enfermería
- **ApplicationUser**: Usuario del sistema con roles definidos

## 💻 Tecnologías Utilizadas

- **Framework**: ASP.NET Core 8.0 MVC
- **Lenguaje**: C# 12
- **ORM**: Entity Framework Core 8.0
- **Base de Datos**: SQL Server
- **Autenticación**: ASP.NET Core Identity
- **Patrón de diseño**: Repository Pattern, Unit of Work
- **Frontend**: Razor Pages, HTML5, CSS3, JavaScript

## 📦 Dependencias Principales

```xml
- Microsoft.AspNetCore.Identity.EntityFrameworkCore (8.0.8)
- Microsoft.EntityFrameworkCore.SqlServer (8.0.8)
- Microsoft.EntityFrameworkCore.Tools (8.0.8)
- Microsoft.AspNetCore.Mvc.ViewFeatures
```

## 🔧 Configuración del Proyecto

### Requisitos Previos

- Visual Studio 2022 (v17.11 o superior)
- .NET 8.0 SDK
- SQL Server 2019 o superior

### Configuración de Base de Datos

1. Actualizar la cadena de conexión en `appsettings.json`:

```json
"ConnectionStrings": {
  "ConexionSQL": "Server=TU_SERVIDOR;Database=BedCheckBDNET8;User ID=sa;Password=TU_PASSWORD;Trusted_Connection=true;Encrypt=false;MultipleActiveResultSets=true"
}
```

2. Ejecutar las migraciones:

```bash
dotnet ef database update
```

### Roles de Usuario

El sistema implementa tres roles principales:

- **Administrador**: Control total del sistema
- **Enfermero**: Gestión de pacientes y camas
- **Cliente**: Consulta de información

## 🚀 Ejecución del Proyecto

1. Clonar el repositorio
2. Abrir `BedCheck.sln` en Visual Studio
3. Configurar la cadena de conexión
4. Restaurar paquetes NuGet
5. Ejecutar las migraciones
6. Presionar F5 para ejecutar

## 📊 Documentación Adicional

### Generación de Diagramas

Para generar la documentación visual del proyecto:

1. **Crear flechas con codos redondeados**:
   - Click derecho en la flecha
   - Line Style > Orthogonal rounded

2. **Generar tablas y diagramas**:
   - Click derecho en la flecha
   - Generate Documentation
   - Template (cargar plantilla)
   - Generate

## 📁 Estructura de Carpetas

```
wwwroot/
└── imagenes/
    └── camas/          # Imágenes de las camas
```

## 👨‍💻 Autor

Victor Manuel Palos Torres
Trabajo de Fin de Grado - ASP.NET Core MVC  
Universidad de sevilla
2024/2025

## 📄 Licencia

Este proyecto es material académico desarrollado como Trabajo de Fin de Grado.

## 📝 Notas del Desarrollo

- La aplicación utiliza Areas para organizar las diferentes secciones
- Se implementa el patrón Repository para abstraer el acceso a datos
- El sistema de autenticación usa ASP.NET Core Identity
- La ruta por defecto del sistema es: `{area=Empleado}/{controller=Home}/{action=Index}`

---

**Estado del Proyecto**: En Desarrollo  
**Versión**: 1.0.0  
**Fecha de Última Actualización**: [Fecha]
