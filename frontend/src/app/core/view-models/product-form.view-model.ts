import { DestroyRef, Injectable, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Router } from '@angular/router';
import { AbstractControl, FormBuilder, ValidationErrors, Validators } from '@angular/forms';
import { EMPTY, catchError, finalize, switchMap } from 'rxjs';
import { APP_CONSTANTS } from '../constants/app.constants';
import { PRODUCT_READER, PRODUCT_WRITER } from '../services/product-api.interface';
import { PRODUCT_FEEDBACK } from '../services/product-feedback.interface';
import { ProductAccessService } from '../services/product-access.service';

const config = APP_CONSTANTS.PRODUCTS;
const notBlank = (control: AbstractControl): ValidationErrors | null =>
  typeof control.value === 'string' && control.value.trim() ? null : { required: true };
const httpsImage = (control: AbstractControl): ValidationErrors | null => {
  try {
    const url = new URL(control.value);
    return url.protocol === config.HTTPS_PROTOCOL && !!url.hostname ? null : { image: true };
  } catch { return { image: true }; }
};
const price = (control: AbstractControl): ValidationErrors | null =>
  typeof control.value === 'number' && Number.isFinite(control.value)
    && control.value >= config.MIN_PRICE && control.value <= config.MAX_PRICE
    ? null : { price: true };

@Injectable()
export class ProductFormViewModel {
  private readonly reader = inject(PRODUCT_READER);
  private readonly writer = inject(PRODUCT_WRITER);
  private readonly feedback = inject(PRODUCT_FEEDBACK);
  private readonly access = inject(ProductAccessService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly destroyRef = inject(DestroyRef);
  readonly text = config.TEXT;
  readonly busy = signal(false);
  readonly ready = signal(false);
  readonly error = signal('');
  readonly id = signal<number | null>(null);
  readonly form = inject(FormBuilder).nonNullable.group({
    title: ['', [notBlank]],
    price: [0, [Validators.required, price]],
    description: ['', [notBlank]],
    image: ['', [notBlank, httpsImage]],
    category: ['', [notBlank]],
  });

  constructor() {
    this.route.paramMap.pipe(
      switchMap(parameters => {
        this.error.set('');
        this.ready.set(false);
        this.form.reset();
        const raw = parameters.get(config.ID_PARAMETER);
        this.id.set(raw === null ? null : Number(raw));
        if (raw === null) { this.ready.set(true); return EMPTY; }
        const id = this.id()!;
        if (!Number.isSafeInteger(id) || id <= 0) {
          this.error.set(this.text.INVALID_ID);
          return EMPTY;
        }
        this.busy.set(true);
        return this.reader.get(id).pipe(
          catchError(() => { this.error.set(this.text.ERROR); return EMPTY; }),
          finalize(() => this.busy.set(false))
        );
      }),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe(product => {
      if (!product) { this.error.set(this.text.NOT_FOUND); return; }
      this.form.patchValue(product);
      this.ready.set(true);
    });
  }

  save(): void {
    if (this.busy() || !this.ready()) return;
    if (!this.access.canWrite()) { this.error.set(this.text.FORBIDDEN); return; }
    this.form.markAllAsTouched();
    if (this.form.invalid) return;

    const values = this.form.getRawValue();
    const product = { ...values, title: values.title.trim(), description: values.description.trim(),
      image: values.image.trim(), category: values.category.trim() };
    const id = this.id();
    const request = id === null ? this.writer.create(product) : this.writer.update(id, product);
    this.error.set('');
    this.busy.set(true);
    request.pipe(takeUntilDestroyed(this.destroyRef), finalize(() => this.busy.set(false))).subscribe({
      next: response => {
        if (id === null) {
          this.feedback.created(response.id);
          this.form.reset();
        } else {
          this.feedback.toast(this.text.UPDATED);
          void this.router.navigate([config.ROOT, id], { replaceUrl: true });
        }
      },
      error: () => this.error.set(this.text.ERROR),
    });
  }

  back(): void {
    if (this.busy()) return;
    const id = this.id();
    void this.router.navigate(id === null ? [config.CATALOG] : [config.ROOT, id]);
  }
}
