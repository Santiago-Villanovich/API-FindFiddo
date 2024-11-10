using FindFiddo.Abstractions;
using FindFiddo_server.Entities;

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
        [VerificableProperty]
        public string? telefono { get; set; }
        [VerificableProperty]
        public string? password { get; set; }
        [VerificableProperty]
        public DateTime fechaNacimiento { get; set; }
        [VerificableProperty]
        public string? direccion { get; set; }
        [VerificableProperty]
        public int codigoPostal { get; set; }
        [VerificableProperty]
        public byte[]? salt { get; set; }
        public string? DV { get; set; }
        public Guid? idioma_preferido { get; set; }

        public List<Opcion> preferencias { get; set; }
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
        public List<Rol>? rol { get; set; } = null!;

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
