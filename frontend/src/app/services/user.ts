import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

// Aquí le explicamos a Angular cómo viene la estructura desde tu backend
export interface User {
  id: number;
  fullName: string;
  name: string;
  email: string;
  phone: string;     // Dato requerido por Escenario 1
  username: string;  // Dato requerido por Escenario 1
  role: string;
  address?: {
    city?: string;
  };
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  // Esta es la misma URL que probaste en Swagger
  private apiUrl = 'https://localhost:7000/api/Users';

  constructor(private http: HttpClient) { }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl);
  }
}
