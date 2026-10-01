# Épica 3 — Inventario: US06, US07 y US08

## Solución y alcance

Se implementan crear, editar y eliminar productos sobre SP4, commit base `8b00155`. Backend ASP.NET Core net8.0 con CQRS/MediatR; frontend Angular standalone con MVVM. El usuario confirmó que la aplicación se utiliza en el navegador del celular. La confirmación usa `window.confirm` del navegador y la creación usa `window.alert`; no son componentes de una aplicación Android/iOS instalada.

Cambios por capa:
- Domain: entidad Product.
- Application: contratos separados de lectura y escritura, DTOs, validación, constantes y un Command/Query con Handler por operación.
- Infrastructure: repositorio propio con almacenamiento local JSON.
- API: controller delgado, protección por rol, manejo de errores de productos e inyección de dependencias.
- Angular: modelos, contratos con InjectionToken, servicio HTTP, ViewModels, vistas, guard de administrador y avisos persistentes durante la navegación.



## Árbol de archivos creados o modificados

```text
Sprint-4/
├── .gitignore
├── backend
│   ├── src
│   │   ├── Sprint4.Backend.API
│   │   │   ├── Controllers
│   │   │   │   └── ProductsController.cs
│   │   │   ├── Middleware
│   │   │   │   └── ProductsMiddleware.cs
│   │   │   ├── Program.cs
│   │   │   └── appsettings.json
│   │   ├── Sprint4.Backend.Application
│   │   │   ├── Common
│   │   │   │   ├── Constants
│   │   │   │   │   └── AppConstants.cs
│   │   │   │   └── Interfaces
│   │   │   │       ├── IProductReader.cs
│   │   │   │       └── IProductWriter.cs
│   │   │   └── Features
│   │   │       └── Products
│   │   │           ├── Commands
│   │   │           │   ├── CreateProduct
│   │   │           │   │   ├── CreateProductCommand.cs
│   │   │           │   │   └── CreateProductCommandHandler.cs
│   │   │           │   ├── DeleteProduct
│   │   │           │   │   ├── DeleteProductCommand.cs
│   │   │           │   │   └── DeleteProductCommandHandler.cs
│   │   │           │   └── UpdateProduct
│   │   │           │       ├── UpdateProductCommand.cs
│   │   │           │       └── UpdateProductCommandHandler.cs
│   │   │           ├── DTOs
│   │   │           │   ├── CreateProductDto.cs
│   │   │           │   └── UpdateProductDto.cs
│   │   │           ├── ProductNotFoundException.cs
│   │   │           ├── Queries
│   │   │           │   ├── GetProduct
│   │   │           │   │   ├── GetProductQuery.cs
│   │   │           │   │   └── GetProductQueryHandler.cs
│   │   │           │   └── ListProducts
│   │   │           │       ├── ListProductsQuery.cs
│   │   │           │       └── ListProductsQueryHandler.cs
│   │   │           └── Validation
│   │   │               └── HttpsImageAttribute.cs
│   │   ├── Sprint4.Backend.Domain
│   │   │   └── Entities
│   │   │       └── Product.cs
│   │   └── Sprint4.Backend.Infrastructure
│   │       ├── Persistence
│   │       │   └── JsonProductRepository.cs
│   │       └── Services
│   │           └── JwtTokenService.cs
│   └── tests
│       └── Sprint4.Backend.Products.Tests
│           ├── OwnBackendTests.cs
│           ├── ProductsTests.cs
│           └── Sprint4.Backend.Products.Tests.csproj
└── frontend
    └── src
        └── app
            ├── app.config.ts
            ├── app.html
            ├── app.routes.ts
            ├── app.spec.ts
            ├── app.ts
            ├── core
            │   ├── constants
            │   │   └── app.constants.ts
            │   ├── guards
            │   │   └── admin-role.guard.ts
            │   ├── models
            │   │   └── product.model.ts
            │   ├── services
            │   │   ├── browser-product-feedback.service.ts
            │   │   ├── product-access.service.ts
            │   │   ├── product-api.interface.ts
            │   │   ├── product-feedback.interface.ts
            │   │   ├── product-http.service.spec.ts
            │   │   └── product-http.service.ts
            │   └── view-models
            │       ├── product-detail.view-model.ts
            │       ├── product-form.view-model.ts
            │       └── products.spec.ts
            └── features
                └── products
                    ├── components
                    │   ├── product-detail
                    │   │   ├── product-detail.component.html
                    │   │   └── product-detail.component.ts
                    │   ├── product-form
                    │   │   ├── product-form.component.html
                    │   │   └── product-form.component.ts
                    │   └── product-toast
                    │       └── product-toast.component.ts
                    └── products.scss
```

