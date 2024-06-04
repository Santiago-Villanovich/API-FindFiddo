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
        List<Rol> GetUserRols(Guid idUsuario);
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
            var user = _repo.GetUserByEmail(email);
            if (user != null)
            {
                var roles = _repo.GetUserRols(user.Id);
                user.rolUsuario = roles.FirstOrDefault().nombre;
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
    }
}
