import { adminRoleGuard } from './core/guards/admin-role.guard';
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
  {
    path: APP_CONSTANTS.PRODUCTS.CREATE_PATH,
    canActivate: [authGuard, adminRoleGuard],
    loadComponent: () => import('./features/products/components/product-form/product-form.component')
      .then(m => m.ProductFormComponent),
  },
  {
    path: APP_CONSTANTS.PRODUCTS.EDIT_PATH,
    canActivate: [authGuard, adminRoleGuard],
    loadComponent: () => import('./features/products/components/product-form/product-form.component')
      .then(m => m.ProductFormComponent),
  },
  {
    path: APP_CONSTANTS.PRODUCTS.DETAIL_PATH,
    canActivate: [authGuard],
    loadComponent: () => import('./features/products/components/product-detail/product-detail.component')
      .then(m => m.ProductDetailComponent),
  },
  // TODO (equipo usuarios):   agregar aquí las rutas de US11/US12

  { path: '**', redirectTo: APP_CONSTANTS.ROUTES.LOGIN },
];
