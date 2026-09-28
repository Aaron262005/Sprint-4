import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

/**
 * Componente raíz. Solo contiene el <router-outlet>: cada pantalla real vive en features/.
 * (Angular 20+ nombra este archivo app.ts y la clase App.)
 */
@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
})
export class App {}
