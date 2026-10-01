/**
 * Modelo de un producto del catálogo (US03).
 * Empata exactamente con ProductDto del backend, que a su vez replica la
 * estructura JSON de la Fake Store API (nota "Mapeo del JSON" de la historia).
 */
export interface ProductRating {
  rate: number;
  count: number;
}

export interface Product {
  id: number;
  title: string;
  price: number;
  description: string;
  category: string;
  /** URL (texto) de la imagen; la Vista la descarga de forma asíncrona. */
  image: string;
  rating: ProductRating;
}
