export interface ProductData {
  title: string;
  price: number;
  description: string;
  image: string;
  category: string;
}

export interface Product extends ProductData {
  id: number;
}
