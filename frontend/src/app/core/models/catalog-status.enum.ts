/**
 * Estados mutuamente excluyentes de la vista del catálogo (US03).
 * Cada valor corresponde a un escenario de la historia:
 *  - Loading -> Escenario 2 (indicador de carga)
 *  - Loaded  -> Escenario 1 (renderizado del catálogo)
 *  - Error   -> Escenario 3 (mensaje amigable + "Reintentar")
 */
export enum CatalogStatus {
  Loading = 'loading',
  Loaded = 'loaded',
  Error = 'error',
}
