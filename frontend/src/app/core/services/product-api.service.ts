import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { APP_CONSTANTS } from '../constants/app.constants';
import { Product } from '../models/product.model';

/**
 * Contrato de acceso a productos (Interface Segregation + Dependency Inversion).
 * El CatalogViewModel depende de ESTE contrato, nunca de HttpClient directamente.
 * Si el equipo necesita un mock (shared/mocks) o cambia de API, se crea otra clase
 * que implemente este contrato y se registra en app.config.ts — el ViewModel no cambia.
 *
 * TODO (equipo catálogo): US04 y US05 pueden agregar aquí getCategories(),
 * getByCategory(category) y getById(id).
 */
export abstract class IProductApiService {
  abstract getAll(): Observable<Product[]>;
}

/** Implementación concreta: GET /api/products del backend (US03). */
@Injectable({ providedIn: 'root' })
export class HttpProductApiService implements IProductApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = APP_CONSTANTS.API.BASE_URL;

  getAll(): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.baseUrl}${APP_CONSTANTS.API.PRODUCTS.ALL}`);
  }
}
