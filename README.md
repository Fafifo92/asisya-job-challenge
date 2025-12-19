# PruebaFullStack - Solución Técnica Arquitecto .NET + React

Este repositorio contiene la solución a la prueba técnica para el rol Desarrollador II. Implementa una API REST robusta, una SPA en React y una arquitectura escalable siguiendo Clean Architecture.

## Características Principales

*   **Arquitectura Limpia (Clean Architecture)**: Separación de responsabilidades en capas (Domain, Application, Infrastructure, API).
*   **Performance Extrema**: Carga dinámica productos optimizada utilizando `EFCore.BulkExtensions` (PostgreSQL COPY).
*   **Seguridad**: Autenticación JWT completa con interceptores en Frontend y Policies en Backend.
*   **Frontend Moderno**: React con Material UI, DataGrid con paginación server-side, react-hook-form y Context API.
*   **DevOps Ready**: Dockerfile optimizado, docker-compose para orquestación local y GitHub Actions para CI.

## Tecnologías

*   **Backend**: .NET 8, C#, EF Core, Npgsql, MediatR (patrón comando conceptual, implementado simple para claridad), FluentValidation.
*   **Base de Datos**: PostgreSQL 15.
*   **Frontend**: React 18, Vite, TypeScript, Material UI (MUI), Axios.
*   **Infraestructura**: Docker, GitHub Actions.

## Arquitectura

### Decisiones de Diseño
1.  **Monolito Modular**: Se eligió una estructura de monolito modular (separación lógica estricta) en lugar de microservicios para evitar la complejidad operativa innecesaria (overengineering) dado el alcance de la prueba, pero dejando la puerta abierta a la extracción de módulos (ej. `Products` vs `Identity`).
2.  **DTOs y Mapeo Explícito**: No se exponen entidades de dominio. Se usan DTOs para controlar exactamente qué entra y sale de la API, desacoplando la persistencia de la presentación.
3.  **Carga Masiva Síncrona**: Se optó por una estrategia síncrona optimizada (`BulkInsertAsync`) en lugar de colas asíncronas para este caso de uso específico. `BulkInsert` con PostgreSQL es extremadamente rápido (segundos para 100k registros), lo que simplifica la arquitectura al eliminar la necesidad de un worker/broker adicional (RabbitMQ) sin sacrificar la UX en este escenario de carga de prueba. Para escenarios reales de millones de registros concurrentes, se recomendaría moverlo a un Background Service.
4.  **Escalabilidad Horizontal**: La API es stateless. La sesión se maneja vía JWT. Esto permite desplegar múltiples réplicas de la API detrás de un Load Balancer (ej. Nginx o Azure App Service) sin cambios en el código. La base de datos puede escalar verticalmente o usar réplicas de lectura.

## Guía de Ejecución Local (Windows + Docker Desktop)

Sigue estos pasos para levantar todo el entorno en tu máquina local.

### Prerrequisitos
1.  Tener instalado **Docker Desktop** y asegurarse de que esté corriendo.
2.  (Opcional) Tener git instalado para clonar el repo.

### Paso 1: Clonar el Repositorio
Abre tu terminal (PowerShell o CMD) y ejecuta:
```bash
git clone https://github.com/Fafifo92/asisya-test-job.git
cd asisya-job-challenge
```

### Paso 2: Ejecutar con Docker Compose
Este comando construirá la imagen de la API, descargará Postgres y levantará ambos servicios conectados.

```bash
docker-compose up --build
```
*Espera unos segundos hasta ver que los logs indiquen que la aplicación ha iniciado.* La base de datos se inicializará automáticamente gracias a la configuración en `Program.cs`.

### Paso 3: Verificar Backend
Abre tu navegador y ve a:
*   **Swagger UI**: [http://localhost:5000/swagger](http://localhost:5000/swagger)
    *   Aquí puedes probar los endpoints directamente.
    *   Para probar la carga masiva:
        1.  Usa `/api/Auth/login` con `{ "username": "admin", "password": "password" }` para obtener el token.
        2.  Copia el token (sin comillas).
        3.  Haz click en el botón "Authorize" arriba a la derecha y pega `Bearer <TU_TOKEN>`.
        4.  Usa `/api/Product/seed` para generar 100,000 productos.

### Paso 4: Ejecutar Frontend (React)
El frontend está configurado para desarrollo local fuera del contenedor para mayor rapidez en la iteración (hot-reload), aunque podría dockerizarse fácilmente.

1.  Abre una **nueva terminal** (manteniendo la anterior corriendo).
2.  Navega a la carpeta del frontend:
    ```bash
    cd frontend
    ```
3.  Instala las dependencias y corre el servidor de desarrollo:
    ```bash
    npm install
    npm run dev
    ```
4.  Abre el navegador en la URL que te muestre la consola, (usualmente [http://localhost:5173](http://localhost:5173)).

### Paso 5: Login en la App
*   **Usuario**: admin
*   **Contraseña**: password

¡Listo! Ahora puedes navegar, ver el listado paginado de productos y crear nuevos.

## Pruebas
Si tienes .NET SDK instalado localmente, puedes correr los tests (asegúrate de tener Docker corriendo para los tests de integración):

```bash
dotnet test
```

## Endpoints

*   `POST /api/Auth/login`: Obtener token JWT. (User: admin, Pass: password)
*   `POST /api/Category`: Crear categoría.
*   `GET /api/Product`: Listar productos (paginado, filtros).
*   `POST /api/Product`: Crear producto individual.
*   `POST /api/Product/seed`: Carga masiva de prueba (100k productos).
