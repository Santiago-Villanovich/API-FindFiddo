using FindFiddo.Entities;

namespace FindFiddo_server.Entities
{
    public class Traduccion:Entity
    {
        public Termino termino { get; set; }
        public string texto { get; set; }

        public Traduccion() { }
    }

    public class TraduccionDTO
    {
        public Guid id { get; set; }
        public string termino { get; set; }
        public string traduccion{ get; set; }

        public TraduccionDTO() { }
        public TraduccionDTO(Guid id, string termino,string traduccion) { 
            this.id = id;
            this.termino = termino;
            this.traduccion = traduccion;
        }
    }
}
