/**
 * Debe coincidir exactamente con Domain/Enums/UserRole.cs del backend
 * (el backend serializa el enum como texto: "Administrador" | "Cliente" | "Auditor").
 */
export enum UserRole {
  Administrador = 'Administrador',
  Cliente = 'Cliente',
  Auditor = 'Auditor',
}
