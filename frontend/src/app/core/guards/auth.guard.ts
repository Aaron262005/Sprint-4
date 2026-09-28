import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { APP_CONSTANTS } from '../constants/app.constants';

/**
 * Bloquea el acceso a vistas protegidas (catálogo, carrito, etc.) si no hay token.
 * US02 - Escenario 2: impedir volver al catálogo tras cerrar sesión, incluso si el
 * usuario navega directamente a la URL o usa el botón "Atrás".
 *
 * TODO (equipo): agregar este guard (canActivate: [authGuard]) a TODAS las rutas
 * protegidas que creen para sus historias de usuario (catálogo, carrito, admin, etc.).
 */
export const authGuard: CanActivateFn = () => {
  const router = inject(Router);
  const hasToken = !!localStorage.getItem(APP_CONSTANTS.STORAGE_KEYS.TOKEN);

  if (!hasToken) {
    router.navigateByUrl(APP_CONSTANTS.ROUTES.LOGIN, { replaceUrl: true });
    return false;
  }

  return true;
};
