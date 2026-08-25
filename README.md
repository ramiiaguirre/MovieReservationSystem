# MovieReservationSystem

## Auth

Use Microsoft.AspNetCore.Authentication.JwtBearer;

La API usa **JWT (HS256)** con clave simétrica leída de la sección `Jwt` (`JwtSettings` + `IOptions`).

- **Registro/Login** (`AuthController`, `[AllowAnonymous]`): `AuthService` valida credenciales y las contraseñas se guardan hasheadas con BCrypt (`IPasswordHasher`).
- **Emisión**: `JwtManager.GenerateJWT` arma el token con los claims `NameIdentifier`, `Name` y un `Role` por cada rol del usuario, con expiración de 30 minutos.
- **Transporte**: el token no se devuelve en el body; se escribe en una cookie `jwt` con `HttpOnly`, `Secure` e igual expiración, para evitar accesos desde JavaScript.
- **Lectura**: el handler `JwtBearer` toma el token desde esa cookie mediante el evento `OnMessageReceived`, en lugar del header `Authorization`.
- **Validación**: se verifica firma y expiración (`ClockSkew = 0`); issuer y audience quedan deshabilitados. El endpoint `validarToken` expone esa comprobación con `JwtManager.ValidarToken`.
- **Autorización**: los claims de rol habilitan el uso de `[Authorize(Roles = "...")]` en los controllers.

System that allows users to reserve movie tickets.

- Remember: UserSecrets

## Validation (FluentValidation)

## EntityFramework

Code First + Migrations

Use EagerLoading for relational data (Includes).

Save in `MovieReservation.Services/Migrations`

## API Rest

The project follow the Richardson Maturity Model guidelines.
