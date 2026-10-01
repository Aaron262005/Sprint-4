import { InjectionToken, Signal } from '@angular/core';

export interface IProductFeedback {
  readonly message: Signal<string>;
  confirmDelete(): Promise<boolean>;
  created(id: number): void;
  toast(message: string): void;
}
export const PRODUCT_FEEDBACK = new InjectionToken<IProductFeedback>('IProductFeedback');
