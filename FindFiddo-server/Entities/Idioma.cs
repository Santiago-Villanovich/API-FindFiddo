using FindFiddo.Entities;

namespace FindFiddo_server.Entities
{
    public class Idioma:Entity
    {
        public string nombre { get; set; }
        public bool isDefault { get; set; }
    }
}
