import { Component, inject } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { ProductDetailViewModel } from '../../../../core/view-models/product-detail.view-model';

@Component({
  selector: 'app-product-detail', standalone: true,
  imports: [DecimalPipe], providers: [ProductDetailViewModel],
  templateUrl: './product-detail.component.html', styleUrl: '../../products.scss',
})
export class ProductDetailComponent {
  readonly vm = inject(ProductDetailViewModel);
}
