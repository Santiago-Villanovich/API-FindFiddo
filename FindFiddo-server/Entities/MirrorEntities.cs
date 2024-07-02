using System.Globalization;

namespace FindFiddo_server.Entities
{
    public class MirrorUser
    {
        public MirrorUser() { }

        public string tabla {  get; set; }
        public string? usuarioActual { get; set; }
        public string? usuarioMrr { get; set; }
        public string actionType { get; set; }
        public string? camposModificados { get; set; }
        
    }
}
