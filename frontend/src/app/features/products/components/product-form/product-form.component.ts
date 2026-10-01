import { Component, inject } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ProductFormViewModel } from '../../../../core/view-models/product-form.view-model';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  providers: [ProductFormViewModel],
  templateUrl: './product-form.component.html',
  styleUrl: '../../products.scss',
})
export class ProductFormComponent {
  readonly vm = inject(ProductFormViewModel);
}
