using FindFiddo.Abstractions;
using FindFiddo.DataAccess;
using FindFiddo.Entities;

namespace FindFiddo.Repository
{
    public interface IUserRepository : ICrud<User>
    {
        User GetUserByEmail(string email);
        List<Rol> GetUserRols(Guid idUsuario);
        LogedUser signUP(User user);
    }
    public class UserRepository : IUserRepository
    {
        UserContext _ctx;
        private readonly IConfiguration _configuration;
        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _ctx = new UserContext(configuration);
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public IList<User> GetAll()
        {
            return _ctx.GetAllUsuarios();
        }

        public User GetById(int id)
        {
            throw new NotImplementedException();
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
    }
}
