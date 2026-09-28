import { Routes } from '@angular/router';
import { APP_CONSTANTS } from './core/constants/app.constants';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: APP_CONSTANTS.ROUTES.LOGIN, pathMatch: 'full' },

  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login.component').then((m) => m.LoginComponent),
  },

  {
    path: 'home',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/home/home.component').then((m) => m.HomeComponent),
  },

  // TODO (equipo catálogo):   agregar aquí las rutas de US03/US04/US05, protegidas con [authGuard]
  // TODO (equipo carrito):    agregar aquí las rutas de US09/US10
  // TODO (equipo admin):      agregar aquí las rutas de US06/US07/US08
  // TODO (equipo usuarios):   agregar aquí las rutas de US11/US12

  { path: '**', redirectTo: APP_CONSTANTS.ROUTES.LOGIN },
];
