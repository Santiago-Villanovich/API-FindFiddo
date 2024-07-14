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
    }
    public class UsuarioApp : IUsuarioApp
    {
        UserRepository _repo;
        public UsuarioApp(IConfiguration configuration)
        {
            _repo = new UserRepository(configuration);
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

        
    }
}
