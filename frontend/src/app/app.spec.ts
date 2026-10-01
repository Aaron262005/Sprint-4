import { TestBed } from '@angular/core/testing';
import { signal } from '@angular/core';
import { App } from './app';
import { PRODUCT_FEEDBACK } from './core/services/product-feedback.interface';

describe('App', () => {
  it('mantiene el aviso de producto fuera de las pantallas que cambian', async () => {
    await TestBed.configureTestingModule({ imports: [App], providers: [
      { provide: PRODUCT_FEEDBACK, useValue: { message: signal('Producto actualizado (Simulación)') } },
    ] }).compileComponents();
    const fixture = TestBed.createComponent(App);
    await fixture.whenStable();
    expect(fixture.nativeElement.querySelector('[role="status"]').textContent).toContain('Producto actualizado');
  });
});
