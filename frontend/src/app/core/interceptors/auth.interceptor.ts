import { HttpInterceptorFn } from '@angular/common/http';
import { APP_CONSTANTS } from '../constants/app.constants';

/**
 * Agrega el token de sesión a toda petición HTTP saliente, sin que cada
 * historia de usuario tenga que preocuparse de leer el token manualmente.
 */
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem(APP_CONSTANTS.STORAGE_KEYS.TOKEN);

  if (!token) {
    return next(req);
  }

  return next(
    req.clone({
      setHeaders: { Authorization: `Bearer ${token}` },
    })
  );
};