## Sustituir la entrega anterior

Esta entrega reemplaza la versión que utilizaba un servicio externo. Si copiaste los archivos anteriores, elimina `backend/src/Sprint4.Backend.Infrastructure/Services/FakeStoreApiService.cs` antes de copiar los nuevos. En la rama de trabajo ese archivo ya fue eliminado. No requiere dependencias de producción nuevas.

## Límites de integración que debes conocer

SP4 no tiene el catálogo de US03/US04/US05. Por eso `APP_CONSTANTS.PRODUCTS.CATALOG` apunta provisionalmente a `/home`. No se implementó el catálogo de otro integrante ni se añadieron enlaces a su pantalla. Cuando esté disponible, cambiar esa constante y enlazar las rutas nuevas desde el catálogo. La navegación al catálogo real todavía depende de ese trabajo.

Se agregó GET `/products/{id}` como apoyo de lectura del formulario y detalle. Cuando el equipo integre su propio detalle, puede reutilizar los ViewModels y el servicio, evitando duplicar rutas.

El backend es dueño de los productos. No consume Fake Store ni otra API de productos. POST guarda, PUT actualiza y DELETE elimina realmente del archivo del servidor. Angular vuelve a consultar el backend al entrar al detalle; no mantiene una copia de simulación.

**Almacenamiento provisional elegido:** JSON local porque el repositorio no tiene una base de datos configurada. Se guarda por defecto en `backend/src/Sprint4.Backend.API/App_Data/products.json`, excluido de Git. Se puede cambiar la ruta con `Products__StoragePath`. La carpeta debe ser escribible y conservarse al volver a desplegar. En contenedores se necesita un volumen persistente.

Esta implementación soporta una sola instancia de la API. Un semáforo serializa las escrituras y el reemplazo del archivo se realiza después de escribir el contenido completo. Guarda el siguiente ID para no reutilizar IDs eliminados después de reiniciar. No es una solución para varias réplicas ni grandes volúmenes; en ese caso debe reemplazarse por un repositorio SQL mediante las mismas interfaces.

El inventario comienza vacío: primero crea un producto y usa el ID devuelto. No se añadieron datos de otras historias. El catálogo visual sigue pendiente del integrante responsable; se entrega GET /products para que pueda consumir los productos reales.

| Método | Ruta | Acceso | Resultado |
|---|---|---|---|
| GET | /products | Sesión válida | Lista real, inicialmente vacía |
| GET | /products/{id} | Sesión válida | Producto o 404 |
| POST | /products | Administrador | 201 con producto e ID generado |
| PUT | /products/{id} | Administrador | 200 con producto actualizado, o 404 |
| DELETE | /products/{id} | Administrador | 200 con producto eliminado, o 404 |

## Cómo explicarlo en clase

Piensa en una tienda:
1. **La Vista es el mostrador.** Muestra los campos y avisa cuando presionas Guardar.
2. **El ViewModel es quien organiza el pedido.** Revisa los datos, activa la espera y decide qué mensaje mostrar.
3. **La interfaz es un acuerdo.** Dice qué acciones debe ofrecer un servicio, sin decir cómo las hace.
4. **El servicio HTTP es el mensajero.** Lleva el producto por HTTPS al backend.
5. **El controller es la recepción.** Recibe el pedido y se lo entrega a MediatR.
6. **El Handler atiende ese pedido.** Valida lo necesario y pide la operación mediante una interfaz.
7. **JsonProductRepository guarda en el archivo del servidor.** Es el único que conoce ese almacenamiento.

Frase para la exposición: “Cada parte tiene un trabajo pequeño. Así puedo cambiar el mensajero sin reconstruir el mostrador”.

## Interfaces e inyección de dependencias

Backend: `IProductReader` ofrece ListAsync y GetAsync; `IProductWriter` ofrece CreateAsync, UpdateAsync y DeleteAsync. JsonProductRepository implementa ambos contratos. Program.cs registra una única instancia (Singleton) y conecta las dos interfaces con ella para compartir el bloqueo de escrituras. Los Handlers reciben contratos en su constructor; nunca crean servicios concretos.

