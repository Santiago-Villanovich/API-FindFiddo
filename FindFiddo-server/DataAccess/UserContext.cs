using FindFiddo.Abstractions;
using FindFiddo.Entities;
using FindFiddo_server.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data;
using System.Data.SqlClient;


namespace FindFiddo.DataAccess
{
    public interface IUserContext : ICrud<User>
    {
        User GetUserByEmail(string email);
        List<Rol> GetUserRols(Guid idUsuario);
        LogedUser signUP(User user);
        void InsertUserLog(Guid idUser, string accion);
        List<UserLog> GetLog(DateTime from, DateTime to, string accion, int pag);
        void InsertUserRol(Guid idUser, List<Rol> roles);

        void InsertOrganizacion(Organizacion organizacion);
        void DeleteOrganizacion(Guid id_organizacion);
        IList<Organizacion> getOrganizaciones(DateTime from, DateTime to, string accion, int pag);
        Organizacion getOrganizacionByID(Guid id);

        void Asignar_Usuario_Organizacion(Guid Id_ususario, Guid Id_organizacion);

        void Desasignar_Usuario_Organizacion(Guid Id_ususario, Guid Id_organizacion);

        void InsertPublicacion(Publicacion publicacion);

        IList<Publicacion> GetPublicaciones(DateTime from, DateTime to, string tipo, int pag);
    
    }
    public class UserContext : IUserContext
    {
        private SqlConnection _conn;
        private readonly IConfiguration _config;
        public UserContext(IConfiguration configuration)
        {
            _config = configuration;
            _conn = new SqlConnection(_config.GetConnectionString("default"));
        }

