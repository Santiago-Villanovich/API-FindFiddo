using FindFiddo.Abstractions;

namespace FindFiddo.Entities
{
    public class User: Entity, IVerificable
    {
        public List<Rol>? rol { get; set; }

        [VerificableProperty]
        public string? nombres { get; set; }
        [VerificableProperty]
        public string? apellidos { get; set; }
        [VerificableProperty]
        public string? dni { get; set; }
        [VerificableProperty]
        public string? email { get; set; }
        public string? telefono { get; set; }
        public string? password { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public string? direccion { get; set; }
        public int codigoPostal { get; set; } 
        public byte[]? salt { get; set; } 
        public string? DV { get; set; }

        public User() { }
    }

    public class LoginUser
    {
        public string email { get; set; } = null!;
        public string password { get; set; } = null!;
        public LoginUser() { }
    }

    public class LogedUser: Entity
    {
        public string? telefono { get; set; } = null!;
        public string? email { get; set; } = null!;
        public string? nombres { get; set; } = null!;
        public string?   apellidos { get; set; } = null!;
        //public List<Rol>? rol { get; set; } = null!;

        public LogedUser() { }
        public LogedUser(Guid id,string? telefono, string? email, string? nombres, string? apellidos)
        {
            this.Id = id;
            this.telefono = telefono;
            this.email = email;
            this.nombres = nombres;
            this.apellidos = apellidos;
        }
    }
}
