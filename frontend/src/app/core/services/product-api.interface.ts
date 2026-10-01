import { InjectionToken } from '@angular/core';
import { Observable } from 'rxjs';
import { Product, ProductData } from '../models/product.model';

export interface IProductReader {
  get(id: number): Observable<Product>;
}
export interface IProductWriter {
  create(product: ProductData): Observable<Product>;
  update(id: number, product: ProductData): Observable<Product>;
  delete(id: number): Observable<Product>;
}

// Las interfaces desaparecen al compilar: los tokens permiten pedirlas a Angular.
export const PRODUCT_READER = new InjectionToken<IProductReader>('IProductReader');
export const PRODUCT_WRITER = new InjectionToken<IProductWriter>('IProductWriter');
