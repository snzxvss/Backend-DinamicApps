# Backend Project - WebApi-DinamicApps (.NET 8)

Este proyecto es una API RESTful desarrollada en **.NET 8** para la gestión de citas médicas, autenticación de pacientes y administración de médicos y citas. Utiliza JWT para autenticación y Entity Framework Core para acceso a datos.

## Estructura del Proyecto

El backend está organizado de la siguiente manera:

```
Backend-DinamicApps/
├── Backend-DinamicApps/
│   ├── Controllers/
│   │   ├── CitaController.cs
│   │   └── PacienteController.cs
│   ├── Helpers/
│   │   └── JwtHelper.cs
│   ├── Program.cs
│   ├── appsettings.json
│   └── WebApi-DinamicApps.csproj
├── Models/
│   ├── DbContexts/
│   │   └── IpsDbContext.cs
│   ├── Model/
│   │   ├── Paciente.cs
│   │   ├── Medico.cs
│   │   └── Cita.cs
│   ├── Request/
│   │   ├── AutenticarPacienteRequest.cs
│   │   └── ReservarCitaRequest.cs
│   └── Response/
│       └── ResponseModel.cs
├── Services/
│   ├── Interfaces/
│   │   ├── ICitaService.cs
│   │   └── IPacienteService.cs
│   └── Services/
│       ├── CitaService.cs
│       └── PacienteService.cs
├── DB.sql
├── Models-DinamicApps.csproj
├── Services-DinamicApps.csproj
└── README.md
```

### Principales Archivos y Carpetas

- **`Controllers/`**: Controladores de la API (endpoints REST).
- **`Helpers/`**: Utilidades como generación de JWT.
- **`Models/`**: Modelos de dominio, requests, responses y DbContext.
- **`Services/`**: Lógica de negocio y acceso a datos, junto con interfaces.
- **`appsettings.json`**: Configuración de la aplicación (conexión a base de datos, JWT, etc).
- **`DB.sql`**: Script para crear y poblar la base de datos SQL Server.

## Lógica y Funcionamiento

- **Autenticación JWT**:  
  El endpoint `/api/paciente/autenticar` genera un token JWT al autenticar un paciente. Los endpoints de citas requieren este token.
- **Gestión de Citas**:  
  Endpoints para consultar y reservar citas, protegidos por JWT.
- **Entity Framework Core**:  
  Acceso a la base de datos SQL Server mediante el DbContext `IpsDbContext`.

## Configuración y Despliegue

### Requisitos

- .NET 8 SDK
- SQL Server (o la base de datos configurada en `appsettings.json`)

### Configuración

1. Clona el repositorio:

```sh
git clone https://github.com/snzxvss/Backend-DinamicApps.git
```

2. Configura la cadena de conexión y la clave JWT en `Backend-DinamicApps/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=...;Database=...;User Id=...;Password=...;"
},
"Jwt": {
  "Secret": "TU_CLAVE_SECRETA"
}
```

3. Crea la base de datos ejecutando el script `DB.sql` en tu SQL Server.

4. Restaura los paquetes y compila la solución:

```sh
dotnet restore
dotnet build
```

5. Ejecuta la API:

```sh
dotnet run --project Backend-DinamicApps/WebApi-DinamicApps.csproj
```

6. La API estará disponible en `https://localhost:5005` (o el puerto configurado).

### Endpoints Principales

- `POST /api/paciente/autenticar`  
  Autenticación de paciente, retorna JWT y datos del paciente.
- `GET /api/cita/disponibles?especialidad=...`  
  Consulta citas disponibles (requiere JWT).
- `POST /api/cita/reservar`  
  Reserva una cita (requiere JWT).

### Seguridad

- JWT requerido en endpoints protegidos.  
  Envía el token en el header:  
  `Authorization: Bearer <token>`

### Despliegue en Producción

- Publica la API:
```sh
dotnet publish -c Release
```
- Configura tu servidor (IIS, Nginx, Apache) para exponer el puerto correspondiente.

---

Desarrollado por **Camilo Sanz**.
