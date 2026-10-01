import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { firstValueFrom } from 'rxjs';
import { ProductHttpService } from './product-http.service';
import { ProductAccessService } from './product-access.service';
import { ISessionStorageService } from './storage.service';
import { APP_CONSTANTS } from '../constants/app.constants';
import { adminRoleGuard } from '../guards/admin-role.guard';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot, provideRouter } from '@angular/router';

const data = { title: 'Cuaderno', price: 25, description: 'Azul', image: 'https://example.com/a.jpg', category: 'Escuela' };

describe('Productos: HTTPS, datos del backend y permisos', () => {
  const access = { canWrite: vi.fn() };
  beforeEach(() => {
    access.canWrite.mockReturnValue(true);
    TestBed.configureTestingModule({ providers: [ProductHttpService, provideHttpClient(), provideHttpClientTesting(),
      provideRouter([]), { provide: ProductAccessService, useValue: access },
      { provide: ISessionStorageService, useValue: { get: () => 'test-session' } },
    ] });
  });
  afterEach(() => TestBed.inject(HttpTestingController).verify());
  it('envía POST por HTTPS', () => {
    TestBed.inject(ProductHttpService).create(data).subscribe();
    const request = TestBed.inject(HttpTestingController).expectOne(APP_CONSTANTS.PRODUCTS.API_URL);
    expect(request.request.method).toBe('POST');
    expect(new URL(request.request.url).protocol).toBe('https:');
    request.flush({ id: 21, ...data });
  });
  it('consulta el backend después del PUT para mostrar el dato guardado', async () => {
    const api = TestBed.inject(ProductHttpService);
    const updated = { id: 1, ...data, title: 'Nuevo' };
    const response = firstValueFrom(api.update(1, updated));
    const request = TestBed.inject(HttpTestingController).expectOne(`${APP_CONSTANTS.PRODUCTS.API_URL}/1`);
    expect(request.request.method).toBe('PUT'); request.flush(updated); await response;
    const detail = firstValueFrom(api.get(1));
    const get = TestBed.inject(HttpTestingController).expectOne(`${APP_CONSTANTS.PRODUCTS.API_URL}/1`);
    expect(get.request.method).toBe('GET');
    get.flush(updated);
    expect(await detail).toEqual(updated);
  });
  it('bloquea escrituras directas del cliente antes de HttpClient', async () => {
    access.canWrite.mockReturnValue(false);
    await expect(firstValueFrom(TestBed.inject(ProductHttpService).delete(1))).rejects.toThrow();
    TestBed.inject(HttpTestingController).expectNone(`${APP_CONSTANTS.PRODUCTS.API_URL}/1`);
  });
  it('el guard envía al usuario sin permiso a la pantalla principal', () => {
    access.canWrite.mockReturnValue(false);
    const result = TestBed.runInInjectionContext(() => adminRoleGuard({} as ActivatedRouteSnapshot, {} as RouterStateSnapshot));
    expect(result).toEqual(TestBed.inject(Router).createUrlTree(['/home']));
  });
});
