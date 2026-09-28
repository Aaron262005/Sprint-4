import { UserRole } from '../../core/models/user-role.enum';

/**
 * Datos de ejemplo para maquetar pantallas (catálogo, usuarios, carritos)
 * ANTES de que la historia de usuario correspondiente esté implementada contra
 * el backend real. Cada equipo debe dejar de usar este mock en cuanto conecte
 * su Query/Command real (ver README de Application/Features).
 */
export const MOCK_USERS = [
  { id: 1, username: 'admin1', fullName: 'Aarón Pérez', role: UserRole.Administrador },
  { id: 2, username: 'admin2', fullName: 'Laura Gómez', role: UserRole.Administrador },
  { id: 3, username: 'auditor1', fullName: 'Carlos Auditor', role: UserRole.Auditor },
  { id: 4, username: 'cliente1', fullName: 'Erick Rafael', role: UserRole.Cliente },
  { id: 5, username: 'cliente2', fullName: 'Mariana López', role: UserRole.Cliente },
];

/** Mock de productos, solo para maquetar el catálogo (US03/US04/US05) antes de tener la API. */
export const MOCK_PRODUCTS = [
  { id: 1, title: 'Producto de ejemplo A', category: 'electronica', price: 199.99 },
  { id: 2, title: 'Producto de ejemplo B', category: 'ropa', price: 49.5 },
  { id: 3, title: 'Producto de ejemplo C', category: 'hogar', price: 89.0 },
];
