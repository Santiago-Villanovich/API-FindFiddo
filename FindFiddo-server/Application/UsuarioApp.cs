using FindFiddo.Abstractions;
using FindFiddo.Entities;
using FindFiddo.Repository;
using FindFiddo.Services;
using FindFiddo_server.Entities;

namespace FindFiddo.Application
{
    public interface IUsuarioApp : ICrud<User>
    {
        User GetUserByEmail(string email);
        List<Rol> GetUserRols(Guid idUsuario);
        LogedUser SignUP(User user);
        void InsertUserLog(Guid id, string accion);
        List<UserLog> GetLog(DateTime from, DateTime to, string accion, int pag);

        Organizacion SaveOrganizacion(Organizacion organizacion);
        void DeleteOrganizacion(Guid id_organizacion);
        IList<Organizacion> GetAllOrganizaciones(Guid idUser);
        Organizacion getOrganizacionByID(Guid id);

        void Asignar_Usuario_Organizacion(Guid Id_ususario, Guid Id_organizacion);
        void Desasignar_Usuario_Organizacion(Guid Id_ususario, Guid Id_organizacion);

        void SavePreferenciaForUser(Guid idUser, List<Opcion> opciones);

        IList<Publicacion> GetMatchsForUser(Guid IdUser);

    }
    public class UsuarioApp : IUsuarioApp
    {
        IUserRepository _repo;
        public UsuarioApp(IUserRepository repo)
        {
            _repo = repo;
        }


        public void DeleteById(Guid id)
        {
            _repo.DeleteById(id);
        }

        public IList<User> GetAll()
        {
            return _repo.GetAll();
        }

        public User GetById(Guid id)
        {
            return _repo.GetById(id);
        }

        public List<UserLog> GetLog(DateTime from, DateTime to, string accion, int pag)
        {
            return _repo.GetLog(from, to, accion, pag);
        }

        public void InsertUserLog(Guid id, string accion)
        {
            _repo.InsertUserLog(id, accion);
        }

        public User GetUserByEmail(string email)
        {
            var user = _repo.GetUserByEmail(email);
            if (user != null)
            {
                user.rol = _repo.GetUserRols(user.Id);
            }
            return user;
        }

        public List<Rol> GetUserRols(Guid idUsuario)
        {
            return _repo.GetUserRols(idUsuario);
        }

        public User Save(User entity)
        {
            throw new NotImplementedException();
        }

        public LogedUser SignUP(User user)
        {
            user.salt = EncryptService.GenerateSalt();
            user.DV = DigitoVerificador.CalcularDV(user);
            user.password = EncryptService.Compute(user.password, user.salt);

            return _repo.signUP(user);
        }

        public Organizacion SaveOrganizacion(Organizacion organizacion)
        {
            organizacion.DV = DigitoVerificador.CalcularDV(organizacion);
            return _repo.SaveOrganizacion(organizacion);
        }

        public void DeleteOrganizacion(Guid id_organizacion)
        {
            _repo.DeleteOrganizacion(id_organizacion);
        }

        public IList<Organizacion> GetAllOrganizaciones(Guid idUser)
        {
            return _repo.GetAllOrganizaciones(idUser);
        }

        public Organizacion getOrganizacionByID(Guid id)
        {
            return _repo.getOrganizacionByID(id);
        }

        public void Asignar_Usuario_Organizacion(Guid Id_ususario, Guid Id_organizacion)
        {
            _repo.Asignar_Usuario_Organizacion(Id_ususario, Id_organizacion);
        }

        public void Desasignar_Usuario_Organizacion(Guid Id_ususario, Guid Id_organizacion)
        {
            _repo.Desasignar_Usuario_Organizacion(Id_ususario, Id_organizacion);
        }

        public void SavePreferenciaForUser(Guid idUser, List<Opcion> opciones)
        {
            _repo.SavePreferenciaForUser(idUser, opciones);
        }

        public IList<Publicacion> GetMatchsForUser(Guid IdUser)
        {
            return _repo.GetMatchsForUser(IdUser);
        }
    }
}
