using FindFiddo.Abstractions;
using FindFiddo.DataAccess;
using FindFiddo.Entities;

namespace FindFiddo.Repository
{
    public interface IUserRepository : ICrud<User>
    {
        User GetUserByEmail(string email);
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

        public User Save(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
