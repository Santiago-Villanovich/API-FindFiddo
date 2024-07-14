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
    }
}
