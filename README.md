# ApiEcommerce

Proyecto de API RESTful para gestión de productos, categorías y usuarios en un sistema de ecommerce, desarrollado en ASP.NET Core 9.0.

## Características principales
- **Autenticación y autorización** con JWT y roles (Admin, User).
- **Versionado de API** (v1, v2) usando `Asp.Versioning`.
- **Mapeo de objetos** usando Mapster (migrado desde AutoMapper).
- **Documentación interactiva** con Scalar y OpenAPI.
- **CORS configurable** y caché de respuestas.
- **Carga y gestión de imágenes de productos**.
- **Persistencia** con Entity Framework Core y SQL Server.
- **Migraciones** y seeding de datos iniciales.

## Estructura del proyecto
- `Controllers/` — Controladores para pro ductos, usuarios y categorías (v1 y v2).
- `Models/` — Modelos de dominio y DTOs.
- `Repository/` — Repositorios y lógica de acceso a datos.
- `Mapping/` — Configuración de mapeos con Mapster.
- `Data/` — Contexto de base de datos y seeding.
- `Migrations/` — Migraciones de Entity Framework.
- `wwwroot/ProductsImages/` — Imágenes de productos.

## Instalación y configuración
1. **Clona el repositorio:**
   ```sh
   git clone <url-del-repo>
   cd ApiEcommerce
   ```
2. **Restaura los paquetes NuGet:**
   ```sh
   dotnet restore ApiEcommerce/ApiEcommerce.csproj
   ```
3. **Configura la cadena de conexión** en `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "ConexionSql": "Server=...;Database=...;User Id=...;Password=...;"
   }
   ```
4. **Aplica las migraciones (opcional):**
   ```sh
   dotnet ef database update --project ApiEcommerce/ApiEcommerce.csproj
   ```
5. **Ejecuta la API:**
   ```sh
   dotnet run --project ApiEcommerce/ApiEcommerce.csproj
   ```

## Paquetes NuGet principales
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
- `Asp.Versioning.Mvc`
- `Mapster` y `Mapster.DependencyInjection`
- `Scalar.AspNetCore`

Instala cualquier paquete faltante con:
```sh
dotnet add ApiEcommerce/ApiEcommerce.csproj package <NombreDelPaquete>
```

## Notas
- El proyecto está preparado para desarrollo local y despliegue en producción.
- Para pruebas de endpoints, puedes usar el archivo `ApiEcommerce.http` o herramientas como Postman.

---

