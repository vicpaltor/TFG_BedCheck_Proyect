\# Registro de Cambios (Changelog)



Todos los cambios notables en el proyecto BedCheck serán documentados en este archivo.



\## \[1.1.0] - 2025-11-20 (Fase de Refactorización y Arquitectura)

\### Agregado

\- \*\*Logs\*\*: Implementación de Serilog con guardado diario en archivos.

\- \*\*Monitorización\*\*: Endpoint `/health` para verificar estado de BD y App.

\- \*\*Documentación\*\*: Swagger UI configurado y mostrando endpoints de API.

\- \*\*Frontend\*\*: Integración de DataTables.js con AJAX para la tabla de Camas.

\- \*\*Tests\*\*: Proyecto de pruebas unitarias (xUnit + Moq) para DTOs y Controladores.



\### Cambiado

\- \*\*Arquitectura\*\*: Migración de lógica en `CamasController` para usar DTOs.

\- \*\*Mapeo\*\*: Implementación de AutoMapper para conversión Entidad <-> DTO.

\- \*\*Validación\*\*: Reemplazo de validación manual por FluentValidation.



\### Corregido

\- Actualización de dependencias a .NET 8.0.11 (LTS).

\- Limpieza de archivos basura en el repositorio Git.



\## \[1.0.0] - 2024-09-01 (Versión Inicial)

\### Agregado

\- CRUD básico de Camas, Pacientes y Habitaciones.

\- Sistema de autenticación con Identity.

