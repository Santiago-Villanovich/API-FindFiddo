using FindFiddo.Entities;

namespace FindFiddo_server.Entities
{
    public class UserLog : Entity
    {
        public LogedUser user { get; set; }
        public string accion { get; set; }
        public DateTime fecha { get; set; }
        public UserLog() { }
    }
}
