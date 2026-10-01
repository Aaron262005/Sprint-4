import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, defer, of, tap, throwError } from 'rxjs';
import { APP_CONSTANTS } from '../constants/app.constants';
import { Product, ProductData } from '../models/product.model';
import { IProductReader, IProductWriter } from './product-api.interface';
import { ProductAccessService } from './product-access.service';
import { ISessionStorageService } from './storage.service';

@Injectable()
export class ProductHttpService implements IProductReader, IProductWriter {
  private readonly http = inject(HttpClient);
  private readonly access = inject(ProductAccessService);
  private readonly storage = inject(ISessionStorageService);
  private readonly config = APP_CONSTANTS.PRODUCTS;
  private session: string | null = null;
  private readonly edited = new Map<number, Product>();

  get(id: number): Observable<Product> {
    return defer(() => {
      this.syncSession();
      const cached = this.edited.get(id);
      return cached ? of(cached) : this.http.get<Product>(this.url(id));
    });
  }

  create(product: ProductData): Observable<Product> {
    return this.write(() => this.http.post<Product>(this.url(), product));
  }

  update(id: number, product: ProductData): Observable<Product> {
    return this.write(() => this.http.put<Product>(this.url(id), product)).pipe(
      tap(updated => this.edited.set(id, updated))
    );
  }

  delete(id: number): Observable<Product> {
    return this.write(() => this.http.delete<Product>(this.url(id))).pipe(
      tap(() => this.edited.delete(id))
    );
  }

  private write(send: () => Observable<Product>): Observable<Product> {
    return defer(() => {
      this.syncSession();
      if (!this.access.canWrite()) {
        return throwError(() => new Error(this.config.TEXT.FORBIDDEN));
      }
      return send();
    });
  }

  private syncSession(): void {
    const token = this.storage.get(APP_CONSTANTS.STORAGE_KEYS.TOKEN);
    if (token !== this.session) {
      this.edited.clear();
      this.session = token;
    }
  }

  private url(id?: number): string {
    const url = new URL(this.config.API_URL);
    if (url.protocol !== this.config.HTTPS_PROTOCOL) {
      throw new Error(this.config.TEXT.HTTPS_ERROR);
    }
    return id === undefined ? url.href : `${url.href}/${id}`;
  }
}
