import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, Router, convertToParamMap } from '@angular/router';
import { of, Subject, throwError } from 'rxjs';
import { signal } from '@angular/core';
import { ProductFormViewModel } from './product-form.view-model';
import { ProductDetailViewModel } from './product-detail.view-model';
import { PRODUCT_READER, PRODUCT_WRITER } from '../services/product-api.interface';
import { PRODUCT_FEEDBACK } from '../services/product-feedback.interface';
import { ProductAccessService } from '../services/product-access.service';
import { Product } from '../models/product.model';

const product: Product = { id: 1, title: 'Cuaderno', price: 25, description: 'Azul',
  image: 'https://example.com/a.jpg', category: 'Escuela' };

describe('Épica 3: formularios y eliminación', () => {
  const reader = { get: vi.fn() };
  const writer = { create: vi.fn(), update: vi.fn(), delete: vi.fn() };
  const feedback = { message: signal(''), created: vi.fn(), toast: vi.fn(), confirmDelete: vi.fn() };
  const access = { canWrite: vi.fn() };
  const router = { navigate: vi.fn(), navigateByUrl: vi.fn() };
  function setup(id?: string) {
    TestBed.configureTestingModule({ providers: [ProductFormViewModel, ProductDetailViewModel,
      { provide: ActivatedRoute, useValue: { paramMap: of(convertToParamMap(id ? { id } : {})) } },
      { provide: Router, useValue: router }, { provide: PRODUCT_READER, useValue: reader },
      { provide: PRODUCT_WRITER, useValue: writer }, { provide: PRODUCT_FEEDBACK, useValue: feedback },
      { provide: ProductAccessService, useValue: access },
    ] });
  }
  beforeEach(() => {
    vi.clearAllMocks();
    reader.get.mockReturnValue(of(product));
    writer.create.mockReturnValue(of({ ...product, id: 21 }));
    writer.update.mockReturnValue(of(product));
    writer.delete.mockReturnValue(of(product));
    access.canWrite.mockReturnValue(true);
    feedback.confirmDelete.mockResolvedValue(true);
  });
  it('no envía campos vacíos y los marca para mostrar errores', () => {
    setup(); const vm = TestBed.inject(ProductFormViewModel); vm.save();
    expect(writer.create).not.toHaveBeenCalled();
    expect(vm.form.controls.title.touched).toBe(true);
  });
  it('rechaza letras en precio y URL HTTP', () => {
    setup(); const vm = TestBed.inject(ProductFormViewModel); vm.form.patchValue(product);
    vm.form.controls.price.setValue('abc' as unknown as number); vm.save();
    expect(writer.create).not.toHaveBeenCalled();
    vm.form.controls.price.setValue(25); vm.form.controls.image.setValue('http://example.com/a.jpg'); vm.save();
    expect(writer.create).not.toHaveBeenCalled();
  });
  it('muestra el ID y limpia después de crear', () => {
    setup(); const vm = TestBed.inject(ProductFormViewModel); vm.form.patchValue(product); vm.save();
    expect(feedback.created).toHaveBeenCalledWith(21);
    expect(vm.form.controls.title.value).toBe('');
  });
  it('precarga todos los campos y actualiza usando PUT', () => {
    setup('1'); const vm = TestBed.inject(ProductFormViewModel);
    expect(vm.form.controls.image.value).toBe(product.image);
    vm.form.controls.title.setValue('Nuevo'); vm.save();
    expect(writer.update).toHaveBeenCalledWith(1, expect.objectContaining({ title: 'Nuevo' }));
    expect(router.navigate).toHaveBeenCalledWith(['/products', 1], { replaceUrl: true });
  });
  it('bloquea el doble toque', () => {
    setup(); const pending = new Subject<Product>(); writer.create.mockReturnValue(pending);
    const vm = TestBed.inject(ProductFormViewModel); vm.form.patchValue(product); vm.save(); vm.save();
    expect(writer.create).toHaveBeenCalledTimes(1); expect(vm.busy()).toBe(true);
    pending.next(product); pending.complete(); expect(vm.busy()).toBe(false);
  });
  it('conserva el formulario y habilita reintento tras error', () => {
    setup(); writer.create.mockReturnValue(throwError(() => new Error()));
    const vm = TestBed.inject(ProductFormViewModel); vm.form.patchValue(product); vm.save();
    expect(vm.busy()).toBe(false); expect(vm.form.controls.title.value).toBe(product.title);
    expect(vm.error()).not.toBe('');
  });
  it('cancelar no elimina ni navega', async () => {
    setup('1'); feedback.confirmDelete.mockResolvedValue(false);
    const vm = TestBed.inject(ProductDetailViewModel); await vm.remove();
    expect(writer.delete).not.toHaveBeenCalled(); expect(router.navigateByUrl).not.toHaveBeenCalled();
    expect(vm.product()).toEqual(product); expect(vm.busy()).toBe(false);
  });
  it('confirmar elimina y vuelve a la pantalla principal', async () => {
    setup('1'); const vm = TestBed.inject(ProductDetailViewModel); await vm.remove();
    expect(writer.delete).toHaveBeenCalledWith(1); expect(feedback.toast).toHaveBeenCalled();
    expect(router.navigateByUrl).toHaveBeenCalledWith('/home', { replaceUrl: true });
  });
  it('un rol sin permiso no elimina ni abre confirmación', async () => {
    setup('1'); access.canWrite.mockReturnValue(false);
    const vm = TestBed.inject(ProductDetailViewModel); await vm.remove();
    expect(feedback.confirmDelete).not.toHaveBeenCalled(); expect(writer.delete).not.toHaveBeenCalled();
  });
});
