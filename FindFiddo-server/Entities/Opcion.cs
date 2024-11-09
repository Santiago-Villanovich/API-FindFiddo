using FindFiddo.Entities;

namespace FindFiddo_server.Entities
{
    public class Opcion:Entity
    {
        public string nombre { get; set; }
        public Opcion(string nombre) {
            this.nombre = nombre;
        }
        public Opcion(Guid id, string nombre)
        {
            this.Id = id;
            this.nombre = nombre;
        }
    }
}
