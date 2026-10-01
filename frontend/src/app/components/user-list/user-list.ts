import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService, User } from '../../services/user';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-list.html',
  styleUrls: ['./user-list.scss']
})
export class UserListComponent implements OnInit {
  users: User[] = [];
  isLoading: boolean = true;
  hasError: boolean = false;

  constructor(private userService: UserService, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.cargarUsuarios();
  }

  cargarUsuarios(): void {
    this.isLoading = true;
    this.hasError = false;
    console.log("1. Pidiendo datos al backend...");
    
    this.userService.getUsers().subscribe({
      next: (data) => {
        console.log("2. ¡Datos recibidos con éxito!", data);
        this.users = data;
        this.isLoading = false;
        this.cdr.detectChanges(); // Empujón manual para que se quite el "Cargando..." y pinte la tabla
      },
      error: (error) => {
        console.error("2. Ocurrió un error:", error);
        this.isLoading = false;
        this.hasError = true;
        this.cdr.detectChanges(); // Empujón manual para mostrar el mensaje de error visual
      }
    });
  }
}