using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindFiddo.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        public DateTime? fechaCreacion {  get; set; }
    }
}
