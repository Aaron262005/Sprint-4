import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { APP_CONSTANTS } from '../constants/app.constants';
import { ProductAccessService } from '../services/product-access.service';

export const adminRoleGuard: CanActivateFn = () =>
  inject(ProductAccessService).canWrite()
    ? true
    : inject(Router).createUrlTree([APP_CONSTANTS.PRODUCTS.CATALOG]);
