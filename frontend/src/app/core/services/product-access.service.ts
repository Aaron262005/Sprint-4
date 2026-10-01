import { Injectable, inject } from '@angular/core';
import { APP_CONSTANTS } from '../constants/app.constants';
import { UserRole } from '../models/user-role.enum';
import { ISessionStorageService } from './storage.service';

@Injectable({ providedIn: 'root' })
export class ProductAccessService {
  private readonly storage = inject(ISessionStorageService);

  canWrite(): boolean {
    return !!this.storage.get(APP_CONSTANTS.STORAGE_KEYS.TOKEN)
      && this.storage.get(APP_CONSTANTS.STORAGE_KEYS.ROLE) === UserRole.Administrador;
  }
}
