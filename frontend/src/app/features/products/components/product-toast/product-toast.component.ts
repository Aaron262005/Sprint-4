import { Component, inject } from '@angular/core';
import { PRODUCT_FEEDBACK } from '../../../../core/services/product-feedback.interface';

@Component({
  selector: 'app-product-toast', standalone: true,
  template: `<div role="status" aria-live="polite">@if (feedback.message()) {
    <p class="toast">{{ feedback.message() }}</p>
  }</div>`,
  styles: `.toast { position: fixed; bottom: 1rem; left: 50%; transform: translateX(-50%);
    width: max-content; max-width: calc(100vw - 4rem); padding: 1rem; border-radius: .5rem;
    background: #163c2a; color: white; z-index: 1000; font-family: system-ui, sans-serif; }`,
})
export class ProductToastComponent {
  readonly feedback = inject(PRODUCT_FEEDBACK);
}
