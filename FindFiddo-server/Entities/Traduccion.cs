namespace FindFiddo_server.Entities
{
    public class Traduccion
    {
        public Termino termino { get; set; }
        public string texto { get; set; }

        public Traduccion() { }
    }

    public class TraduccionDTO
    {
        public int id { get; set; }
        public string termino { get; set; }
        public string traduccion{ get; set; }

        public TraduccionDTO() { }
        public TraduccionDTO(int id, string termino,string traduccion) { 
            this.id = id;
            this.termino = termino;
            this.traduccion = traduccion;
        }
    }
}
