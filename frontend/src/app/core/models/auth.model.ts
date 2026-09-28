import { UserRole } from './user-role.enum';

/** Cuerpo enviado a POST /auth/login (debe calzar con LoginRequestDto del backend). */
export interface LoginRequest {
  username: string;
  password: string;
}

/** Respuesta de POST /auth/login (debe calzar con LoginResult del backend). */
export interface LoginResponse {
  success: boolean;
  token?: string;
  userId?: number;
  username?: string;
  role?: UserRole;
  errorMessage?: string;
}
