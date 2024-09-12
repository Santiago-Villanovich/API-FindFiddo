using FindFiddo.Entities;

namespace FindFiddo_server.Entities
{
    public class Traduccion:Entity
    {
        public Termino termino { get; set; }
        public string texto { get; set; }
    }
}
