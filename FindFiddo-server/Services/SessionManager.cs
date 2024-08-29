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

      /// <summary>
      ///  public Idioma idioma { get; set; }
      /// </summary>

      //  static IList<IdiomaObserver> Observadores = new List<IdiomaObserver>();  //creo lista de observadores
        //public BEUsuario Usuario { get; set; }
        public User usuario { get; set; }
        public DateTime FechaInicio { get; set; }
       // public List<int> permisos { get; set; }

        public static SessionManager GetInstance
        {
            get
            {
                if (_session == null) _session = new SessionManager();

                return _session;
            }
        }

        public static void Login(User usuario)
        {

            lock (_lock)
            {
                if (_session != null)
                {
                    _session.usuario = usuario;
                    _session.FechaInicio = DateTime.Now;
                }
                else
                {
                    throw new Exception("No hay sesion iniciada");
                }
            }
        }
      /*  public static bool recursiva(int id, IList<Componente> roles)
        {
            foreach (Componente rol in roles)
            {
                if (rol.Id == id) return true;
                if (rol.Hijos != null) return recursiva(id, rol.Hijos);
            }
            return false;
        }
        public static bool tiene_permiso(int id)
        {
            foreach (Componente rol in _session.Usuario.permisos)
            {
                if (rol.Id == id) return true;
                if (rol.Hijos != null)
                {
                    if (recursiva(id, rol.Hijos)) return true;

                }
            }
            return false;
        }*/
        public static void Logout()
        {
            lock (_lock)
            {
                if (_session != null)
                {
                    _session = null;
                }
                else
                {
                    throw new Exception("Sesión no iniciada");
                }
            }


        }

        private SessionManager()
        {

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
