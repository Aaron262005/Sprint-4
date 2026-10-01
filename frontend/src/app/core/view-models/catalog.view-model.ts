import { DestroyRef, Injectable, computed, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { APP_CONSTANTS } from '../constants/app.constants';
import { CatalogStatus } from '../models/catalog-status.enum';
import { Product } from '../models/product.model';
import { IProductApiService } from '../services/product-api.service';
import { ConnectivityService } from '../services/connectivity.service';

/**
 * ViewModel del catálogo general (US03, patrón MVVM).
 * Es el intermediario entre la Vista (CatalogComponent) y el Modelo (IProductApiService).
 * La Vista SOLO lee los signals de solo lectura expuestos aquí y llama a sus métodos.
 *
 * Se provee a nivel de componente (providers: [CatalogViewModel]) y no en 'root':
 * cada vez que se entra a la pantalla, el estado empieza limpio.
 */
@Injectable()
export class CatalogViewModel {
  private readonly productApi = inject(IProductApiService);
  private readonly connectivity = inject(ConnectivityService);
  private readonly destroyRef = inject(DestroyRef);

  private readonly _products = signal<Product[]>([]);
  private readonly _status = signal<CatalogStatus>(CatalogStatus.Loading);
  private readonly _errorMessage = signal<string | null>(null);

  readonly products = this._products.asReadonly();
  readonly status = this._status.asReadonly();
  readonly errorMessage = this._errorMessage.asReadonly();
  readonly isEmpty = computed(
    () => this._status() === CatalogStatus.Loaded && this._products().length === 0
  );

  loadProducts(): void {
    this._errorMessage.set(null);

    // US03 - Escenario 3: sin conexión, ni siquiera se intenta llamar a la API.
    if (!this.connectivity.isOnline()) {
      this.setError(APP_CONSTANTS.ERROR_MESSAGES.NO_CONNECTION);
      return;
    }

    // US03 - Escenario 2: indicador de carga mientras la petición está en proceso.
    this._status.set(CatalogStatus.Loading);

    this.productApi
      .getAll()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (products) => {
          // US03 - Escenario 1: catálogo descargado correctamente.
          this._products.set(products);
          this._status.set(CatalogStatus.Loaded);
        },
        // US03 - Escenario 3: error de red o caída del servidor.
        error: () => this.setError(APP_CONSTANTS.ERROR_MESSAGES.CATALOG_LOAD_FAILED),
      });
  }

  /** US03 - Escenario 3: botón "Reintentar". */
  retry(): void {
    this.loadProducts();
  }

  private setError(message: string): void {
    this._products.set([]);
    this._errorMessage.set(message);
    this._status.set(CatalogStatus.Error);
  }
}
