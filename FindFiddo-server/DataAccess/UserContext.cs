using FindFiddo.Abstractions;
using FindFiddo.Entities;
using System;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;

namespace FindFiddo.DataAccess
{
    public interface IUserContext: ICrud<User>
    {
        User GetUserByEmail(string email);
    }
    public class UserContext : IUserContext
    {
        private SqlConnection _conn;

        public UserContext()
        {
            
            _conn = new SqlConnection("Data Source=.;Initial Catalog=FindFiddo_App;Integrated Security=True;TrustServerCertificate=True");
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
            try
            {
                User user = null;

                using SqlCommand cmd = new SqlCommand("GetUserByEmail",_conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@email", email);

                _conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user = new User();
                    user.Id = reader.GetGuid(reader.GetOrdinal("id_usuario"));
                    user.nombres = (string)reader["nombres"];
                    user.apellidos = (string)reader["apellidos"];
                    user.password = (string)reader["clave"];
                    user.dni = (string)reader["dni"];
                    user.email = (string)reader["email"];
                    user.telefono = (string)reader["telefono"];
                    user.fechaNacimiento = reader.GetDateTime(reader.GetOrdinal("fecha_nacimiento"));
                    user.DV = (string)reader["dv"];
                    //user.salt = Convert.FromBase64String((string)reader["salt"]);

                    user.rol = new Rol()
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("id_rol")),
                        nombre = (string)reader["nombre_rol"]
                    };
                }

                return user;
            }
            catch (Exception)
            {
                throw;
            }finally { _conn.Close(); }
        }

        public User Save(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
