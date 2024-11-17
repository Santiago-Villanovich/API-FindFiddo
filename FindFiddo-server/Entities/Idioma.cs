using FindFiddo.Entities;

namespace FindFiddo_server.Entities
{
    public class Idioma:Entity
    {
        public string codigo {  get; set; }
        public string descripcion { get; set; }
        public bool isDefault { get; set; }
    }
}
