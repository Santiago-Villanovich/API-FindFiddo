using FindFiddo.Entities;

namespace FindFiddo_server.Entities
{
    public class Publicacion:Entity
    {
        public User usuario { get; set; }
        public TipoPublicacion tipo { get; set; }
        public List<Opcion>? opciones { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string  historia { get; set; }
        public string ubicacion { get; set; }

    }

    public class TipoPublicacion:Entity
    {
        public string nombre { get; set; }
        public TipoPublicacion(string nombre) {
            this.nombre = nombre;
        }
        public TipoPublicacion(Guid id, string nombre)
        {
            this.Id = id;
            this.nombre = nombre;
        }
    }
}
