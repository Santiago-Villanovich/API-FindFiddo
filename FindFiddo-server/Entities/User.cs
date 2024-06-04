using FindFiddo.Abstractions;

namespace FindFiddo.Entities
{
    public class User: Entity, IVerificable
    {
        public Rol rol { get; set; }
        public string? rolUsuario {  get; set; }
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

    public class LoguedUser
    {
        public string? dni { get; set; } = null!;
        public string? email { get; set; } = null!;
        public string? nombres { get; set; } = null!;
        public string?   apellidos { get; set; } = null!;

    }
}
