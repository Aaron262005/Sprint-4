import { Injectable, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { AuthApiService } from '../services/auth-api.service';
import { ISessionStorageService } from '../services/storage.service';
import { ConnectivityService } from '../services/connectivity.service';
import { APP_CONSTANTS } from '../constants/app.constants';
import { LoginResponse } from '../models/auth.model';

/**
 * ViewModel de autenticación (patrón MVVM).
 * Es el intermediario entre la Vista (LoginComponent/HomeComponent) y el Modelo
 * (AuthApiService). La Vista SOLO lee los signals expuestos aquí y llama a estos
 * métodos; nunca llama a HttpClient ni a localStorage directamente.
 *
 * Cubre US01 (login, 3 escenarios) y US02 (logout, 3 escenarios).
 */
@Injectable({ providedIn: 'root' })
export class AuthViewModel {
  private readonly authApi = inject(AuthApiService);
  private readonly storage = inject(ISessionStorageService);
  private readonly connectivity = inject(ConnectivityService);
  private readonly router = inject(Router);

  readonly isLoading = signal(false);
  readonly errorMessage = signal<string | null>(null);

  login(username: string, password: string): void {
    this.errorMessage.set(null);

    // US01 - Escenario 3: sin conexión, ni siquiera se intenta llamar a la API.
    if (!this.connectivity.isOnline()) {
      this.errorMessage.set(APP_CONSTANTS.ERROR_MESSAGES.NO_CONNECTION);
      return;
    }

    this.isLoading.set(true);
    this.authApi.login({ username, password }).subscribe({
      next: (response) => this.handleLoginSuccess(response),
      error: () => {
        // US01 - Escenario 2: credenciales incorrectas (401 del backend).
        this.isLoading.set(false);
        this.errorMessage.set(APP_CONSTANTS.ERROR_MESSAGES.INVALID_CREDENTIALS);
      },
    });
  }

  private handleLoginSuccess(response: LoginResponse): void {
    this.isLoading.set(false);

    if (!response.success || !response.token) {
      this.errorMessage.set(response.errorMessage ?? APP_CONSTANTS.ERROR_MESSAGES.INVALID_CREDENTIALS);
      return;
    }

    // US01 - Escenario 1: guarda sesión y navega a la pantalla principal.
    this.storage.set(APP_CONSTANTS.STORAGE_KEYS.TOKEN, response.token);
    this.storage.set(
      APP_CONSTANTS.STORAGE_KEYS.USER,
      JSON.stringify({ id: response.userId, username: response.username })
    );
    this.storage.set(APP_CONSTANTS.STORAGE_KEYS.ROLE, response.role ?? '');

    this.router.navigateByUrl(APP_CONSTANTS.ROUTES.HOME);
  }

  logout(userId: number): void {
    this.authApi.logout(userId).subscribe({
      complete: () => this.clearSessionAndRedirect(),
      // Aunque falle la llamada al backend, la limpieza local debe ocurrir igual (US02).
      error: () => this.clearSessionAndRedirect(),
    });
  }

  private clearSessionAndRedirect(): void {
    // US02 - Escenario 3: limpieza profunda de memoria/almacenamiento (token, rol, carrito).
    this.storage.clear([
      APP_CONSTANTS.STORAGE_KEYS.TOKEN,
      APP_CONSTANTS.STORAGE_KEYS.USER,
      APP_CONSTANTS.STORAGE_KEYS.ROLE,
    ]);

    // US02 - Escenario 2: replaceUrl para no dejar el catálogo en el historial de navegación.
    this.router.navigateByUrl(APP_CONSTANTS.ROUTES.LOGIN, { replaceUrl: true });
  }
}
