import { Component, OnInit, inject } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { APP_CONSTANTS } from '../../core/constants/app.constants';
import { CatalogStatus } from '../../core/models/catalog-status.enum';
import { CatalogViewModel } from '../../core/view-models/catalog.view-model';

/**
 * Vista del catálogo general (US03). Es "tonta" a propósito: no llama a HttpClient
 * ni a localStorage; solo pide los datos a su ViewModel y renderiza su estado.
 */
@Component({
  selector: 'app-catalog',
  standalone: true,
  imports: [CurrencyPipe],
  providers: [CatalogViewModel],
  templateUrl: './catalog.component.html',
  styleUrl: './catalog.component.scss',
})
export class CatalogComponent implements OnInit {
  readonly viewModel = inject(CatalogViewModel);
  readonly texts = APP_CONSTANTS.CATALOG;
  readonly catalogStatus = CatalogStatus;

  ngOnInit(): void {
    this.viewModel.loadProducts();
  }

  onRetry(): void {
    this.viewModel.retry();
  }
}
