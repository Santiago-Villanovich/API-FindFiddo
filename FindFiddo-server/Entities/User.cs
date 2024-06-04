using FindFiddo.Abstractions;

namespace FindFiddo.Entities
{
    public class User: Entity, IVerificable
    {
        public Rol rol { get; set; } = null!;
        public string rolUsuario {  get; set; }
        [VerificableProperty]
        public string nombres { get; set; } = null!;
        [VerificableProperty]
        public string apellidos { get; set; } = null!;
        [VerificableProperty]
        public string dni { get; set; } = null!;
        [VerificableProperty]
        public string email { get; set; } = null!;
        public string telefono { get; set; } = null!;
        public string password { get; set; } = null!;
        public DateTime fechaNacimiento { get; set; }
        public string direccion { get; set; } = null!;
        public int codigoPostal { get; set; } 
        public DateTime FechaCreacion { get; set; }
        public byte[] salt { get; set; } = null!;
        public string DV { get; set; } = null!;

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
        public string dni { get; set; } = null!;
        public string email { get; set; } = null!;
        public string nombres { get; set; } = null!;
        public string apellidos { get; set; } = null!;

    }
}
