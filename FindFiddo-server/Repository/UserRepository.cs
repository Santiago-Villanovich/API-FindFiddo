using FindFiddo.Abstractions;
using FindFiddo.DataAccess;
using FindFiddo.Entities;
using FindFiddo_server.Entities;

namespace FindFiddo.Repository
{
    public interface IUserRepository : ICrud<User>
    {
        User GetUserByEmail(string email);
        List<Rol> GetUserRols(Guid idUsuario);
        LogedUser signUP(User user);
        void InsertUserLog(Guid id, string accion);
        List<UserLog> GetLog(DateTime from, DateTime to, string accion, int pag);

        void InsertOrganizacion(Organizacion organizacion);
        void DeleteOrganizacion(Guid id_organizacion);
        IList<Organizacion> getOrganizaciones(DateTime from, DateTime to, string accion, int pag);
        Organizacion getOrganizacionByID(Guid id);

        void Asignar_Usuario_Organizacion(Guid Id_ususario, Guid Id_organizacion);

        void Desasignar_Usuario_Organizacion(Guid Id_ususario, Guid Id_organizacion);
    }
    public class UserRepository : IUserRepository
    {
        IUserContext _ctx;
        public UserRepository(IUserContext ctx)
        {
            _ctx = ctx;
        }

        public void DeleteById(Guid id)
        {
            _ctx.DeleteById(id);
        }

        public IList<User> GetAll()
        {
            return _ctx.GetAll();
        }

        public User GetById(Guid id)
        {
            return _ctx.GetById(id);
        }

        public User GetUserByEmail(string email)
        {
            return _ctx.GetUserByEmail(email);
        }

        public List<Rol> GetUserRols(Guid idUsuario)
        {
            return _ctx.GetUserRols(idUsuario);
        }

        public User Save(User entity)
        {
            throw new NotImplementedException();
        }

        public LogedUser signUP(User user)
        {
            var u = _ctx.signUP(user);

            _ctx.InsertUserRol(u.Id, user.rol);
            _ctx.InsertUserLog(u.Id, "SignUp");
            
            return u;
        }

        public List<UserLog> GetLog(DateTime from, DateTime to, string accion, int pag)
        {
            return _ctx.GetLog(from, to, accion, pag);
        }

        public void InsertUserLog(Guid id, string accion)
        {
            _ctx.InsertUserLog(id, accion);
        }

        public void InsertOrganizacion(Organizacion organizacion)
        {
            _ctx.InsertOrganizacion(organizacion);
        }

        public void DeleteOrganizacion(Guid id_organizacion)
        {
             _ctx.DeleteOrganizacion(id_organizacion);
        }

        public IList<Organizacion> getOrganizaciones(DateTime from, DateTime to, string accion, int pag)
        {
           return _ctx.getOrganizaciones(from, to, accion, pag);
        }

        public Organizacion getOrganizacionByID(Guid id)
        {
            return _ctx.getOrganizacionByID(id);
        }

        public void Asignar_Usuario_Organizacion(Guid Id_ususario, Guid Id_organizacion)
        {
            _ctx.Asignar_Usuario_Organizacion(Id_ususario, Id_organizacion);
        }

        public void Desasignar_Usuario_Organizacion(Guid Id_ususario, Guid Id_organizacion)
        {
            _ctx.Desasignar_Usuario_Organizacion(Id_ususario, Id_organizacion);
        }
    }
}
