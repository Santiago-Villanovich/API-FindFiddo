using FindFiddo.Entities;
using FindFiddo_server.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FindFiddo_server.DataAccess
{
    public interface IPublicacionContext
    {
        void SavePublicacion(Publicacion publicacion);
        IList<TipoPublicacion> GetAllTipos();
        IList<Publicacion> GetPublicacionesByUser(Guid idUser);
        IList<Publicacion> GetPublicaciones(DateTime from, DateTime to, string tipo, int pag);

        void DeleteCategoria(Guid idCategoria);
        Categoria SaveCategoria(Categoria categoria);
        IList<Categoria> GetAllcategorias(string nombre);

        void Dislike_publicacion(Guid id_user, Guid id_publicacion);
        void like_publicacion(Guid id_user, Guid id_publicacion);

        IList<Publicacion> get_matchs(Guid id_user);
    }
    public class PublicacionContext:IPublicacionContext
    {
        private SqlConnection _conn;
        private readonly IConfiguration _config;
        public PublicacionContext(IConfiguration configuration)
        {
            _config = configuration;
            _conn = new SqlConnection(_config.GetConnectionString("default"));
        }

        public void SavePublicacion(Publicacion publicacion)
        {
            bool isNew = false;
            try
            {
                using SqlCommand cmd = new SqlCommand("pub_SavePublicacion", _conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                if(publicacion.Id == Guid.Empty)
                {
                    publicacion.Id = Guid.NewGuid();
                    isNew = true;
                }
                    
                cmd.Parameters.AddWithValue("@id", publicacion.Id);
                cmd.Parameters.AddWithValue("@idUser", publicacion.usuario.Id);
                cmd.Parameters.AddWithValue("@idTipo", publicacion.tipo.Id);
                cmd.Parameters.AddWithValue("@titulo", publicacion.titulo);
                cmd.Parameters.AddWithValue("@historia", publicacion.historia);
                cmd.Parameters.AddWithValue("@descripcion", publicacion.descripcion);
                cmd.Parameters.AddWithValue("@ubicacion", publicacion.ubicacion);

                _conn.Open();
                cmd.ExecuteNonQuery();

                if (isNew && publicacion.opciones.Count()>0) {
                    cmd.CommandText = "pub_SavePublicacionOpcion";

                    foreach (var op in publicacion.opciones)
                    {
                        cmd.Parameters.Clear();

                        cmd.Parameters.AddWithValue("@idPublicacion", publicacion.Id);
                        cmd.Parameters.AddWithValue("@idOpcion", op.Id);

                        cmd.ExecuteNonQuery();
                    }
                }

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
            throw new NotImplementedException();
        }

        public IList<Publicacion> GetPublicacionesByUser(Guid idUser)
        {
            try
            {
                IList<Publicacion> Publicaciones = new List<Publicacion>();

                using SqlCommand cmd = new SqlCommand("pub_GetAllPublicacionByUser", _conn);

                cmd.Parameters.AddWithValue("@idUser", idUser);

                _conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                
                while (reader.Read())
                {
                    Guid id = reader.GetGuid(reader.GetOrdinal("id_publicacion"));
                    Publicacion publi = Publicaciones.FirstOrDefault(p => p.Id.Equals(id));

                    Opcion op = null;
                    if (!reader.IsDBNull(reader.GetOrdinal("opcion_id")))
                    {
                        op = new Opcion(
                            reader.GetGuid(reader.GetOrdinal("opcion_id")),
                            reader.GetString(reader.GetOrdinal("opcion_nombre"))
                        );

                        publi.opciones.Add(op);
                    }

                    if (publi == null)
                    {
                        publi = new Publicacion();
                        publi.Id = id;

                        publi.tipo = new TipoPublicacion(
                            reader.GetGuid(reader.GetOrdinal("tipo_id")),
                            reader.GetString(reader.GetOrdinal("tipo_descripcion"))
                        );

                        publi.titulo = reader.GetString(reader.GetOrdinal("titulo"));
                        publi.descripcion = (string)reader["descripcion"];
                        publi.historia = (string)reader["historia"];
                        publi.ubicacion = reader.GetString(reader.GetOrdinal("ubicacion"));
                        publi.fechaCreacion = reader.GetDateTime(reader.GetOrdinal("fecha_creacion"));

                        publi.opciones = new List<Opcion>();
                        if (op != null)
                            publi.opciones.Add(op);

                        Publicaciones.Add(publi);
                    }
                    else
                    {
                        if(op!=null)
                            publi.opciones.Add(op);
                    }
                }

                return Publicaciones;
            }
            catch (Exception)
            {
                throw;
            }
            finally { _conn.Close(); }
            
        }

        public void DeleteCategoria(Guid idCategoria)
        {
          
            try
            {
                using SqlCommand cmd = new SqlCommand("cat_DeleteCategoria", _conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", idCategoria);

                _conn.Open();
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { _conn.Close(); }
        }

        public Categoria SaveCategoria(Categoria categoria)
        {
            
            try
            {
                using SqlCommand cmd = new SqlCommand("cat_SaveCategoria", _conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                if (categoria.Id == Guid.Empty)
                {
                    categoria.Id = Guid.NewGuid();
                }

                cmd.Parameters.AddWithValue("@id", categoria.Id);
                cmd.Parameters.AddWithValue("@nombre", categoria.nombre);

                _conn.Open();
                cmd.ExecuteNonQuery();

                if (categoria.opciones != null && categoria.opciones.Count > 0) 
                {
                    cmd.CommandText = "cat_SaveCategoriaOpcion";
                    foreach (Opcion op in categoria.opciones)
                    {
                        cmd.Parameters.Clear();
                        if(op.Id == Guid.Empty)
                            op.Id = Guid.NewGuid();

                        cmd.Parameters.AddWithValue("@id", op.Id);
                        cmd.Parameters.AddWithValue("@nombre", op.nombre);
                        cmd.Parameters.AddWithValue("@idCategoria", categoria.Id);

                        cmd.ExecuteNonQuery();
                    }
                }

                return categoria;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { _conn.Close(); }
        }

        public IList<Categoria> GetAllcategorias(string nombre)
        {
            try
            {
                IList<Categoria> categorias = new List<Categoria>();

                using (SqlCommand cmd = new SqlCommand("cat_GetAllCategorias", _conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", !string.IsNullOrEmpty(nombre) ? nombre : (object)DBNull.Value);

                    _conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Guid id = reader.GetGuid(reader.GetOrdinal("categoria_id"));
                        var cat = categorias.FirstOrDefault(c => c.Id.Equals(id));

                        Opcion op = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("opcion_id")))
                        {
                            op = new Opcion(
                            reader.GetGuid(reader.GetOrdinal("opcion_id")),
                            reader.GetString(reader.GetOrdinal("opcion_nombre"))
                            );

                        }

                        if (cat == null)
                        {
                            cat = new Categoria();
                            cat.Id = id;
                            cat.nombre = reader.GetString(reader.GetOrdinal("categoria_nombre"));
                            cat.opciones = new List<Opcion>();

                            if(op != null)
                                cat.opciones.Add(op);

                            categorias.Add(cat);

                        }
                        else
                        {
                            if (op != null)
                                cat.opciones.Add(op);
                        }
                    }
                }

                return categorias;
            }
            catch (Exception)
            {
                throw;
            }
            finally { _conn.Close(); }
        }

        public void Dislike_publicacion(Guid id_user, Guid id_publicacion)
        {
            try
            {
                using SqlCommand cmd = new SqlCommand("save_match", _conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                
         
                Guid Id_match = Guid.NewGuid();
                cmd.Parameters.AddWithValue("@user_id", id_user);
                cmd.Parameters.AddWithValue("@id_publicacion",id_publicacion );
                cmd.Parameters.AddWithValue("@estado", "dislike");
                cmd.Parameters.AddWithValue("@id_match",Id_match);
                cmd.Parameters.AddWithValue("@fecha", DateTime.Now);

                _conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { _conn.Close(); }
        }

        public void like_publicacion(Guid id_user, Guid id_publicacion)
        {
            try
            {
                using SqlCommand cmd = new SqlCommand("save_match", _conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;



                Guid Id_match = Guid.NewGuid();
                cmd.Parameters.AddWithValue("@user_id", id_user);
                cmd.Parameters.AddWithValue("@id_publicacion", id_publicacion);
                cmd.Parameters.AddWithValue("@estado", "like");
                cmd.Parameters.AddWithValue("@id_match", Id_match);
                cmd.Parameters.AddWithValue("@fecha", DateTime.Now);

                _conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { _conn.Close(); }
        }

        public IList<Publicacion> get_matchs(Guid id_user)
        {
            try
            {
                IList<Publicacion> Publicaciones = new List<Publicacion>();

                using SqlCommand cmd = new SqlCommand("get_like_user", _conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_user", id_user);

                _conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

               
                while (reader.Read())
                {
                    Publicacion publi = new Publicacion();
                    publi.Id = reader.GetGuid(reader.GetOrdinal("id_publicacion"));


                   
                        publi.tipo = new TipoPublicacion(
                            reader.GetGuid(reader.GetOrdinal("id_tipo")),
                            reader.GetString(reader.GetOrdinal("tipo_descripcion"))
                        );

                    publi.titulo = reader.IsDBNull(reader.GetOrdinal("titulo")) ? null : reader.GetString(reader.GetOrdinal("titulo"));
                    publi.descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion"));
                    publi.historia = reader.IsDBNull(reader.GetOrdinal("historia")) ? null : reader.GetString(reader.GetOrdinal("historia"));
                    publi.ubicacion = reader.IsDBNull(reader.GetOrdinal("ubicacion")) ? null : reader.GetString(reader.GetOrdinal("ubicacion"));
                    publi.fechaCreacion = reader.IsDBNull(reader.GetOrdinal("fecha_creacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("fecha_creacion"));




                    Publicaciones.Add(publi);
                    
                   
                }

                return Publicaciones;
            }
            catch (Exception)
            {
                throw;
            }
            finally { _conn.Close(); }
        }

        public IList<TipoPublicacion> GetAllTipos()
        {
            try
            {
                IList<TipoPublicacion> categorias = new List<TipoPublicacion>();

                using (SqlCommand cmd = new SqlCommand("pub_GetAllTipos", _conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    _conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    TipoPublicacion tipo = null;

                    while (reader.Read())
                    {
                        tipo = new TipoPublicacion(
                            reader.GetGuid(reader.GetOrdinal("id_tipo")),
                            reader.GetString(reader.GetOrdinal("descripcion"))
                        );

                        categorias.Add(tipo);   
                    }
                }

                return categorias;
            }
            catch (Exception)
            {
                throw;
            }
            finally { _conn.Close(); }
        }
    }
}
