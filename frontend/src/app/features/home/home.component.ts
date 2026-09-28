import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { APP_CONSTANTS } from '../../core/constants/app.constants';
import { AuthViewModel } from '../../core/view-models/auth.view-model';
import { UserRole } from '../../core/models/user-role.enum';

/**
 * Pantalla principal MOCK. Sirve para validar el flujo de login/logout y para que
 * el equipo vea dónde debe "engancharse" cada historia de usuario.
 *
 * TODO (equipo catálogo): reemplazar el contenido de esta vista por el catálogo real (US03).
 */
@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
})
export class HomeComponent {
  private readonly viewModel = inject(AuthViewModel);

  private readonly storedUser = JSON.parse(
    localStorage.getItem(APP_CONSTANTS.STORAGE_KEYS.USER) ?? '{}'
  );

  readonly role = (localStorage.getItem(APP_CONSTANTS.STORAGE_KEYS.ROLE) ?? UserRole.Cliente) as UserRole;
  readonly username: string = this.storedUser?.username ?? '';
  private readonly userId: number = this.storedUser?.id ?? 0;

  /** Switch en vez de if/else anidados: fácil de leer y de extender por rol. */
  get menuLabel(): string {
    switch (this.role) {
      case UserRole.Administrador:
        return 'Panel de Administrador — gestión de productos y usuarios (US06, US07, US08, US11)';
      case UserRole.Auditor:
        return 'Panel de Auditor — histórico de carritos globales (US12)';
      case UserRole.Cliente:
      default:
        return 'Catálogo de productos (US03, US04, US05, US09, US10)';
    }
  }

  onLogout(): void {
    this.viewModel.logout(this.userId);
  }
}
