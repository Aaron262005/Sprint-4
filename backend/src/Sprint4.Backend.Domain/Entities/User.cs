namespace Sprint4.Backend.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        
        // Propiedades originales que tu equipo necesita para el Login:
        public string FullName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        
        // Propiedades nuevas para tu Historia de Usuario (US11):
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = "Cliente";
        public Address Address { get; set; } = new Address();
    }

    public class Address
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public Geolocation Geolocation { get; set; } = new Geolocation();
    }

    public class Geolocation
    {
        public string Lat { get; set; } = string.Empty;
        public string Long { get; set; } = string.Empty;
    }
}