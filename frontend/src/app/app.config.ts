import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { routes } from './app.routes';
import { authInterceptor } from './core/interceptors/auth.interceptor';
import { ISessionStorageService, BrowserSessionStorageService } from './core/services/storage.service';

/**
 * Composición de dependencias del frontend (equivalente a Program.cs en el backend).
 * Aquí es donde se "amarra" cada interfaz con su implementación concreta.
 * Si el equipo agrega una nueva abstracción (ej. ICartStorageService), se registra aquí.
 */
export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([authInterceptor])),
    { provide: ISessionStorageService, useClass: BrowserSessionStorageService },
  ],
};
