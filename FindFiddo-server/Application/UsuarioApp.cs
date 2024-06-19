using FindFiddo.Abstractions;
using FindFiddo.Entities;
using FindFiddo.Repository;
using FindFiddo.Services;

namespace FindFiddo.Application
{
    public interface IUsuarioApp : ICrud<User>
    {
        User GetUserByEmail(string email);
        List<Rol> GetUserRols(Guid idUsuario);
        LogedUser SignUP(User user);
        bool UpdateDVuser(User user);
    }
    public class UsuarioApp : IUsuarioApp
    {
        UserRepository _repo;
        public UsuarioApp(IConfiguration configuration)
        {
            _repo = new UserRepository(configuration);
        }


        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public IList<User> GetAll()
        {
            return _repo.GetAll();
        }

        public User GetById(int id)
        {
            throw new NotImplementedException();
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
            user.DV = DigitoVerificador.CalcularDV(user);

            user.salt = EncryptService.GenerateSalt();
            user.password = EncryptService.Compute(user.password, user.salt);

            return _repo.signUP(user);
        }

        public bool UpdateDVuser(User user)
        {
            return _repo.UpdateDVuser(user);
        }
        
    }
}
