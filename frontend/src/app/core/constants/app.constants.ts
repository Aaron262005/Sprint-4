/**
 * ÚNICO archivo de constantes de todo el frontend.
 * Regla del equipo: ninguna URL, clave de storage o mensaje se escribe suelto
 * en un componente o servicio. Todo valor fijo vive aquí, así un cambio
 * (por ejemplo, la URL del backend) se hace en un solo lugar.
 */
export const APP_CONSTANTS = {
  API: {
    // TODO (equipo): ajustar al puerto real donde corra Sprint4.Backend.API
    BASE_URL: 'https://localhost:7000/api',
    AUTH: {
      LOGIN: '/auth/login',
      LOGOUT: '/auth/logout',
    },
    // TODO (equipo catálogo/carrito/usuarios): agreguen aquí sus propias rutas,
    // ej. PRODUCTS: '/products', CARTS: '/carts', USERS: '/users'
  },
  STORAGE_KEYS: {
    TOKEN: 'sprint4_token',
    USER: 'sprint4_user',
    ROLE: 'sprint4_role',
  },
  ROUTES: {
    LOGIN: '/login',
    HOME: '/home',
  },
  ERROR_MESSAGES: {
    INVALID_CREDENTIALS: 'Usuario o contraseña inválidos',
    NO_CONNECTION: 'No hay conexión a internet. Verifica tu red e intenta de nuevo.',
  },
} as const;
