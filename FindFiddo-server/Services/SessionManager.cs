using FindFiddo_server.Entities;
using FindFiddo.Abstractions;
using FindFiddo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FindFiddo_server.Services
{
    public class SessionManager
    {
        private static object _lock = new Object();
        private static SessionManager _session;
        public User usuario { get; set; }
        public DateTime FechaInicio { get; set; }

        public static SessionManager GetInstance
        {
            get
            {
                lock (_lock)
                {
                    if (_session == null)
                        _session = new SessionManager();
                }
                return _session;
            }
        }

        public static void Login(User usuario)
        {

            lock (_lock)
            {
                if (_session == null)
                {
                    _session = new SessionManager();
                }
                _session.usuario = usuario;
                _session.FechaInicio = DateTime.Now;
            }
        }
        public static void Logout()
        {
           
                lock (_lock)
                {
                    _session = null;
                }
        }

        
        public static bool TraerUsuario()
        {
            if (_session != null)
            {
                return _session.usuario != null;
            }
            return false;
        }
    }
}
