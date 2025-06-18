# User Management Microservice

Microservicio para la gestión integral de usuarios, roles y permisos, diseñado para integrarse en arquitecturas de microservicios o aplicaciones modulares.

---

## Características principales

- Gestión completa de usuarios: creación, actualización, eliminación (soft delete) y consultas.
- Administración de roles y permisos con relaciones flexibles (UserRole, RolePermission).
- Autenticación mediante JWT con endpoints para login y registro.
- Validaciones de disponibilidad de usuario y correo electrónico.
- Arquitectura basada en servicios con separación clara de responsabilidades.
- Uso del patrón **Unit of Work** para manejo transaccional de repositorios.
- Integración con Prometheus para la recolección de métricas.
- Dashboard básico de monitoreo con Grafana.
- Logging estructurado con `ILogger`.
- Endpoints RESTful protegidos con autorización.

---

## Tecnologías usadas

- **ASP.NET Core Web API**
- **C#**
- **JWT** para autenticación y autorización
- **Entity Framework Core** (o el ORM que uses) con patrón Unit of Work
- **Prometheus** para recolección de métricas
- **Grafana** para visualización y monitoreo
- **Inyección de dependencias** para facilitar testing y mantenimiento
- **Swagger / OpenAPI** (recomendado para documentación de API)

---

## Requisitos previos

- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- Base de datos compatible (SQL Server, PostgreSQL, etc.)
- Servidor Prometheus configurado para scrapear las métricas expuestas
- Instancia de Grafana para visualizar dashboards
- Configuración de variables de entorno para JWT, base de datos y endpoints

---

## Instalación y configuración

1. Clonar el repositorio  
   ```bash
   git clone https://github.com/tuusuario/user-management-microservice.git
   cd user-management-microservice
