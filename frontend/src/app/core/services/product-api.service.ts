import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { APP_CONSTANTS } from '../constants/app.constants';
import { Product } from '../models/product.model';

/**
 * Contrato de lectura del catálogo (Interface Segregation + Dependency Inversion).
 * El CatalogViewModel depende de ESTE contrato, nunca de HttpClient directamente.
 * Si el equipo necesita un mock (shared/mocks) o cambia de API, se crea otra clase
 * que implemente este contrato y se registra en app.config.ts — el ViewModel no cambia.
 *
 * TODO (equipo catálogo): US04 puede agregar aquí getByCategory(category).
 */
export abstract class IProductApiService {
  abstract getAll(): Observable<Product[]>;
}

/**
 * Implementación concreta (US03): GET /products del backend propio.
 * Reutiliza el endpoint de listado de la Épica 3 (ListProductsQuery), así el catálogo
 * muestra exactamente los productos que los administradores crean desde la app.
 * La URL sale de APP_CONSTANTS.PRODUCTS.API_URL, la misma que usa la Épica 3.
 */
@Injectable({ providedIn: 'root' })
export class HttpProductApiService implements IProductApiService {
  private readonly http = inject(HttpClient);

  getAll(): Observable<Product[]> {
    return this.http.get<Product[]>(APP_CONSTANTS.PRODUCTS.API_URL);
  }
}
