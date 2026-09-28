import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { APP_CONSTANTS } from '../constants/app.constants';
import { LoginRequest, LoginResponse } from '../models/auth.model';

/**
 * Única responsabilidad: comunicación HTTP con el backend de autenticación (US01/US02).
 * No conoce el estado de la app ni el almacenamiento — eso es trabajo del AuthViewModel.
 * (Modelo, en términos de MVVM).
 */
@Injectable({ providedIn: 'root' })
export class AuthApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = APP_CONSTANTS.API.BASE_URL;

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.baseUrl}${APP_CONSTANTS.API.AUTH.LOGIN}`, request);
  }

  logout(userId: number): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}${APP_CONSTANTS.API.AUTH.LOGOUT}`, { userId });
  }
}
