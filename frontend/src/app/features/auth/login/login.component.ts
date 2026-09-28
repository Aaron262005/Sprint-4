import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthViewModel } from '../../../core/view-models/auth.view-model';

/**
 * Vista de Login (US01). Es "tonta" a propósito: no contiene lógica de negocio,
 * solo enlaza el formulario a la ViewModel y refleja su estado (isLoading/errorMessage).
 * Esto separa claramente Vista (este archivo) de ViewModel (AuthViewModel) — patrón MVVM.
 */
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  private readonly fb = inject(FormBuilder);
  readonly viewModel = inject(AuthViewModel);

  readonly form = this.fb.nonNullable.group({
    username: ['', Validators.required],
    password: ['', Validators.required],
  });

  onSubmit(): void {
    if (this.form.invalid) return;

    const { username, password } = this.form.getRawValue();
    this.viewModel.login(username, password);
  }
}
