import { ProductToastComponent } from './features/products/components/product-toast/product-toast.component';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

/**
 * Componente raíz. Contiene el <router-outlet> y el aviso que debe sobrevivir a la navegación.
 * (Angular 20+ nombra este archivo app.ts y la clase App.)
 */
@Component({
  selector: 'app-root',
  imports: [ProductToastComponent, RouterOutlet],
  templateUrl: './app.html',
})
export class App {}
