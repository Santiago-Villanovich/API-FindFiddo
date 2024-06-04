using FindFiddo.Abstractions;
using FindFiddo.Entities;
using System.Data.SqlClient;


namespace FindFiddo.DataAccess
{
    public interface IUserContext : ICrud<User>
    {
        User GetUserByEmail(string email);
        List<Rol> GetUserRols(Guid idUsuario);
        User signUP(User user);
    }
    public class UserContext : IUserContext
    {
        private SqlConnection _conn;
        private readonly IConfiguration _config;
        public UserContext(IConfiguration configuration)
        {
            _config = configuration;
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
                _conn = new SqlConnection(_config.GetConnectionString("default"));
                User user = null;

                using SqlCommand cmd = new SqlCommand("GetUserByEmail", _conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@email", email);

                _conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user = new();
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

                }

                return user;
            }
            catch (Exception)
            {
                throw;
            }
            finally { _conn.Close(); }
        }

        public List<Rol> GetUserRols(Guid idUsuario)
        {
            List<Rol> list = new List<Rol>();
            try
            {
                Rol rol = null;

                using SqlCommand cmd = new SqlCommand("GetUserRoles", _conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

                _conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    rol = new Rol()
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("id_usuario")),
                        nombre = reader.GetString(reader.GetOrdinal("nombre_rol"))
                    };

                    list.Add(rol);
                }

                return list;
            }
            catch (Exception)
            {
                throw;
            }
            finally { _conn.Close(); }
        }

        public User Save(User entity)
        {
            throw new NotImplementedException();
        }

        public User signUP(User user)
        {
            try
            {
                using SqlCommand cmd = new SqlCommand("InsertUser", _conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id_rol", user.rol.Id);
                cmd.Parameters.AddWithValue("@nombres", user.nombres);
                cmd.Parameters.AddWithValue("@apellidos", user.apellidos);
                cmd.Parameters.AddWithValue("@dni", user.dni);
                cmd.Parameters.AddWithValue("@email", user.email);
                cmd.Parameters.AddWithValue("@telefono", user.telefono);
                cmd.Parameters.AddWithValue("@clave", user.password);
                cmd.Parameters.AddWithValue("@fecha_nacimiento", user.fechaNacimiento);
                cmd.Parameters.AddWithValue("@direccion", user.direccion);
                cmd.Parameters.AddWithValue("@codigo_postal", user.codigoPostal);
                cmd.Parameters.AddWithValue("@dv", user.DV);
                cmd.Parameters.AddWithValue("@salt", user.salt);
                cmd.Parameters.AddWithValue("@fecha_creacion", DateTime.Now);


                _conn.Open();
                var result = cmd.ExecuteScalar().ToString();

                Guid id;
                if (Guid.TryParse(result, out id))
                {
                    user.Id = id;
                    return user;
                }
                else
                {
                    throw new Exception("Ya se encuentra registrado un usuario con el mail");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _conn.Close();
            }

        }
    }
}
