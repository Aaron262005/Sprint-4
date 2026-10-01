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
    // US03: catálogo general de productos.
    PRODUCTS: {
      ALL: '/products',
    },
    // TODO (equipo catálogo/carrito/usuarios): agreguen aquí sus propias rutas,
    // ej. CARTS: '/carts', USERS: '/users'
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
    CATALOG_LOAD_FAILED: 'No pudimos cargar el catálogo. Revisa tu conexión e intenta de nuevo.',
  },
  // US03: textos fijos de la vista del catálogo.
  CATALOG: {
    TITLE: 'Catálogo de productos',
    LOADING: 'Cargando productos...',
    EMPTY: 'No hay productos disponibles por ahora.',
    RETRY: 'Reintentar',
    CURRENCY_CODE: 'USD',
  },
} as const;