Frontend: `IProductReader`, `IProductWriter` e `IProductFeedback` son interfaces TypeScript reales. Como no existen después de compilar, `PRODUCT_READER`, `PRODUCT_WRITER` y `PRODUCT_FEEDBACK` son las etiquetas que Angular utiliza para encontrarlas. app.config.ts es el único lugar que conecta esas etiquetas con ProductHttpService y BrowserProductFeedbackService. `useExisting` comparte una sola instancia del servicio HTTP. Cada GET consulta al servidor. Los providers de los componentes solo crean su ViewModel, no vinculan interfaces con implementaciones.

Se reutiliza ISessionStorageService del proyecto, que ya era una clase abstracta; no se cambia ese contrato ajeno ni se crean nuevas clases abstractas sin lógica compartida.

## SOLID aplicado

| Principio | Aplicación concreta |
|---|---|
| Responsabilidad única | La Vista dibuja; el ViewModel organiza estado; el servicio hace HTTP; el Handler atiende un caso de uso. |
| Abierto/cerrado | Se puede agregar un repositorio SQL que implemente las interfaces y cambiar el registro de DI, sin rehacer los Handlers. |
| Sustitución | Las pruebas de permisos sustituyen el almacén por otro que cumple los contratos. Además, las pruebas del ciclo CRUD usan el repositorio JSON real. |
| Segregación de interfaces | Lectura y escritura son contratos diferentes; los diálogos tampoco se mezclan con acceso a productos. |
| Inversión de dependencias | ViewModels y Handlers piden interfaces, no servicios concretos. app.config.ts y Program.cs eligen las implementaciones. |

Se usa `if` para condiciones independientes: formulario inválido, rol incorrecto, acción en curso, ID inválido o cancelación. No hay una selección de muchos casos sobre un mismo valor que justifique introducir `switch`. Se usa una expresión condicional para elegir crear o actualizar porque solo hay dos opciones.

## Seguridad y HTTPS

- Las escrituras del controller tienen Authorize con rol Administrador; sin token responden 401, y Cliente/Auditor reciben 403.
- Los guards mejoran la navegación, pero el backend toma la decisión de seguridad real.
- El servicio Angular vuelve a comprobar permisos antes de escribir y exige una URL HTTPS.
- El middleware rechaza HTTP en /products antes de ejecutar una escritura. El backend guarda los productos localmente; no realiza peticiones a una API externa.
- Se exigen imágenes HTTPS, precio entre 0.01 y 999999999 y textos no vacíos. Este rango es un supuesto explícito porque las historias no lo fijan.
- Se quitó la clave JWT fija de Program.cs, JwtTokenService y appsettings.json. Es el único ajuste en autenticación y es necesario para no permitir firmas con una clave pública conocida. La API exige una clave externa de al menos 32 bytes; login y los roles mantienen sus contratos.
- El almacén de usuarios y credenciales de demostración ya existente pertenece a US01 y no fue modificado. No es autenticación apta para producción.

## Instalar, ejecutar y comprobar

No se añadieron dependencias de producción. Se aprovechan MediatR, HttpClient, Reactive Forms y RxJS ya incluidos. Las dependencias nuevas son únicamente las pruebas de backend, declaradas en su csproj: Microsoft.NET.Test.Sdk 17.11.1, Microsoft.AspNetCore.Mvc.Testing 8.0.8, xunit 2.9.2 y xunit.runner.visualstudio 2.8.2. `dotnet restore` las instala.

Desde la raíz del repositorio:

```sh
dotnet restore backend/src/Sprint4.Backend.API/Sprint4.Backend.API.csproj
dotnet build backend/src/Sprint4.Backend.API/Sprint4.Backend.API.csproj
dotnet test backend/tests/Sprint4.Backend.Products.Tests/Sprint4.Backend.Products.Tests.csproj
```

Se usan rutas .csproj para funcionar también con SDK 8: el archivo .slnx que ya traía el repositorio requiere un SDK más reciente. El código sigue dirigido a net8.0.

Para ejecutar, configura primero una clave aleatoria solo para esa terminal. En macOS/Linux:

