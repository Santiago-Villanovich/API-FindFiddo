using FindFiddo.Entities;

namespace FindFiddo_server.Entities
{
    public class Publicacion:Entity
    {
        public User Usuario { get; set; }

        public string tipo { get; set; }

        public string id_opcion { get; set; }

        public string descripcion { get; set; }

        public string  historia { get; set; }
        public string ubicacion { get; set; }
        public DateTime Fecha_Alta { get; set; }

    }
}
