using FindFiddo.Abstractions;
using FindFiddo.DataAccess;
using FindFiddo.Entities;

namespace FindFiddo.Repository
{
    public interface IUserRepository : ICrud<User>
    {
        User GetUserByEmail(string email);
        List<Rol> GetUserRols(Guid idUsuario);
        User signUP(User user);
    }
    public class UserRepository : IUserRepository
    {
        UserContext _ctx;
        public UserRepository() 
        { 
            _ctx = new UserContext();
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

        public User signUP(User user)
        {
            return _ctx.signUP(user);   
        }
    }
}
