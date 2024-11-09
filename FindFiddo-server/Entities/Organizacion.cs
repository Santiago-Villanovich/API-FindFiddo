using FindFiddo.Abstractions;
using FindFiddo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindFiddo_server.Entities
{
    public class Organizacion : Entity, IVerificable
    {
        public string nombre { get; set; }
        public string razon_social { get; set; }
        public string direccion { get; set; }
        public int codigo_postal { get; set; }
        public string DV { get; set; }
    }
}
