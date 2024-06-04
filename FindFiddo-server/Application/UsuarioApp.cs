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

        User SignUP(User user);
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
            throw new NotImplementedException();
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
                //var roles = _repo.GetUserRols(user.Id);
                //user.rolUsuario = roles.FirstOrDefault().nombre;
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

        public User SignUP(User user)
        {
            user.DV = DigitoVerificador.CalcularDV(user);

            user.salt = EncryptService.GenerateSalt();
            user.password = EncryptService.Compute(user.password, user.salt);

            return _repo.signUP(user);
        }
    }
}