        public void DeleteById(Guid id)
        {
            try
            {
                using SqlCommand cmd = new SqlCommand("usr_DeleteUser", _conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idUser", id);

                _conn.Open();
                cmd.ExecuteNonQuery();

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

        public IList<User> GetAll()
        {
            try
            {
                IList<User> Usuarios = new List<User>();

                using SqlCommand cmd = new SqlCommand("usr_GetAllUsers", _conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                

                _conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    User user = new User();
                    user.Id = reader.GetGuid(reader.GetOrdinal("id_usuario"));
                    user.nombres = (string)reader["nombres"];
                    user.apellidos = (string)reader["apellidos"];
                    user.password = (string)reader["clave"];
                    user.dni = (string)reader["dni"];
                    user.email = (string)reader["email"];
                    user.telefono = (string)reader["telefono"];
                    user.fechaNacimiento = reader.GetDateTime(reader.GetOrdinal("fecha_nacimiento"));
                    user.direccion = reader.GetString("direccion");
                    user.salt = (byte[])reader["salt"];
                    user.DV = (string)reader["dv"];
                    Usuarios.Add(user);
                }

                return Usuarios;
            }
            catch (Exception)
            {
                throw;
            }
            finally { _conn.Close(); }
        }
    

        public User GetById(Guid id)
        {
            try
            {
                User usuario = null;
                using SqlCommand cmd = new SqlCommand("usr_GetAllUsers", _conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;


                _conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    usuario = new User();
                    usuario.Id = id;
                    usuario.nombres = (string)reader["nombres"];
                    usuario.apellidos = (string)reader["apellidos"];
                    usuario.password = (string)reader["clave"];
                    usuario.dni = (string)reader["dni"];
                    usuario.email = (string)reader["email"];
                    usuario.telefono = (string)reader["telefono"];
                    usuario.fechaNacimiento = reader.GetDateTime(reader.GetOrdinal("fecha_nacimiento"));
                    usuario.DV = (string)reader["dv"];
                    usuario.salt = (byte[])reader["salt"];
                }

                return usuario;
            }
            catch (Exception)
            {
                throw;
            }
            finally { _conn.Close(); }
        }

        public User GetUserByEmail(string email)
        {
            try
            {
                _conn = new SqlConnection(_config.GetConnectionString("default"));
                User user = null;

                using SqlCommand cmd = new SqlCommand("usr_GetUserByEmail", _conn);

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
                    user.salt = (byte[])reader["salt"];

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

                using SqlCommand cmd = new SqlCommand("usr_GetUserRoles", _conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

                _conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    rol = new Rol()
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("id_rol")),
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

        public LogedUser signUP(User user)
        {
            try
            {
                using SqlCommand cmd = new SqlCommand("usr_InsertUser", _conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

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
                    return new LogedUser(id,user.telefono,user.email,user.nombres,user.apellidos);
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


        public void InsertUserLog(Guid idUser, string accion)
        {
            try
            {
                using SqlCommand cmd = new SqlCommand("usr_InsertUserLog", _conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;


                _conn.Open();
                cmd.Parameters.AddWithValue("@idUsuario", idUser);
                cmd.Parameters.AddWithValue("@accion", accion);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { _conn.Close(); }
        }

        public void InsertUserRol(Guid idUser, List<Rol> roles)
        {
            try
            {
                using SqlCommand cmd = new SqlCommand("usr_SetUsuarioRol", _conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;


                _conn.Open();
                foreach (Rol rol in roles)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@idUsuario", idUser);
                    cmd.Parameters.AddWithValue("@idRol", rol.Id);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { _conn.Close(); }
        }

        public List<UserLog> GetLog(DateTime from, DateTime to, string accion, int pag)
        {
            List<UserLog> list = new List<UserLog>();
            try
            {
                UserLog log = null;

                using SqlCommand cmd = new SqlCommand("log_GetUserFiltered", _conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@from", DBNull.Value);
                cmd.Parameters.AddWithValue("@to", DBNull.Value);
                cmd.Parameters.AddWithValue("@action", !string.IsNullOrEmpty(accion)? accion : DBNull.Value);
                cmd.Parameters.AddWithValue("@page", pag);


                _conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    log = new UserLog()
                    {
                        Id = reader.GetGuid("id_log"),
                        accion = reader.GetString("accion"),
                        fecha = Convert.ToDateTime(reader["fecha"]),
                        user = new LogedUser()
                        {
                            Id = reader.GetGuid("id_usuario"),
                            nombres = reader.GetString("nombres"),
                            apellidos = reader.GetString("apellidos"),
                            email = reader.GetString("email"),
                        }
                    };

                    list.Add(log);
                }

                return list;
            }
            catch (Exception)
            {
                throw;
            }
            finally { _conn.Close(); }
        }

        public void InsertOrganizacion(Organizacion organizacion)
        {
            try
            {
                using SqlCommand cmd = new SqlCommand("insert_organizacion", _conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;


                _conn.Open();
                
               
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@idOrganizacion", organizacion.Id);
                cmd.Parameters.AddWithValue("@direccion", organizacion.direccion);
                cmd.Parameters.AddWithValue("@fecha_creacion", organizacion.fechaCreacion);
                cmd.Parameters.AddWithValue("@razon_social", organizacion.razon_social);
                cmd.Parameters.AddWithValue("@codigo_postal", organizacion.codigo_postal);
                cmd.Parameters.AddWithValue("@digito_Verificador", organizacion.digito_verificador);
                cmd.Parameters.AddWithValue("@nombre", organizacion.nombre);

                cmd.ExecuteNonQuery();
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { _conn.Close(); }
        }

        public void DeleteOrganizacion(Guid id_organizacion)
        {
            try
            {
                using SqlCommand cmd = new SqlCommand("usr_DeleteOrganizacion", _conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idOrganizacion", id_organizacion);

                _conn.Open();
                cmd.ExecuteNonQuery();

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

        public IList<Organizacion> getOrganizaciones(DateTime from, DateTime to, string accion, int pag)
        {
            List<Organizacion> list = new List<Organizacion>();
            try
            {
                Organizacion org = null;

                using SqlCommand cmd = new SqlCommand("get_all_organizaciones", _conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@from", DBNull.Value);
                cmd.Parameters.AddWithValue("@to", DBNull.Value);
                cmd.Parameters.AddWithValue("@action", !string.IsNullOrEmpty(accion) ? accion : DBNull.Value);
                cmd.Parameters.AddWithValue("@page", pag);


                _conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    org = new Organizacion()
                    {
                        Id = reader.GetGuid("id_org"),
                        nombre = reader.GetString("nombre"),
                        razon_social=reader.GetString("razon_social"),
                        direccion=reader.GetString("direccion"),
                        codigo_postal=reader.GetInt32("codigo_postal"),
                        digito_verificador=reader.GetString("digito_verificador"),
                        fechaCreacion = Convert.ToDateTime(reader["fecha_creacion"])
                       
                    };

                    list.Add(org);
                }

                return list;
            }
            catch (Exception)
            {
                throw;
            }
            finally { _conn.Close(); }
        }

        public Organizacion getOrganizacionByID(Guid id)
        {
            List<Organizacion> list = new List<Organizacion>();
            try
            {
                Organizacion org = null;

                using SqlCommand cmd = new SqlCommand("get_organizacion_by_id", _conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id",id);


                _conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    org = new Organizacion()
                    {
                        Id = reader.GetGuid("id_org"),
                        nombre = reader.GetString("nombre"),
                        razon_social = reader.GetString("razon_social"),
                        direccion = reader.GetString("direccion"),
                        codigo_postal = reader.GetInt32("codigo_postal"),
                        digito_verificador = reader.GetString("digito_verificador"),
                        fechaCreacion = Convert.ToDateTime(reader["fecha_creacion"])

                    };

                   
                }
                return org;

            }
            catch (Exception)
            {
                throw;
            }
            finally { _conn.Close(); }
        }

        public void Asignar_Usuario_Organizacion(Guid Id_ususario, Guid Id_organizacion)
        {
            try
            {
                using SqlCommand cmd = new SqlCommand("Asignar_usuario_organizacion", _conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idUser", Id_ususario);
                cmd.Parameters.AddWithValue("@idOrg", Id_organizacion);

                _conn.Open();
                cmd.ExecuteNonQuery();

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

        public void Desasignar_Usuario_Organizacion(Guid Id_ususario, Guid Id_organizacion)
        {
            try
            {
                using SqlCommand cmd = new SqlCommand("desasignar_usuario_organizacion", _conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idUser", Id_ususario);
                cmd.Parameters.AddWithValue("@idOrg", Id_organizacion);

                _conn.Open();
                cmd.ExecuteNonQuery();

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

        public void InsertPublicacion(Publicacion publicacion)
        {
            try
            {
                using SqlCommand cmd = new SqlCommand("insert_organizacion", _conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;


                _conn.Open();


                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@id_publicacion", publicacion.Id);
                cmd.Parameters.AddWithValue("@ubicacion", publicacion.ubicacion);
                cmd.Parameters.AddWithValue("@fecha_creacion", publicacion.fechaCreacion);
                cmd.Parameters.AddWithValue("@insert_time", DateTime.Now);
                cmd.Parameters.AddWithValue("@tipo", publicacion.tipo);
                cmd.Parameters.AddWithValue("@descripcion", publicacion.descripcion);
                

                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { _conn.Close(); }
        }

        public IList<Publicacion> GetPublicaciones(DateTime from, DateTime to, string tipo, int pag)
        {
            try
            {
                IList<Publicacion> Publicaciones = new List<Publicacion>();

                using SqlCommand cmd = new SqlCommand("usr_Publicacion", _conn);

                cmd.Parameters.AddWithValue("@from", from);
                cmd.Parameters.AddWithValue("@to", to);
                cmd.Parameters.AddWithValue("@tipo", tipo);
                cmd.Parameters.AddWithValue("@pag", pag);


                _conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    
                    Publicacion publi = new Publicacion();
                    publi.Id = reader.GetGuid(reader.GetOrdinal("id_publicacion"));
                    publi.descripcion = (string)reader["descripcion"];
                    publi.tipo= (string)reader["tipo"];
                    publi.historia = (string)reader["historia"];
                    publi.fechaCreacion = reader.GetDateTime(reader.GetOrdinal("fecha_nacimiento"));
                    publi.Fecha_Alta = reader.GetDateTime(reader.GetOrdinal("fecha_alta"));
                    publi.ubicacion = reader.GetString("ubicacion");
                    
                }

                return Publicaciones;
            }
            catch (Exception)
            {
                throw;
            }
            finally { _conn.Close(); }
        }
    }
}
