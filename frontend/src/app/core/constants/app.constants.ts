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
    // ej. CARTS: '/carts', USERS: '/users'
  },
  PRODUCTS: {
    // El backend conserva /api/auth y expone los endpoints solicitados en /products.
    API_URL: 'https://localhost:7000/products',
    CREATE_PATH: 'products/new',
    EDIT_PATH: 'products/:id/edit',
    DETAIL_PATH: 'products/:id',
    ROOT: '/products',
    NEW: '/products/new',
    EDIT_SEGMENT: 'edit',
    ID_PARAMETER: 'id',
    // Cambiar únicamente esta constante cuando el equipo publique el catálogo.
    CATALOG: '/home',
    HTTPS_PROTOCOL: 'https:',
    MIN_PRICE: 0.01,
    MAX_PRICE: 999999999,
    TOAST_DURATION: 4500,
    TEXT: {
      CREATE: 'Crear producto', EDIT: 'Editar producto', DETAIL: 'Detalle del producto',
      TITLE: 'Título', PRICE: 'Precio', DESCRIPTION: 'Descripción',
      IMAGE: 'URL de imagen HTTPS', CATEGORY: 'Categoría',
      SAVE: 'Guardar', DELETE: 'Eliminar', BACK: 'Volver', LOADING: 'Procesando…',
      CREATED: 'Producto creado. ID: ',
      UPDATED: 'Producto actualizado', DELETED: 'Producto eliminado',
      CONFIRM_DELETE: '¿Estás seguro de eliminar este producto?',
      ERROR: 'No se pudo completar la solicitud. Revisa tu conexión e intenta de nuevo.',
      FORBIDDEN: 'No tienes permiso para realizar esta acción.',
      NOT_FOUND: 'No se encontró el producto.',
      REQUIRED: 'Completa este campo.', PRICE_ERROR: 'Escribe un precio entre 0.01 y 999999999.',
      IMAGE_ERROR: 'Escribe una URL HTTPS válida.', INVALID_ID: 'El identificador no es válido.',
      HTTPS_ERROR: 'La conexión con la API debe utilizar HTTPS.',
    },
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
  // US03: textos fijos de la vista del catálogo (los productos vienen de GET /products).
  CATALOG: {
    TITLE: 'Catálogo de productos',
    LOADING: 'Cargando productos...',
    EMPTY: 'Todavía no hay productos en el catálogo.',
    RETRY: 'Reintentar',
    CURRENCY_CODE: 'USD',
  },
} as const;
