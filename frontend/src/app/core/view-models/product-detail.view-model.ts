import { DestroyRef, Injectable, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Router } from '@angular/router';
import { EMPTY, catchError, finalize, firstValueFrom, switchMap } from 'rxjs';
import { APP_CONSTANTS } from '../constants/app.constants';
import { Product } from '../models/product.model';
import { PRODUCT_READER, PRODUCT_WRITER } from '../services/product-api.interface';
import { PRODUCT_FEEDBACK } from '../services/product-feedback.interface';
import { ProductAccessService } from '../services/product-access.service';

@Injectable()
export class ProductDetailViewModel {
  private readonly reader = inject(PRODUCT_READER);
  private readonly writer = inject(PRODUCT_WRITER);
  private readonly feedback = inject(PRODUCT_FEEDBACK);
  private readonly access = inject(ProductAccessService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly destroyRef = inject(DestroyRef);
  readonly config = APP_CONSTANTS.PRODUCTS;
  readonly text = this.config.TEXT;
  readonly product = signal<Product | null>(null);
  readonly busy = signal(false);
  readonly error = signal('');
  canWrite(): boolean { return this.access.canWrite(); }

  constructor() {
    this.route.paramMap.pipe(
      switchMap(parameters => {
        this.product.set(null);
        this.error.set('');
        const id = Number(parameters.get(this.config.ID_PARAMETER));
        if (!Number.isSafeInteger(id) || id <= 0) {
          this.error.set(this.text.INVALID_ID); return EMPTY;
        }
        this.busy.set(true);
        return this.reader.get(id).pipe(
          catchError(() => { this.error.set(this.text.ERROR); return EMPTY; }),
          finalize(() => this.busy.set(false))
        );
      }),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe(product => {
      this.product.set(product);
      if (!product) this.error.set(this.text.NOT_FOUND);
    });
  }

  edit(): void {
    const product = this.product();
    if (!product || this.busy() || !this.canWrite()) return;
    void this.router.navigate([this.config.ROOT, product.id, this.config.EDIT_SEGMENT]);
  }

  async remove(): Promise<void> {
    const product = this.product();
    if (!product || this.busy() || !this.canWrite()) return;
    this.busy.set(true);
    this.error.set('');
    try {
      if (!await this.feedback.confirmDelete()) return;
      if (this.destroyRef.destroyed || !this.canWrite()) return;
      await firstValueFrom(this.writer.delete(product.id).pipe(takeUntilDestroyed(this.destroyRef)));
      this.feedback.toast(this.text.DELETED);
      await this.router.navigateByUrl(this.config.CATALOG, { replaceUrl: true });
    } catch {
      if (!this.destroyRef.destroyed) this.error.set(this.text.ERROR);
    } finally { this.busy.set(false); }
  }

  back(): void {
    if (!this.busy()) void this.router.navigateByUrl(this.config.CATALOG);
  }
}
