import { Injectable } from '@angular/core';

/**
 * Contrato de almacenamiento de sesión (Interface Segregation + Dependency Inversion).
 * El AuthViewModel depende de ESTE contrato, nunca de "localStorage" directamente.
 * Si el equipo migra a un almacenamiento nativo seguro (ej. Capacitor Secure Storage
 * en una app móvil híbrida), solo se crea una nueva clase que implemente este mismo
 * contrato y se registra en app.config.ts — el ViewModel no cambia.
 */
export abstract class ISessionStorageService {
  abstract set(key: string, value: string): void;
  abstract get(key: string): string | null;
  abstract remove(key: string): void;
  abstract clear(keys: string[]): void;
}

/** Implementación concreta usando localStorage del navegador. */
@Injectable({ providedIn: 'root' })
export class BrowserSessionStorageService implements ISessionStorageService {
  set(key: string, value: string): void {
    localStorage.setItem(key, value);
  }

  get(key: string): string | null {
    return localStorage.getItem(key);
  }

  remove(key: string): void {
    localStorage.removeItem(key);
  }

  clear(keys: string[]): void {
    keys.forEach((key) => localStorage.removeItem(key));
  }
}
