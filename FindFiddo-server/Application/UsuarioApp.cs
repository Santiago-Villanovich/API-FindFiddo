using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FindFiddo.Abstractions;
using FindFiddo.Entities;
using FindFiddo.Repository;
using FindFiddo.Services;

namespace FindFiddo.Application
{
    public interface IUsuarioApp: ICrud<User>
    {
        User GetUserByEmail(string email);
    }
    public class UsuarioApp : IUsuarioApp
    {
        UserRepository _repo;
        public UsuarioApp() 
        {
            _repo = new UserRepository();
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
            return _repo.GetUserByEmail(email);
        }

        public User Save(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
