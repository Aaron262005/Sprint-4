import { Injectable } from '@angular/core';

/**
 * Única responsabilidad: saber si el dispositivo tiene conexión.
 * Usado por AuthViewModel para US01 - Escenario 3 (no llamar a la API sin conexión).
 */
@Injectable({ providedIn: 'root' })
export class ConnectivityService {
  isOnline(): boolean {
    return navigator.onLine;
  }
}