```sh
export JwtSettings__Key="$(openssl rand -base64 48)"
dotnet dev-certs https --trust
dotnet run --project backend/src/Sprint4.Backend.API
```

En PowerShell, genera la clave sin escribirla en el repositorio:

```powershell
$bytes = New-Object byte[] 48
$rng = [System.Security.Cryptography.RandomNumberGenerator]::Create()
$rng.GetBytes($bytes)
$env:JwtSettings__Key = [Convert]::ToBase64String($bytes)
$rng.Dispose()
dotnet dev-certs https --trust
dotnet run --project backend/src/Sprint4.Backend.API
```

La variable debe existir cada vez que abras una terminal nueva. Cambiar la clave invalida los tokens anteriores; vuelve a iniciar sesión. No copies claves a código, capturas ni commits.

En otra terminal:

```sh
cd frontend
npm ci
npx ng build
npm test -- --watch=false
npm start
```

En este entorno la caché LMDB de Angular provocó un fallo de empaquetado. Se validó producción usando su alternativa SQLite, sin cambiar el proyecto:

```sh
NG_BUILD_CACHE_STORE=sqlite NG_BUILD_SASS_EMBEDDED=0 NG_BUILD_MAX_WORKERS=2 npm run build
```

En el entorno restringido se deshabilitó la recarga de configuración de .NET con `DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false` y se utilizó runtime 8.0.31 para las pruebas. Son ajustes del entorno de comprobación, no requisitos nuevos de la aplicación.

Rutas después del login:
- `/products/new`: crear (Administrador).
- `/products/1`: detalle del producto con ID 1, si ya fue creado (usuario autenticado).
- `/products/1/edit`: editar ese producto (Administrador).

## Navegador de un celular físico

`localhost` en un celular apunta al celular, no a tu computadora. Publica la API con un dominio HTTPS y certificado confiable para ese teléfono. Ajusta BASE_URL y PRODUCTS.API_URL en app.constants.ts al mismo servidor, conservando /api para autenticación y /products para inventario. En backend configura `Frontend__AllowedOrigins__0` con el origen exacto del frontend del teléfono. El valor predeterminado conserva http://localhost:4200 para desarrollo. No desactives la validación de certificados.

## Pruebas manuales

1. Administrador: crear con todos los campos válidos; comprobar alerta con ID y formulario limpio.
2. Dejar un campo vacío, usar solo espacios, escribir letras en precio o una URL inválida; comprobar errores rojos y ausencia de POST/PUT en la pestaña Network.
3. Crear primero un producto; abrir /products/{id}/edit con el ID recibido; comprobar los cinco campos precargados.
4. Cambiar título y guardar; comprobar el aviso exacto “Producto actualizado” y el nuevo título en detalle.
5. Usar conexión lenta; comprobar spinner, controles deshabilitados y una sola solicitud al tocar dos veces.
6. Eliminar y cancelar; comprobar que no hubo DELETE ni navegación.
7. Eliminar y aceptar; comprobar un DELETE, aviso de éxito y regreso al destino configurado. Consultar ese ID de nuevo debe devolver 404 y GET /products ya no debe incluirlo.
8. Cliente y Auditor: abrir directamente /products/new y /products/1/edit; comprobar redirección. En detalle no deben aparecer Editar/Eliminar. Forzar POST/PUT/DELETE con sus tokens debe dar 403.
9. Sin token: forzar una escritura debe dar 401. Una llamada HTTP a /products debe rechazarse.
10. Simular error de red; comprobar mensaje comprensible, botón habilitado otra vez y datos del formulario conservados.
11. Crear y editar un producto, reiniciar el backend y recargar el navegador: comprobar que conserva los datos.
12. Probar a 360 px de ancho y con teclado; comprobar lectura, foco visible, botones accesibles y ausencia de desbordamiento.

## Validación realizada

- Backend: compilación sin errores ni advertencias.
- Pruebas backend: 20 casos: permisos con JWT, ciclo HTTP completo con JSON real, persistencia al recrear el repositorio y altas concurrentes.
- Angular: compilación de producción y 14 pruebas automatizadas.
- No se verificó en un teléfono físico. El ciclo de endpoints propios se prueba con el servidor ASP.NET en memoria y archivos temporales reales.
- No se integra ni se publica directamente sobre SP4: la implementación está en una rama de trabajo independiente.
