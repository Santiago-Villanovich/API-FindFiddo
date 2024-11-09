using FindFiddo.Entities;

namespace FindFiddo_server.Entities
{
    public class Categoria:Entity
    {
        public string nombre {  get; set; }
        public List<Opcion>? opciones { get; set; }
    }
}
