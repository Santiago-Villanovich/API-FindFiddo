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

        Organizacion SaveOrganizacion(Organizacion organizacion);
        void DeleteOrganizacion(Guid id_organizacion);
        IList<Organizacion> GetAllOrganizaciones(Guid idUser, Guid idOrg);
        //Organizacion getOrganizacionByID(Guid id);

        void Asignar_Usuario_Organizacion(Guid Id_ususario, Guid Id_organizacion);

        void Desasignar_Usuario_Organizacion(Guid Id_ususario, Guid Id_organizacion);

        void SavePreferenciaForUser(Guid idUser, List<Opcion> opciones);

        IList<Publicacion> GetMatchsForUser(Guid IdUser);

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

        public Organizacion SaveOrganizacion(Organizacion organizacion)
        {
            return _ctx.SaveOrganizacion(organizacion);
        }

        public void DeleteOrganizacion(Guid id_organizacion)
        {
             _ctx.DeleteOrganizacion(id_organizacion);
        }

        public IList<Organizacion> GetAllOrganizaciones(Guid idUser, Guid idOrg)
        {
           return _ctx.GetAllOrganizaciones( idUser,idOrg);
        }

        /*public Organizacion getOrganizacionByID(Guid id)
        {
            return _ctx.getOrganizacionByID(id);
        }*/

        public void Asignar_Usuario_Organizacion(Guid Id_ususario, Guid Id_organizacion)
        {
            _ctx.Asignar_Usuario_Organizacion(Id_ususario, Id_organizacion);
        }

        public void Desasignar_Usuario_Organizacion(Guid Id_ususario, Guid Id_organizacion)
        {
            _ctx.Desasignar_Usuario_Organizacion(Id_ususario, Id_organizacion);
        }

        public void SavePreferenciaForUser(Guid idUser, List<Opcion> opciones)
        {
            _ctx.SavePreferenciaForUser(idUser, opciones);
        }

        public IList<Publicacion> GetMatchsForUser(Guid IdUser)
        {
            return GetMatchsForUser(IdUser);
        }
    }
}
